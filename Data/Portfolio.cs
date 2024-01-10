namespace PortfolioValue.Data;

public class Portfolio
{
    public AccountWrapper[] Accounts { get; set; }

    public CurrencyAmount Total { get; set; }

    public override string ToString()
    {
        if (Accounts == null || Accounts.Length <= 0)
        {
            return $"All accounts seem to be empty.\nTotal value: {Total}";
        }

        var output = $"Portfolio as of ({DateTimeOffset.Now}):\n\n";
        output += string.Join("\n", Accounts.Select(a => $"{a.MarketValue.ToStringUsDollar(),10} | {a.Total()}"));
        output += "\n\n";
        output += $"{Total.ToStringUsDollar(),10} | Total\n";

        return output;
    }
}
