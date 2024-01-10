using IndependentReserve.DotNetClientApi.Data;

namespace PortfolioValue.Data;

public class AccountWrapper : ApiDataWrapper<Account>, IComparable<AccountWrapper>
{
    public AccountWrapper(Account original) : base(original)
    {
    }

    public bool IsEmpty() => Original.AvailableBalance == 0m && Original.TotalBalance == 0m;

    public CurrencyAmount Total() => new CurrencyAmount(Original.CurrencyCode, Original.TotalBalance);

    /// <summary>
    /// Market value based on the order book or fx rate (for fiat currencies).
    /// </summary>
    public CurrencyAmount MarketValue { get; set; }

    public int CompareTo(AccountWrapper? other)
    {
        if (other == null)
        {
            return 0;
        }

        return MarketValue.CompareTo(other.MarketValue);
    }

    public override string ToString() => $"{this.Total()}";
}
