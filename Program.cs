using IndependentReserve.DotNetClientApi.Data;
using PortfolioValue.Data.Configuration;
using PortfolioValue.Services;

// todo: get config from arguments
// todo: convert the total AUD value to USD value

var config = new IrPortfolioValueConfiguration
{
    ApiUrl = "https://api.independentreserve.com"
    , ApiKey =  ""
    , ApiSecret = ""
    , Currency = CurrencyCode.Aud
};

var portfolioService = new PortfolioService(config);

var portfolio = await portfolioService.GetPortfolio();

Console.WriteLine();
Console.WriteLine(portfolio);
