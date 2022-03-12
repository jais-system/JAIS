using Common.Helpers;

namespace Common.Services.ServerService;

public class ServerService : IServerService
{
    public int Port { get; set; }

    public async Task<string> GetIpAddress()
    {
        try
        {
            (int ExitCode, string? Output, string? ErrorOutput, Exception? Exception) result = await ShellHelper.Execute("hostname -I | awk '{print $1}'");

            if (result.ExitCode == 0 && result.Output != null)
            {
                return result.Output;
            }
            else
            {
                return "";
            }
        }
        catch (Exception)
        {
            return "";
        }
    }
}