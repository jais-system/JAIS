using System.Diagnostics;
using System.Text;

namespace Common.Helpers;

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

    public static async Task<(int ExitCode, string? Output, string? ErrorOutput, Exception? Exception)> Execute(string command, Action<string>? outputReceived = null, Action<string>? errorOutputReceived = null)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();
            var outputCloseEvent = new TaskCompletionSource<bool>();
            var errorCloseEvent = new TaskCompletionSource<bool>();

            if (outputReceived == null)
            {
                process.OutputDataReceived += (_, args) =>
                {
                    if (args.Data == null)
                    {
                        outputCloseEvent.SetResult(true);
                    }
                    else
                    {
                        lock (outputBuilder)
                        {
                            outputBuilder.AppendLine(args.Data);
                        }
                    }
                };
            }
            else
            {
                process.OutputDataReceived += (_, args) =>
                {
                    if (args.Data == null)
                    {
                        outputCloseEvent.SetResult(true);
                    }
                    else
                    {
                        lock (outputBuilder)
                        {
                            outputBuilder.AppendLine(args.Data);
                        }

                        outputReceived(args.Data);
                    }
                };
            }

            if (errorOutputReceived == null)
            {
                process.ErrorDataReceived += (_, args) =>
                {
                    if (args.Data == null)
                    {
                        errorCloseEvent.SetResult(true);
                    }
                    else
                    {
                        lock (errorBuilder)
                        {
                            errorBuilder.AppendLine(args.Data);
                        }
                    }
                };
            }
            else
            {
                process.ErrorDataReceived += (_, args) =>
                {
                    if (args.Data == null)
                    {
                        errorCloseEvent.SetResult(true);
                    }
                    else
                    {
                        lock (errorBuilder)
                        {
                            errorBuilder.AppendLine(args.Data);
                        }
                        errorOutputReceived(args.Data);
                    }
                };
            }

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            Task waitForExit = Task.Run(process.WaitForExit);
            Task allProcesses = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

            await allProcesses;

            string output;
            lock (outputBuilder)
            {
                output = outputBuilder.ToString().Trim();
            }

            string errorOutput;
            lock (errorBuilder)
            {
                errorOutput = errorBuilder.ToString().Trim();
            }

            int exitCode = process.ExitCode;

            return (exitCode, output, errorOutput, null);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return (int.MinValue, null, exception.ToString(), exception);
        }
    }
}