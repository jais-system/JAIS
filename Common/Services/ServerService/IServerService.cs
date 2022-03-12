namespace Common.Services.ServerService;

public interface IServerService
{
    public int Port { get; set; }

    Task<string> GetIpAddress();
}