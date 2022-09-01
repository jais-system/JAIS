using System;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Logging;
using JAIS.Core;

namespace JAIS;

internal class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        Console.WriteLine("\n\n=====================================");
        Console.WriteLine("     ██  █████  ██ ███████ \n     ██ ██   ██ ██ ██      \n     ██ ███████ ██ ███████ \n██   ██ ██   ██ ██      ██ \n █████  ██   ██ ██ ███████ ");
        Console.WriteLine("=====================================\n\n");

        Startup.Initialize();

        AppBuilder builder = BuildAvaloniaApp();

        if (args.Contains("--drm"))
        {
            int exitCode;

            try
            {
                Console.WriteLine("Using /dev/dri/card0...");
                exitCode = builder.StartLinuxDrm(args, "/dev/dri/card0", 1.6d);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("Error. Trying /dev/dri/card1...");
                exitCode = builder.StartLinuxDrm(args, "/dev/dri/card1", 1.6d);
            }

            return exitCode;
        }

        return builder.StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<MainApplication>()
            .UsePlatformDetect()
            .LogToTrace();
}