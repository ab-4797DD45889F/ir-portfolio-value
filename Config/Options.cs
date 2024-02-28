using CommandLine;

namespace PortfolioValue.Config;

public class Options
{
    [Option('k', "key", Required = true, HelpText = "Api key with readonly privileges")]
    public string Key { get; set; }

    [Option('s', "secret", Required = true, HelpText = "Api key secret")]
    public string Secret { get; set; }

    [Option('c', "currency", Required = false, Default = "Aud", HelpText = "Currency code to be used for calculation.")]
    public string Currency { get; set; }

    public override string ToString() => $"Currency={Currency}, Key={Key}, Secret={Secret}";
}
