using System.Diagnostics;
using System.Threading.Tasks;

namespace JAIS.Helpers;

public class ShellHelper
{
    public static void ExecuteWithoutResult(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true
            }
        };

        process.Start();
    }
}