using IndependentReserve.DotNetClientApi.Data;

namespace PortfolioValue.Data;

public struct CurrencyAmount : IComparable<CurrencyAmount>
{
    public CurrencyAmount(CurrencyCode currencyCode, decimal amount)
    {
        CurrencyCode = currencyCode;
        Amount = amount;
    }

    public CurrencyCode CurrencyCode { get; private set; }

    public decimal Amount { get; private set; }

        private static void ValidateCurrencyCodes(CurrencyAmount left, CurrencyAmount right)
    {
        // Check if the CurrencyCodes are the same
        if (left.CurrencyCode != right.CurrencyCode)
        {
            throw new InvalidOperationException("Cannot compare CurrencyAmount with different CurrencyCodes.");
        }
    }

    public static bool operator ==(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return left.Amount == right.Amount;
    }

    public static bool operator !=(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return left.Amount != right.Amount;
    }

    public static bool operator >(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return left.Amount > right.Amount;
    }

    public static bool operator <(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return left.Amount < right.Amount;
    }

    public static bool operator >=(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return left.Amount >= right.Amount;
    }

    public static bool operator <=(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return left.Amount <= right.Amount;
    }

    public static CurrencyAmount operator +(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return new CurrencyAmount(left.CurrencyCode, left.Amount + right.Amount);
    }

    public static CurrencyAmount operator -(CurrencyAmount left, CurrencyAmount right)
    {
        ValidateCurrencyCodes(left, right);
        return new CurrencyAmount(left.CurrencyCode, left.Amount - right.Amount);
    }

    public static CurrencyAmount operator *(CurrencyAmount left, decimal right)
    {
        return new CurrencyAmount(left.CurrencyCode, left.Amount * right);
    }

    public static CurrencyAmount operator /(CurrencyAmount left, decimal right)
    {
        return new CurrencyAmount(left.CurrencyCode, left.Amount / right);
    }

    public int CompareTo(CurrencyAmount other)
    {
        ValidateCurrencyCodes(this, other);
        return this.Amount.CompareTo(other.Amount);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is CurrencyAmount))
            return false;

        var other = (CurrencyAmount)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CurrencyCode, Amount);
    }

    public override string ToString() => $"{CurrencyCode} {Amount}";

    public string ToStringUsDollar() => $"${Amount:N2}";
}

public static class CurrencyAmountExtensions
{
    public static CurrencyAmount Currency(this decimal d, CurrencyCode currencyCode) => new CurrencyAmount(currencyCode, d);

    public static CurrencyAmount Sum<TSource>(this IEnumerable<TSource> amounts, Func<TSource, CurrencyAmount> getAmount)
    {
        var arrayAmounts = amounts as TSource[] ?? amounts.ToArray();

        if (arrayAmounts.Length == 0) throw new Exception("Non-empty list should be provided");

        var zero = 0m.Currency(getAmount(arrayAmounts.First()).CurrencyCode);

        return arrayAmounts.Aggregate(zero, (current, item) => current + getAmount(item));
    }
}
