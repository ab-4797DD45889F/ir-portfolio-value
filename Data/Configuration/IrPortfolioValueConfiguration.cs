using IndependentReserve.DotNetClientApi.Data;

namespace PortfolioValue.Data.Configuration;

public class IrPortfolioValueConfiguration
{
    public string ApiUrl { get; set; }

    public string ApiKey { get; set; }

    public string ApiSecret { get; set; }

    public CurrencyCode Currency { get; set; }
}
