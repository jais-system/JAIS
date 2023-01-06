using System.Runtime.CompilerServices;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Connection.DynamicAssemblyName)]
namespace AppCore.Services.ConnectionManager;

internal enum DeviceState : uint
{
    Unknown = 0,
    Unmanaged = 10,
    Unavailable = 20,
    Disconnected = 30,
    Prepare = 40,
    Config = 50,
    NeedAuth = 60,
    IpConfig = 70,
    IpCheck = 80,
    Secondaries = 90,
    Activated = 100,
    Deactivating = 110,
    Failed = 120
}

internal enum ConnectivityState : uint
{
    Unknown = 0,
    None = 1,
    Portal = 2,
    Limited = 3,
    Full = 4
}

[DBusInterface("org.freedesktop.DBus.ObjectManager")]
internal interface IObjectManager : IDBusObject
{
    Task<IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>> GetManagedObjectsAsync();
    Task<IDisposable> WatchInterfacesAddedAsync(Action<(ObjectPath objectPath, IDictionary<string, IDictionary<string, object>> interfacesAndProperties)> handler, Action<Exception> onError = null);
    Task<IDisposable> WatchInterfacesRemovedAsync(Action<(ObjectPath objectPath, string[] interfaces)> handler, Action<Exception> onError = null);
}

[DBusInterface("org.freedesktop.NetworkManager")]
internal interface INetworkManager : IDBusObject
{
    Task ReloadAsync(uint flags);
    Task<IDevice[]> GetDevicesAsync();
    Task<ObjectPath[]> GetAllDevicesAsync();
    Task<ObjectPath> GetDeviceByIpIfaceAsync(string iface);
    Task<ObjectPath> ActivateConnectionAsync(ObjectPath connection, ObjectPath device, ObjectPath specificObject);
    Task<(ObjectPath path, ObjectPath activeConnection)> AddAndActivateConnectionAsync(IDictionary<string, IDictionary<string, object>> connection, ObjectPath device, ObjectPath specificObject);
    Task<(ObjectPath path, ObjectPath activeConnection, IDictionary<string, object> result)> AddAndActivateConnection2Async(IDictionary<string, IDictionary<string, object>> connection, ObjectPath device, ObjectPath specificObject, IDictionary<string, object> options);
    Task DeactivateConnectionAsync(ObjectPath activeConnection);
    Task SleepAsync(bool sleep);
    Task EnableAsync(bool enable);
    Task<IDictionary<string, string>> GetPermissionsAsync();
    Task SetLoggingAsync(string level, string domains);
    Task<(string level, string domains)> GetLoggingAsync();
    Task<ConnectivityState> CheckConnectivityAsync();
    Task<uint> StateAsync();
    Task<ObjectPath> CheckpointCreateAsync(ObjectPath[] devices, uint rollbackTimeout, uint flags);
    Task CheckpointDestroyAsync(ObjectPath checkpoint);
    Task<IDictionary<string, uint>> CheckpointRollbackAsync(ObjectPath checkpoint);
    Task CheckpointAdjustRollbackTimeoutAsync(ObjectPath checkpoint, uint addTimeout);
    Task<IDisposable> WatchCheckPermissionsAsync(Action handler, Action<Exception> onError = null);
    Task<IDisposable> WatchStateChangedAsync(Action<uint> handler, Action<Exception> onError = null);
    Task<IDisposable> WatchDeviceAddedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<IDisposable> WatchDeviceRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<T> GetAsync<T>(string prop);
    Task<NetworkManagerProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class NetworkManagerProperties
{
    public ObjectPath[]? Devices { get; set; } = default(ObjectPath[]);

    public ObjectPath[] AllDevices { get; set; } = default(ObjectPath[]);

    public ObjectPath[] Checkpoints { get; set; } = default(ObjectPath[]);

    public bool NetworkingEnabled { get; set; } = default(bool);

    public bool WirelessEnabled { get; set; } = default(bool);

    public bool WirelessHardwareEnabled { get; set; } = default(bool);

    public bool WwanEnabled { get; set; } = default(bool);

    public bool WwanHardwareEnabled { get; set; } = default(bool);

    public bool WimaxEnabled { get; set; } = default(bool);

    public bool WimaxHardwareEnabled { get; set; } = default(bool);

    public uint RadioFlags { get; set; } = default(uint);

    public ObjectPath[] ActiveConnections { get; set; } = default(ObjectPath[]);

    public ObjectPath PrimaryConnection { get; set; } = default(ObjectPath);

    public string PrimaryConnectionType { get; set; } = default(string);

    public uint Metered { get; set; } = default(uint);

    public ObjectPath ActivatingConnection { get; set; } = default(ObjectPath);

    public bool Startup { get; set; } = default(bool);

    public string Version { get; set; } = default(string);

    public uint[] Capabilities { get; set; } = default(uint[]);

    public uint State { get; set; } = default(uint);

    public uint Connectivity { get; set; } = default(uint);

    public bool ConnectivityCheckAvailable { get; set; } = default(bool);

    public bool ConnectivityCheckEnabled { get; set; } = default(bool);

    public string ConnectivityCheckUri { get; set; } = default(string);

    public IDictionary<string, object> GlobalDnsConfiguration { get; set; } = default(IDictionary<string, object>);
}

internal static class NetworkManagerExtensions
{
    public static Task<ObjectPath[]> GetDevicesAsync(this INetworkManager o) => o.GetAsync<ObjectPath[]>("Devices");
    public static Task<ObjectPath[]> GetAllDevicesAsync(this INetworkManager o) => o.GetAsync<ObjectPath[]>("AllDevices");
    public static Task<ObjectPath[]> GetCheckpointsAsync(this INetworkManager o) => o.GetAsync<ObjectPath[]>("Checkpoints");
    public static Task<bool> GetNetworkingEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("NetworkingEnabled");
    public static Task<bool> GetWirelessEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("WirelessEnabled");
    public static Task<bool> GetWirelessHardwareEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("WirelessHardwareEnabled");
    public static Task<bool> GetWwanEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("WwanEnabled");
    public static Task<bool> GetWwanHardwareEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("WwanHardwareEnabled");
    public static Task<bool> GetWimaxEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("WimaxEnabled");
    public static Task<bool> GetWimaxHardwareEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("WimaxHardwareEnabled");
    public static Task<uint> GetRadioFlagsAsync(this INetworkManager o) => o.GetAsync<uint>("RadioFlags");
    public static Task<ObjectPath[]> GetActiveConnectionsAsync(this INetworkManager o) => o.GetAsync<ObjectPath[]>("ActiveConnections");
    public static Task<ObjectPath> GetPrimaryConnectionAsync(this INetworkManager o) => o.GetAsync<ObjectPath>("PrimaryConnection");
    public static Task<string> GetPrimaryConnectionTypeAsync(this INetworkManager o) => o.GetAsync<string>("PrimaryConnectionType");
    public static Task<uint> GetMeteredAsync(this INetworkManager o) => o.GetAsync<uint>("Metered");
    public static Task<ObjectPath> GetActivatingConnectionAsync(this INetworkManager o) => o.GetAsync<ObjectPath>("ActivatingConnection");
    public static Task<bool> GetStartupAsync(this INetworkManager o) => o.GetAsync<bool>("Startup");
    public static Task<string> GetVersionAsync(this INetworkManager o) => o.GetAsync<string>("Version");
    public static Task<uint[]> GetCapabilitiesAsync(this INetworkManager o) => o.GetAsync<uint[]>("Capabilities");
    public static Task<uint> GetStateAsync(this INetworkManager o) => o.GetAsync<uint>("State");
    public static Task<ConnectivityState> GetConnectivityAsync(this INetworkManager o) => o.GetAsync<ConnectivityState>("Connectivity");
    public static Task<bool> GetConnectivityCheckAvailableAsync(this INetworkManager o) => o.GetAsync<bool>("ConnectivityCheckAvailable");
    public static Task<bool> GetConnectivityCheckEnabledAsync(this INetworkManager o) => o.GetAsync<bool>("ConnectivityCheckEnabled");
    public static Task<string> GetConnectivityCheckUriAsync(this INetworkManager o) => o.GetAsync<string>("ConnectivityCheckUri");
    public static Task<IDictionary<string, object>> GetGlobalDnsConfigurationAsync(this INetworkManager o) => o.GetAsync<IDictionary<string, object>>("GlobalDnsConfiguration");
    public static Task SetWirelessEnabledAsync(this INetworkManager o, bool val) => o.SetAsync("WirelessEnabled", val);
    public static Task SetWwanEnabledAsync(this INetworkManager o, bool val) => o.SetAsync("WwanEnabled", val);
    public static Task SetWimaxEnabledAsync(this INetworkManager o, bool val) => o.SetAsync("WimaxEnabled", val);
    public static Task SetConnectivityCheckEnabledAsync(this INetworkManager o, bool val) => o.SetAsync("ConnectivityCheckEnabled", val);
    public static Task SetGlobalDnsConfigurationAsync(this INetworkManager o, IDictionary<string, object> val) => o.SetAsync("GlobalDnsConfiguration", val);
}

[DBusInterface("org.freedesktop.NetworkManager.DHCP6Config")]
internal interface IDhcp6Config : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<Dhcp6ConfigProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class Dhcp6ConfigProperties
{
    public IDictionary<string, object> Options { get; set; } = default(IDictionary<string, object>);
}

internal static class Dhcp6ConfigExtensions
{
    public static Task<IDictionary<string, object>> GetOptionsAsync(this IDhcp6Config o) => o.GetAsync<IDictionary<string, object>>("Options");
}

[DBusInterface("org.freedesktop.NetworkManager.AccessPoint")]
internal interface IAccessPoint : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<AccessPointProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class AccessPointProperties
{
    public uint Flags { get; set; } = default(uint);

    public uint WpaFlags { get; set; } = default(uint);

    public uint RsnFlags { get; set; } = default(uint);

    public byte[] Ssid { get; set; } = default(byte[]);

    public uint Frequency { get; set; } = default(uint);

    public string HwAddress { get; set; } = default(string);

    public uint Mode { get; set; } = default(uint);

    public uint MaxBitrate { get; set; } = default(uint);

    public byte Strength { get; set; } = default(byte);

    public int LastSeen { get; set; } = default(int);
}

internal static class AccessPointExtensions
{
    public static Task<uint> GetFlagsAsync(this IAccessPoint o) => o.GetAsync<uint>("Flags");
    public static Task<uint> GetWpaFlagsAsync(this IAccessPoint o) => o.GetAsync<uint>("WpaFlags");
    public static Task<uint> GetRsnFlagsAsync(this IAccessPoint o) => o.GetAsync<uint>("RsnFlags");
    public static Task<byte[]> GetSsidAsync(this IAccessPoint o) => o.GetAsync<byte[]>("Ssid");
    public static Task<uint> GetFrequencyAsync(this IAccessPoint o) => o.GetAsync<uint>("Frequency");
    public static Task<string> GetHwAddressAsync(this IAccessPoint o) => o.GetAsync<string>("HwAddress");
    public static Task<uint> GetModeAsync(this IAccessPoint o) => o.GetAsync<uint>("Mode");
    public static Task<uint> GetMaxBitrateAsync(this IAccessPoint o) => o.GetAsync<uint>("MaxBitrate");
    public static Task<byte> GetStrengthAsync(this IAccessPoint o) => o.GetAsync<byte>("Strength");
    public static Task<int> GetLastSeenAsync(this IAccessPoint o) => o.GetAsync<int>("LastSeen");
}

[DBusInterface("org.freedesktop.NetworkManager.IP4Config")]
internal interface IIp4Config : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<Ip4ConfigProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class Ip4ConfigProperties
{
    public uint[][] Addresses { get; set; } = default(uint[][]);

    public IDictionary<string, object>[] AddressData { get; set; } = default(IDictionary<string, object>[]);

    public string Gateway { get; set; } = default(string);

    public uint[][] Routes { get; set; } = default(uint[][]);

    public IDictionary<string, object>[] RouteData { get; set; } = default(IDictionary<string, object>[]);

    public IDictionary<string, object>[] NameserverData { get; set; } = default(IDictionary<string, object>[]);

    public uint[] Nameservers { get; set; } = default(uint[]);

    public string[] Domains { get; set; } = default(string[]);

    public string[] Searches { get; set; } = default(string[]);

    public string[] DnsOptions { get; set; } = default(string[]);

    public int DnsPriority { get; set; } = default(int);

    public string[] WinsServerData { get; set; } = default(string[]);

    public uint[] WinsServers { get; set; } = default(uint[]);
}

internal static class Ip4ConfigExtensions
{
    public static Task<uint[][]> GetAddressesAsync(this IIp4Config o) => o.GetAsync<uint[][]>("Addresses");
    public static Task<IDictionary<string, object>[]> GetAddressDataAsync(this IIp4Config o) => o.GetAsync<IDictionary<string, object>[]>("AddressData");
    public static Task<string> GetGatewayAsync(this IIp4Config o) => o.GetAsync<string>("Gateway");
    public static Task<uint[][]> GetRoutesAsync(this IIp4Config o) => o.GetAsync<uint[][]>("Routes");
    public static Task<IDictionary<string, object>[]> GetRouteDataAsync(this IIp4Config o) => o.GetAsync<IDictionary<string, object>[]>("RouteData");
    public static Task<IDictionary<string, object>[]> GetNameserverDataAsync(this IIp4Config o) => o.GetAsync<IDictionary<string, object>[]>("NameserverData");
    public static Task<uint[]> GetNameserversAsync(this IIp4Config o) => o.GetAsync<uint[]>("Nameservers");
    public static Task<string[]> GetDomainsAsync(this IIp4Config o) => o.GetAsync<string[]>("Domains");
    public static Task<string[]> GetSearchesAsync(this IIp4Config o) => o.GetAsync<string[]>("Searches");
    public static Task<string[]> GetDnsOptionsAsync(this IIp4Config o) => o.GetAsync<string[]>("DnsOptions");
    public static Task<int> GetDnsPriorityAsync(this IIp4Config o) => o.GetAsync<int>("DnsPriority");
    public static Task<string[]> GetWinsServerDataAsync(this IIp4Config o) => o.GetAsync<string[]>("WinsServerData");
    public static Task<uint[]> GetWinsServersAsync(this IIp4Config o) => o.GetAsync<uint[]>("WinsServers");
}

[DBusInterface("org.freedesktop.NetworkManager.Connection.Active")]
internal interface IActive : IDBusObject
{
    Task<IDisposable> WatchStateChangedAsync(Action<(uint state, uint reason)> handler, Action<Exception> onError = null);
    Task<T> GetAsync<T>(string prop);
    Task<ActiveProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class ActiveProperties
{
    public ObjectPath Connection { get; set; } = default(ObjectPath);

    public ObjectPath SpecificObject { get; set; } = default(ObjectPath);

    public string Id { get; set; } = default(string);

    public string Uuid { get; set; } = default(string);

    public string Type { get; set; } = default(string);

    public ObjectPath[] Devices { get; set; } = default(ObjectPath[]);

    public uint State { get; set; } = default(uint);

    public uint StateFlags { get; set; } = default(uint);

    public bool Default { get; set; } = default(bool);

    public ObjectPath Ip4Config { get; set; } = default(ObjectPath);

    public ObjectPath Dhcp4Config { get; set; } = default(ObjectPath);

    public bool Default6 { get; set; } = default(bool);

    public ObjectPath Ip6Config { get; set; } = default(ObjectPath);

    public ObjectPath Dhcp6Config { get; set; } = default(ObjectPath);

    public bool Vpn { get; set; } = default(bool);

    public ObjectPath Master { get; set; } = default(ObjectPath);
}

internal static class ActiveExtensions
{
    public static Task<ObjectPath> GetConnectionAsync(this IActive o) => o.GetAsync<ObjectPath>("Connection");
    public static Task<ObjectPath> GetSpecificObjectAsync(this IActive o) => o.GetAsync<ObjectPath>("SpecificObject");
    public static Task<string> GetIdAsync(this IActive o) => o.GetAsync<string>("Id");
    public static Task<string> GetUuidAsync(this IActive o) => o.GetAsync<string>("Uuid");
    public static Task<string> GetTypeAsync(this IActive o) => o.GetAsync<string>("Type");
    public static Task<ObjectPath[]> GetDevicesAsync(this IActive o) => o.GetAsync<ObjectPath[]>("Devices");
    public static Task<uint> GetStateAsync(this IActive o) => o.GetAsync<uint>("State");
    public static Task<uint> GetStateFlagsAsync(this IActive o) => o.GetAsync<uint>("StateFlags");
    public static Task<bool> GetDefaultAsync(this IActive o) => o.GetAsync<bool>("Default");
    public static Task<ObjectPath> GetIp4ConfigAsync(this IActive o) => o.GetAsync<ObjectPath>("Ip4Config");
    public static Task<ObjectPath> GetDhcp4ConfigAsync(this IActive o) => o.GetAsync<ObjectPath>("Dhcp4Config");
    public static Task<bool> GetDefault6Async(this IActive o) => o.GetAsync<bool>("Default6");
    public static Task<ObjectPath> GetIp6ConfigAsync(this IActive o) => o.GetAsync<ObjectPath>("Ip6Config");
    public static Task<ObjectPath> GetDhcp6ConfigAsync(this IActive o) => o.GetAsync<ObjectPath>("Dhcp6Config");
    public static Task<bool> GetVpnAsync(this IActive o) => o.GetAsync<bool>("Vpn");
    public static Task<ObjectPath> GetMasterAsync(this IActive o) => o.GetAsync<ObjectPath>("Master");
}

[DBusInterface("org.freedesktop.NetworkManager.AgentManager")]
internal interface IAgentManager : IDBusObject
{
    Task RegisterAsync(string identifier);
    Task RegisterWithCapabilitiesAsync(string identifier, uint capabilities);
    Task UnregisterAsync();
}

[DBusInterface("org.freedesktop.NetworkManager.DHCP4Config")]
internal interface IDhcp4Config : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<Dhcp4ConfigProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class Dhcp4ConfigProperties
{
    public IDictionary<string, object> Options { get; set; } = default(IDictionary<string, object>);
}

internal static class Dhcp4ConfigExtensions
{
    public static Task<IDictionary<string, object>> GetOptionsAsync(this IDhcp4Config o) => o.GetAsync<IDictionary<string, object>>("Options");
}

[DBusInterface("org.freedesktop.NetworkManager.Device.Statistics")]
internal interface IStatistics : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<StatisticsProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class StatisticsProperties
{
    public uint RefreshRateMs { get; set; } = default(uint);

    public ulong TxBytes { get; set; } = default(ulong);

    public ulong RxBytes { get; set; } = default(ulong);
}

internal static class StatisticsExtensions
{
    public static Task<uint> GetRefreshRateMsAsync(this IStatistics o) => o.GetAsync<uint>("RefreshRateMs");
    public static Task<ulong> GetTxBytesAsync(this IStatistics o) => o.GetAsync<ulong>("TxBytes");
    public static Task<ulong> GetRxBytesAsync(this IStatistics o) => o.GetAsync<ulong>("RxBytes");
    public static Task SetRefreshRateMsAsync(this IStatistics o, uint val) => o.SetAsync("RefreshRateMs", val);
}

[DBusInterface("org.freedesktop.NetworkManager.Device.Bridge")]
internal interface IBridge : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<BridgeProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class BridgeProperties
{
    public string HwAddress { get; set; } = default(string);

    public bool Carrier { get; set; } = default(bool);

    public ObjectPath[] Slaves { get; set; } = default(ObjectPath[]);
}

internal static class BridgeExtensions
{
    public static Task<string> GetHwAddressAsync(this IBridge o) => o.GetAsync<string>("HwAddress");
    public static Task<bool> GetCarrierAsync(this IBridge o) => o.GetAsync<bool>("Carrier");
    public static Task<ObjectPath[]> GetSlavesAsync(this IBridge o) => o.GetAsync<ObjectPath[]>("Slaves");
}

[DBusInterface("org.freedesktop.NetworkManager.Device")]
internal interface IDevice : IDBusObject
{
    Task ReapplyAsync(IDictionary<string, IDictionary<string, object>> connection, ulong versionId, uint flags);
    Task<(IDictionary<string, IDictionary<string, object>> connection, ulong versionId)> GetAppliedConnectionAsync(uint flags);
    Task DisconnectAsync();
    Task DeleteAsync();
    Task<IDisposable> WatchStateChangedAsync(Action<(DeviceState newState, DeviceState oldState, uint reason)> handler, Action<Exception> onError = null);
    Task<T> GetAsync<T>(string prop);
    Task<DeviceProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class DeviceProperties
{
    public string Udi { get; set; } = default(string);

    public string Path { get; set; } = default(string);

    public string Interface { get; set; } = default(string);

    public string IpInterface { get; set; } = default(string);

    public string Driver { get; set; } = default(string);

    public string DriverVersion { get; set; } = default(string);

    public string FirmwareVersion { get; set; } = default(string);

    public uint Capabilities { get; set; } = default(uint);

    public uint Ip4Address { get; set; } = default(uint);

    public uint State { get; set; } = default(uint);

    public (uint, uint) StateReason { get; set; } = default((uint, uint));

    public ObjectPath ActiveConnection { get; set; } = default(ObjectPath);

    public ObjectPath Ip4Config { get; set; } = default(ObjectPath);

    public ObjectPath Dhcp4Config { get; set; } = default(ObjectPath);

    public ObjectPath Ip6Config { get; set; } = default(ObjectPath);

    public ObjectPath Dhcp6Config { get; set; } = default(ObjectPath);

    public bool Managed { get; set; } = default(bool);

    public bool Autoconnect { get; set; } = default(bool);

    public bool FirmwareMissing { get; set; } = default(bool);

    public bool NmPluginMissing { get; set; } = default(bool);

    public uint DeviceType { get; set; } = default(uint);

    public ObjectPath[] AvailableConnections { get; set; } = default(ObjectPath[]);

    public string PhysicalPortId { get; set; } = default(string);

    public uint Mtu { get; set; } = default(uint);

    public uint Metered { get; set; } = default(uint);

    public IDictionary<string, object>[] LldpNeighbors { get; set; } = default(IDictionary<string, object>[]);

    public bool Real { get; set; } = default(bool);

    public uint Ip4Connectivity { get; set; } = default(uint);

    public uint Ip6Connectivity { get; set; } = default(uint);

    public uint InterfaceFlags { get; set; } = default(uint);

    public string HwAddress { get; set; } = default(string);

    public ObjectPath[] Ports { get; set; } = default(ObjectPath[]);
}

internal static class DeviceExtensions
{
    public static Task<string> GetUdiAsync(this IDevice o) => o.GetAsync<string>("Udi");
    public static Task<string> GetPathAsync(this IDevice o) => o.GetAsync<string>("Path");
    public static Task<string> GetInterfaceAsync(this IDevice o) => o.GetAsync<string>("Interface");
    public static Task<string> GetIpInterfaceAsync(this IDevice o) => o.GetAsync<string>("IpInterface");
    public static Task<string> GetDriverAsync(this IDevice o) => o.GetAsync<string>("Driver");
    public static Task<string> GetDriverVersionAsync(this IDevice o) => o.GetAsync<string>("DriverVersion");
    public static Task<string> GetFirmwareVersionAsync(this IDevice o) => o.GetAsync<string>("FirmwareVersion");
    public static Task<uint> GetCapabilitiesAsync(this IDevice o) => o.GetAsync<uint>("Capabilities");
    public static Task<uint> GetIp4AddressAsync(this IDevice o) => o.GetAsync<uint>("Ip4Address");
    public static Task<DeviceState> GetStateAsync(this IDevice o) => o.GetAsync<DeviceState>("State");
    public static Task<(uint, uint)> GetStateReasonAsync(this IDevice o) => o.GetAsync<(uint, uint)>("StateReason");
    public static Task<ObjectPath> GetActiveConnectionAsync(this IDevice o) => o.GetAsync<ObjectPath>("ActiveConnection");
    public static Task<ObjectPath> GetIp4ConfigAsync(this IDevice o) => o.GetAsync<ObjectPath>("Ip4Config");
    public static Task<ObjectPath> GetDhcp4ConfigAsync(this IDevice o) => o.GetAsync<ObjectPath>("Dhcp4Config");
    public static Task<ObjectPath> GetIp6ConfigAsync(this IDevice o) => o.GetAsync<ObjectPath>("Ip6Config");
    public static Task<ObjectPath> GetDhcp6ConfigAsync(this IDevice o) => o.GetAsync<ObjectPath>("Dhcp6Config");
    public static Task<bool> GetManagedAsync(this IDevice o) => o.GetAsync<bool>("Managed");
    public static Task<bool> GetAutoconnectAsync(this IDevice o) => o.GetAsync<bool>("Autoconnect");
    public static Task<bool> GetFirmwareMissingAsync(this IDevice o) => o.GetAsync<bool>("FirmwareMissing");
    public static Task<bool> GetNmPluginMissingAsync(this IDevice o) => o.GetAsync<bool>("NmPluginMissing");
    public static Task<uint> GetDeviceTypeAsync(this IDevice o) => o.GetAsync<uint>("DeviceType");
    public static Task<ObjectPath[]> GetAvailableConnectionsAsync(this IDevice o) => o.GetAsync<ObjectPath[]>("AvailableConnections");
    public static Task<string> GetPhysicalPortIdAsync(this IDevice o) => o.GetAsync<string>("PhysicalPortId");
    public static Task<uint> GetMtuAsync(this IDevice o) => o.GetAsync<uint>("Mtu");
    public static Task<uint> GetMeteredAsync(this IDevice o) => o.GetAsync<uint>("Metered");
    public static Task<IDictionary<string, object>[]> GetLldpNeighborsAsync(this IDevice o) => o.GetAsync<IDictionary<string, object>[]>("LldpNeighbors");
    public static Task<bool> GetRealAsync(this IDevice o) => o.GetAsync<bool>("Real");
    public static Task<uint> GetIp4ConnectivityAsync(this IDevice o) => o.GetAsync<uint>("Ip4Connectivity");
    public static Task<uint> GetIp6ConnectivityAsync(this IDevice o) => o.GetAsync<uint>("Ip6Connectivity");
    public static Task<uint> GetInterfaceFlagsAsync(this IDevice o) => o.GetAsync<uint>("InterfaceFlags");
    public static Task<string> GetHwAddressAsync(this IDevice o) => o.GetAsync<string>("HwAddress");
    public static Task<ObjectPath[]> GetPortsAsync(this IDevice o) => o.GetAsync<ObjectPath[]>("Ports");
    public static Task SetManagedAsync(this IDevice o, bool val) => o.SetAsync("Managed", val);
    public static Task SetAutoconnectAsync(this IDevice o, bool val) => o.SetAsync("Autoconnect", val);
}

[DBusInterface("org.freedesktop.NetworkManager.Device.WifiP2P")]
internal interface IWifiP2P : IDBusObject
{
    Task StartFindAsync(IDictionary<string, object> options);
    Task StopFindAsync();
    Task<IDisposable> WatchPeerAddedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<IDisposable> WatchPeerRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<T> GetAsync<T>(string prop);
    Task<WifiP2PProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class WifiP2PProperties
{
    public string HwAddress { get; set; } = default(string);

    public ObjectPath[] Peers { get; set; } = default(ObjectPath[]);
}

internal static class WifiP2PExtensions
{
    public static Task<string> GetHwAddressAsync(this IWifiP2P o) => o.GetAsync<string>("HwAddress");
    public static Task<ObjectPath[]> GetPeersAsync(this IWifiP2P o) => o.GetAsync<ObjectPath[]>("Peers");
}

[DBusInterface("org.freedesktop.NetworkManager.Device.Generic")]
internal interface IGeneric : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<GenericProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class GenericProperties
{
    public string HwAddress { get; set; } = default(string);

    public string TypeDescription { get; set; } = default(string);
}

internal static class GenericExtensions
{
    public static Task<string> GetHwAddressAsync(this IGeneric o) => o.GetAsync<string>("HwAddress");
    public static Task<string> GetTypeDescriptionAsync(this IGeneric o) => o.GetAsync<string>("TypeDescription");
}

[DBusInterface("org.freedesktop.NetworkManager.Device.Wired")]
internal interface IWired : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<WiredProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class WiredProperties
{
    public string HwAddress { get; set; } = default(string);

    public string PermHwAddress { get; set; } = default(string);

    public uint Speed { get; set; } = default(uint);

    public string[] S390Subchannels { get; set; } = default(string[]);

    public bool Carrier { get; set; } = default(bool);
}

internal static class WiredExtensions
{
    public static Task<string> GetHwAddressAsync(this IWired o) => o.GetAsync<string>("HwAddress");
    public static Task<string> GetPermHwAddressAsync(this IWired o) => o.GetAsync<string>("PermHwAddress");
    public static Task<uint> GetSpeedAsync(this IWired o) => o.GetAsync<uint>("Speed");
    public static Task<string[]> GetS390SubchannelsAsync(this IWired o) => o.GetAsync<string[]>("S390Subchannels");
    public static Task<bool> GetCarrierAsync(this IWired o) => o.GetAsync<bool>("Carrier");
}

[DBusInterface("org.freedesktop.NetworkManager.Device.Modem")]
internal interface IModem : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<ModemProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class ModemProperties
{
    public uint ModemCapabilities { get; set; } = default(uint);

    public uint CurrentCapabilities { get; set; } = default(uint);

    public string DeviceId { get; set; } = default(string);

    public string OperatorCode { get; set; } = default(string);

    public string Apn { get; set; } = default(string);
}

internal static class ModemExtensions
{
    public static Task<uint> GetModemCapabilitiesAsync(this IModem o) => o.GetAsync<uint>("ModemCapabilities");
    public static Task<uint> GetCurrentCapabilitiesAsync(this IModem o) => o.GetAsync<uint>("CurrentCapabilities");
    public static Task<string> GetDeviceIdAsync(this IModem o) => o.GetAsync<string>("DeviceId");
    public static Task<string> GetOperatorCodeAsync(this IModem o) => o.GetAsync<string>("OperatorCode");
    public static Task<string> GetApnAsync(this IModem o) => o.GetAsync<string>("Apn");
}

[DBusInterface("org.freedesktop.NetworkManager.Device.Wireless")]
internal interface IWireless : IDBusObject
{
    Task<ObjectPath[]> GetAccessPointsAsync();
    Task<ObjectPath[]> GetAllAccessPointsAsync();
    Task RequestScanAsync(IDictionary<string, object> options);
    Task<IDisposable> WatchAccessPointAddedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<IDisposable> WatchAccessPointRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<T> GetAsync<T>(string prop);
    Task<WirelessProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class WirelessProperties
{
    public string HwAddress { get; set; } = default(string);

    public string PermHwAddress { get; set; } = default(string);

    public uint Mode { get; set; } = default(uint);

    public uint Bitrate { get; set; } = default(uint);

    public ObjectPath[] AccessPoints { get; set; } = default(ObjectPath[]);

    public ObjectPath ActiveAccessPoint { get; set; } = default(ObjectPath);

    public uint WirelessCapabilities { get; set; } = default(uint);

    public long LastScan { get; set; } = default(long);
}

internal static class WirelessExtensions
{
    public static Task<string> GetHwAddressAsync(this IWireless o) => o.GetAsync<string>("HwAddress");
    public static Task<string> GetPermHwAddressAsync(this IWireless o) => o.GetAsync<string>("PermHwAddress");
    public static Task<uint> GetModeAsync(this IWireless o) => o.GetAsync<uint>("Mode");
    public static Task<uint> GetBitrateAsync(this IWireless o) => o.GetAsync<uint>("Bitrate");
    public static Task<ObjectPath[]> GetAccessPointsAsync(this IWireless o) => o.GetAsync<ObjectPath[]>("AccessPoints");
    public static Task<ObjectPath> GetActiveAccessPointAsync(this IWireless o) => o.GetAsync<ObjectPath>("ActiveAccessPoint");
    public static Task<uint> GetWirelessCapabilitiesAsync(this IWireless o) => o.GetAsync<uint>("WirelessCapabilities");
    public static Task<long> GetLastScanAsync(this IWireless o) => o.GetAsync<long>("LastScan");
}

[DBusInterface("org.freedesktop.NetworkManager.DnsManager")]
internal interface IDnsManager : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<DnsManagerProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class DnsManagerProperties
{
    public string Mode { get; set; } = default(string);

    public string RcManager { get; set; } = default(string);

    public IDictionary<string, object>[] Configuration { get; set; } = default(IDictionary<string, object>[]);
}

internal static class DnsManagerExtensions
{
    public static Task<string> GetModeAsync(this IDnsManager o) => o.GetAsync<string>("Mode");
    public static Task<string> GetRcManagerAsync(this IDnsManager o) => o.GetAsync<string>("RcManager");
    public static Task<IDictionary<string, object>[]> GetConfigurationAsync(this IDnsManager o) => o.GetAsync<IDictionary<string, object>[]>("Configuration");
}

[DBusInterface("org.freedesktop.NetworkManager.IP6Config")]
internal interface IIp6Config : IDBusObject
{
    Task<T> GetAsync<T>(string prop);
    Task<Ip6ConfigProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class Ip6ConfigProperties
{
    public (byte[], uint, byte[])[] Addresses { get; set; } = default((byte[], uint, byte[])[]);

    public IDictionary<string, object>[] AddressData { get; set; } = default(IDictionary<string, object>[]);

    public string Gateway { get; set; } = default(string);

    public (byte[], uint, byte[], uint)[] Routes { get; set; } = default((byte[], uint, byte[], uint)[]);

    public IDictionary<string, object>[] RouteData { get; set; } = default(IDictionary<string, object>[]);

    public byte[][] Nameservers { get; set; } = default(byte[][]);

    public string[] Domains { get; set; } = default(string[]);

    public string[] Searches { get; set; } = default(string[]);

    public string[] DnsOptions { get; set; } = default(string[]);

    public int DnsPriority { get; set; } = default(int);
}

internal static class Ip6ConfigExtensions
{
    public static Task<(byte[], uint, byte[])[]> GetAddressesAsync(this IIp6Config o) => o.GetAsync<(byte[], uint, byte[])[]>("Addresses");
    public static Task<IDictionary<string, object>[]> GetAddressDataAsync(this IIp6Config o) => o.GetAsync<IDictionary<string, object>[]>("AddressData");
    public static Task<string> GetGatewayAsync(this IIp6Config o) => o.GetAsync<string>("Gateway");
    public static Task<(byte[], uint, byte[], uint)[]> GetRoutesAsync(this IIp6Config o) => o.GetAsync<(byte[], uint, byte[], uint)[]>("Routes");
    public static Task<IDictionary<string, object>[]> GetRouteDataAsync(this IIp6Config o) => o.GetAsync<IDictionary<string, object>[]>("RouteData");
    public static Task<byte[][]> GetNameserversAsync(this IIp6Config o) => o.GetAsync<byte[][]>("Nameservers");
    public static Task<string[]> GetDomainsAsync(this IIp6Config o) => o.GetAsync<string[]>("Domains");
    public static Task<string[]> GetSearchesAsync(this IIp6Config o) => o.GetAsync<string[]>("Searches");
    public static Task<string[]> GetDnsOptionsAsync(this IIp6Config o) => o.GetAsync<string[]>("DnsOptions");
    public static Task<int> GetDnsPriorityAsync(this IIp6Config o) => o.GetAsync<int>("DnsPriority");
}

[DBusInterface("org.freedesktop.NetworkManager.Settings")]
internal interface ISettings : IDBusObject
{
    Task<ObjectPath[]> ListConnectionsAsync();
    Task<ObjectPath> GetConnectionByUuidAsync(string uuid);
    Task<ObjectPath> AddConnectionAsync(IDictionary<string, IDictionary<string, object>> connection);
    Task<ObjectPath> AddConnectionUnsavedAsync(IDictionary<string, IDictionary<string, object>> connection);
    Task<(ObjectPath path, IDictionary<string, object> result)> AddConnection2Async(IDictionary<string, IDictionary<string, object>> settings, uint flags, IDictionary<string, object> args);
    Task<(bool status, string[] failures)> LoadConnectionsAsync(string[] filenames);
    Task<bool> ReloadConnectionsAsync();
    Task SaveHostnameAsync(string hostname);
    Task<IDisposable> WatchNewConnectionAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<IDisposable> WatchConnectionRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
    Task<T> GetAsync<T>(string prop);
    Task<SettingsProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class SettingsProperties
{
    public ObjectPath[] Connections { get; set; } = default(ObjectPath[]);

    public string Hostname { get; set; } = default(string);

    public bool CanModify { get; set; } = default(bool);
}

internal static class SettingsExtensions
{
    public static Task<ObjectPath[]> GetConnectionsAsync(this ISettings o) => o.GetAsync<ObjectPath[]>("Connections");
    public static Task<string> GetHostnameAsync(this ISettings o) => o.GetAsync<string>("Hostname");
    public static Task<bool> GetCanModifyAsync(this ISettings o) => o.GetAsync<bool>("CanModify");
}

[DBusInterface("org.freedesktop.NetworkManager.Settings.Connection")]
internal interface IConnection : IDBusObject
{
    Task UpdateAsync(IDictionary<string, IDictionary<string, object>> properties);
    Task UpdateUnsavedAsync(IDictionary<string, IDictionary<string, object>> properties);
    Task DeleteAsync();
    Task<IDictionary<string, IDictionary<string, object>>> GetSettingsAsync();
    Task<IDictionary<string, IDictionary<string, object>>> GetSecretsAsync(string settingName);
    Task ClearSecretsAsync();
    Task SaveAsync();
    Task<IDictionary<string, object>> Update2Async(IDictionary<string, IDictionary<string, object>> settings, uint flags, IDictionary<string, object> args);
    Task<IDisposable> WatchUpdatedAsync(Action handler, Action<Exception> onError = null);
    Task<IDisposable> WatchRemovedAsync(Action handler, Action<Exception> onError = null);
    Task<T> GetAsync<T>(string prop);
    Task<ConnectionProperties> GetAllAsync();
    Task SetAsync(string prop, object val);
    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[Dictionary]
internal class ConnectionProperties
{
    public bool Unsaved { get; set; } = default(bool);

    public uint Flags { get; set; } = default(uint);

    public string Filename { get; set; } = default(string);
}

internal static class ConnectionExtensions
{
    public static Task<bool> GetUnsavedAsync(this IConnection o) => o.GetAsync<bool>("Unsaved");
    public static Task<uint> GetFlagsAsync(this IConnection o) => o.GetAsync<uint>("Flags");
    public static Task<string> GetFilenameAsync(this IConnection o) => o.GetAsync<string>("Filename");
}