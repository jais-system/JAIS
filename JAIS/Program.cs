using System;
using System.Linq;
using System.Threading;
using Avalonia;
using Server;
using AppCore = JAIS.Core.AppCore;

namespace JAIS;

class Program
{
    public static Thread ServerThread { get; private set; }

    [STAThread]
    public static int Main(string[] args)
    {
        AppCore.Initialize();

        var serverCore = new ServerCore();

        ServerThread = new Thread(serverCore.Initialize)
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
                exitCode = builder.StartLinuxDrm(args, "/dev/dri/card0", 1.7d);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error. Trying /dev/dri/card1...");
                exitCode = builder.StartLinuxDrm(args, "/dev/dri/card1", 1.7d);
            }

            return exitCode;
        }

        return builder.StartWithClassicDesktopLifetime(args);
    }

    private static void SilenceConsole()
    {
        new Thread(() =>
            {
                Console.CursorVisible = false;
                while (true)
                    Console.ReadKey(true);
            })
            { IsBackground = true }.Start();
    }


    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}