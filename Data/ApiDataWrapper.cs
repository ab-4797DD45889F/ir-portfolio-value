namespace PortfolioValue.Data;

public class ApiDataWrapper<T> where T: new()
{
    public ApiDataWrapper(T original)
    {
        Original = original;
    }

    public T Original { get; set; }
}
