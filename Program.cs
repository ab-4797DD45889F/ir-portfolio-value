using CommandLine;
using IndependentReserve.DotNetClientApi.Data;
using PortfolioValue.Config;
using PortfolioValue.Data.Configuration;
using PortfolioValue.Services;

// todo: process all fiat currencies, don't convert them into the given one, show as is because Independent Reserve doesn't give a way to directly convert one fiat into another
// todo: validate currency code
// todo: api client should be reworked to the interface so that it could be mocked
// todo: add custom validation
// todo: process --help argument
// todo: process --version argument
// todo: check working with arguments for docker

var result = Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options =>
    {
        // all seems good, but we validate the currency to be parsed and to represent fiat currency
    })
    .WithNotParsed(errors =>
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"Input argument error: {error}");
        }
    });

if (result.Value == null)
{
    return;
}

var options = result.Value;
Console.WriteLine(options);

// todo: try parse and throw user friendly message
var currency = (CurrencyCode)Enum.Parse(typeof(CurrencyCode), options.Currency, ignoreCase: true);

var config = new IrPortfolioValueConfiguration
{
    ApiUrl = "https://api.independentreserve.com"
    , ApiKey = options.Key
    , ApiSecret = options.Secret
    , Currency = currency
};

var portfolioService = new PortfolioService(config);

var portfolio = await portfolioService.GetPortfolio();

Console.WriteLine();
Console.WriteLine(portfolio);
