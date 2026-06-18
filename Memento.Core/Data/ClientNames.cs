namespace Memento.Core.Data;

public static class ClientNames
{
    public const string LocalApiClientName = nameof(LocalApiClientName);
    public const string VpnApiClientName = nameof(VpnApiClientName);
    public const string LocalAuthClientName = nameof(LocalAuthClientName);
    public const string VpnAuthClientName = nameof(VpnAuthClientName);

    public static string GetApiClientName(bool shouldUseVpn)
        => shouldUseVpn ? VpnApiClientName : LocalApiClientName;

    public static string GetAuthClientName(bool shouldUseVpn)
        => shouldUseVpn ? VpnAuthClientName : LocalAuthClientName;
}
