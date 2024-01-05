// See https://aka.ms/new-console-template for more information


// todo: move to the config

var apiKey = "";
var apiSecret = "";
var url = "https://api.independentreserve.com";

Console.WriteLine("Initializing IR client\n");

var apiConfig = new IndependentReserve.DotNetClientApi.ApiConfig(url, apiKey, apiSecret);
var irClient = IndependentReserve.DotNetClientApi.Client.Create(apiConfig);

Console.WriteLine("Requesting valid secondary currencies");
var secondaryCurrencies = await irClient.GetValidSecondaryCurrencyCodesAsync();
Console.WriteLine($"Supported secondary currencies are: {string.Join(", ", secondaryCurrencies)}\n");

Console.WriteLine("Requesting valid primary currencies");
var primaryCurrencies = await irClient.GetValidPrimaryCurrencyCodesAsync();
Console.WriteLine($"Supported primary currencies are: {string.Join(", ", primaryCurrencies)}\n");

Console.WriteLine("Requesting accounts");
var accounts = await irClient.GetAccountsAsync();
var nonZeroAccounts = accounts.Where(a => a.TotalBalance != 0m).ToArray();

if (nonZeroAccounts.Length <= 0)
{
    Console.WriteLine("All accounts seem to be zero.");
}
else
{
    Console.WriteLine($"Non zero accounts are:\n{string.Join("\n", nonZeroAccounts.Select(a => $"{a.CurrencyCode} {a.TotalBalance}"))}\n");
}

// get order book for each primary
// calculate to sell it at market price
// usd (fiat) should be converted to based on the fx rates
