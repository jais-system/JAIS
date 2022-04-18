using System;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Avalonia;
using Common;
using JAIS.Core;
using RestServer;
using Startup = JAIS.Core.Startup;

namespace JAIS;

internal class Program
{
    public static Thread ServerThread { get; private set; } = null!;

    [STAThread]
    public static int Main(string[] args)
    {
        Console.WriteLine("\n\n=====================================");
        Console.WriteLine("     ██  █████  ██ ███████ \n     ██ ██   ██ ██ ██      \n     ██ ███████ ██ ███████ \n██   ██ ██   ██ ██      ██ \n █████  ██   ██ ██ ███████ ");
        Console.WriteLine("=====================================\n\n");

        Startup.Initialize();

        DependencyInjection.Build();

        ServerThread = new Thread(ServerCore.Initialize)
        {
            IsBackground = true
        };

        ServerThread.Start();

        AppBuilder builder = BuildAvaloniaApp();

        if (args.Contains("--drm"))
        {
            int exitCode;

            try
            {
                Console.WriteLine("Using /dev/dri/card0...");
                exitCode = builder.StartLinuxDrm(args, "/dev/dri/card0", 1.6d);
            }
            catch (Exception)
            {
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