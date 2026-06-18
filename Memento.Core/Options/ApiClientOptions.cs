namespace Memento.Core.Options;

public sealed class ApiClientOptions
{
    public string LocalApiHost { get; set; } = "";
    public string VpnApiHost { get; set; } = "";
    public string LocalAuthHost { get; set; } = "";
    public string VpnAuthHost { get; set; } = "";
}
