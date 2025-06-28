using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;
using PortfolioValue.Data;
using PortfolioValue.Data.Configuration;

namespace PortfolioValue.Services;

public class PortfolioService(IrPortfolioValueConfiguration configuration)
{
    private Client? _irClient;
    private HashSet<CurrencyCode>? _primaryCurrencies;
    private HashSet<CurrencyCode>? _secondaryCurrencies;
    private IEnumerable<FxRate>? _fxRates;

    /// <summary>
    /// Gets accounts, calculates their market values and returns as a Portfolio object.
    /// </summary>
    /// <returns></returns>
    public async Task<Portfolio> GetPortfolio()
    {
        await InitIfNecessary();

        var portfolio = new Portfolio();

        await LoadAccounts(portfolio);
        await SetMarketValues(portfolio);

        return portfolio;
    }

    private SemaphoreSlim _initLockVictim = new SemaphoreSlim(1, 1);

    private async Task InitIfNecessary()
    {
        // init and load currencies only once
        if (IsInitialized()) return;

        await _initLockVictim.WaitAsync();

        // init load currencies only once
        try
        {
            if (IsInitialized()) return;

            var apiConfig = new ApiConfig(configuration.ApiUrl, configuration.ApiKey, configuration.ApiSecret);

            _irClient = Client.Create(apiConfig);

            await LoadCurrencies();
        }
        finally
        {
            _initLockVictim.Release();
        }
    }

    private bool IsInitialized() => _primaryCurrencies != null && _secondaryCurrencies != null && _primaryCurrencies.Count != 0 && _secondaryCurrencies.Count != 0;

    private async Task LoadCurrencies()
    {
        Console.Write("Requesting valid primary currencies... ");
        var primaryCurrencies = await _irClient.GetValidPrimaryCurrencyCodesAsync();
        _primaryCurrencies = primaryCurrencies.ToHashSet();
        Console.WriteLine($"{string.Join(", ", _primaryCurrencies)}");

        Console.Write("Requesting valid secondary currencies... ");
        var secondaryCurrencies = await _irClient.GetValidSecondaryCurrencyCodesAsync();
        _secondaryCurrencies = secondaryCurrencies.ToHashSet();
        Console.WriteLine($"{string.Join(", ", _secondaryCurrencies)}");
    }

    private async Task LoadAccounts(Portfolio portfolio)
    {
        Console.Write("Requesting accounts... ");
        var accounts = await _irClient.GetAccountsAsync();
        var accountWrappers = accounts.Select(a => new AccountWrapper(a)).ToArray();
        Console.WriteLine($"{accountWrappers.Length} accounts received");
        var nonEmptyAccounts = accountWrappers.Where(a => !a.IsEmpty()).ToArray();

        portfolio.Accounts = nonEmptyAccounts;
    }

    /// <summary>
    /// Goes though the portfolio and calculates market values as well as sorts the accounts by market value desc.
    /// </summary>
    /// <param name="portfolio"></param>
    private async Task SetMarketValues(Portfolio portfolio)
    {
        foreach (var account in portfolio.Accounts)
        {
            account.MarketValue = await GetMarketValue(account);
        }

        portfolio.Accounts = portfolio.Accounts.OrderByDescending(a => a.MarketValue).ToArray();

        // get market value
        portfolio.Total = portfolio.Accounts.Length <= 0
            ? 0m.Currency(configuration.Currency)
            : portfolio.Accounts.Sum(a => a.MarketValue);
    }

    private async Task<CurrencyAmount> GetMarketValue(AccountWrapper account)
    {
        var currency = account.Original.CurrencyCode;

        if (_primaryCurrencies.Contains(currency))
        {
            return await GetOrderBookValue(account);
        }

        if (_secondaryCurrencies.Contains(currency))
        {
            return await GetFiatValue(account);
        }

        Console.WriteLine($"{account} doesn't look like supported primary or secondary currency, ignoring");

        return 0m.Currency(configuration.Currency);
    }

    private async Task<CurrencyAmount> GetOrderBookValue(AccountWrapper account)
    {
        Console.Write($"Requesting {account.Original.CurrencyCode} order book... ");
        var orderBook = await _irClient.GetOrderBookAsync(account.Original.CurrencyCode, configuration.Currency);

        // can't estimate market value if order book is empty
        if (orderBook == null || orderBook.SellOrders == null)
        {
            return 0m.Currency(configuration.Currency);
        }

        Console.WriteLine($"{orderBook.SellOrders.Count} sell orders received");

        var volumeLeft = account.Total().Amount;
        var value = 0m;

        for (var i = 0; i < orderBook.SellOrders.Count; i++)
        {
            var order = orderBook.SellOrders[i];
            var tradeVolume = Math.Min(order.Volume, volumeLeft);
            var tradeValue = order.Price * tradeVolume;

            // todo: take into account brokerage, use GetBrokerageFees method
            value += tradeValue;
            volumeLeft -= tradeVolume;

            if (volumeLeft <= 0)
            {
                break;
            }
        }

        if (volumeLeft > 0)
        {
            Console.WriteLine($"Warning: not enough volume in the order book to sell {account.Total()}, outstanding volume: {volumeLeft}");
        }

        return value.Currency(configuration.Currency);
    }

    private async Task<CurrencyAmount> GetFiatValue(AccountWrapper account)
    {
        if (account.Original.CurrencyCode == configuration.Currency)
        {
            return account.Total();
        }

        // Get FX rate for conversion
        var fxRate = await GetFxRate(account.Original.CurrencyCode, configuration.Currency);
        if (fxRate == null)
        {
            Console.WriteLine($"Warning: No FX rate found for {account.Original.CurrencyCode} to {configuration.Currency}, using 0 value");
            return 0m.Currency(configuration.Currency);
        }

        var originalAmount = account.Total().Amount;
        var convertedValue = originalAmount * fxRate.Rate;
        Console.WriteLine($"FX: {originalAmount} {account.Original.CurrencyCode} -> {convertedValue:F2} {configuration.Currency} (rate: {fxRate.Rate:F4})");
        return convertedValue.Currency(configuration.Currency);
    }

    private async Task<FxRate?> GetFxRate(CurrencyCode fromCurrency, CurrencyCode toCurrency)
    {
        await LoadFxRatesIfNecessary();

        if (_fxRates == null)
        {
            return null;
        }

        // Look for direct rate: fromCurrency -> toCurrency
        var directRate = _fxRates.FirstOrDefault(r => 
            r.CurrencyCodeA == fromCurrency.ToString() && r.CurrencyCodeB == toCurrency.ToString());
        
        if (directRate != null)
        {
            return directRate;
        }

        // Look for inverse rate: toCurrency -> fromCurrency
        var inverseRate = _fxRates.FirstOrDefault(r => 
            r.CurrencyCodeA == toCurrency.ToString() && r.CurrencyCodeB == fromCurrency.ToString());
        
        if (inverseRate != null)
        {
            // Return inverse rate (1/rate)
            return new FxRate
            {
                CurrencyCodeA = fromCurrency.ToString(),
                CurrencyCodeB = toCurrency.ToString(),
                Rate = 1m / inverseRate.Rate
            };
        }

        return null;
    }

    private async Task LoadFxRatesIfNecessary()
    {
        // Only load once if not already loaded
        if (_fxRates != null)
        {
            return;
        }

        Console.Write("Requesting FX rates... ");
        
        try
        {
            _fxRates = await _irClient.GetFxRatesAsync();
            Console.WriteLine($"{_fxRates?.Count() ?? 0} FX rates received");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching FX rates: {ex.Message}");
            _fxRates = null;
        }
    }
}
