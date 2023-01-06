using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Svg.Skia;
using JAIS.Core;

namespace JAIS;

internal class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        int exitCode = 0;

        try
        {
            TaskScheduler.UnobservedTaskException += (_, eventArgs) =>
            {
                eventArgs.SetObserved();
                eventArgs.Exception.Handle(exception =>
                {
                    Console.WriteLine(exception);
                    return true;
                });
            };

            AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
            {
                var exception = (Exception) eventArgs.ExceptionObject;
                Console.WriteLine(exception);
            };

            Console.WriteLine("\n\n=====================================");
            Console.WriteLine(
                "     ██  █████  ██ ███████ \n     ██ ██   ██ ██ ██      \n     ██ ███████ ██ ███████ \n██   ██ ██   ██ ██      ██ \n █████  ██   ██ ██ ███████ ");
            Console.WriteLine("=====================================\n\n");

            Startup.Initialize();

            AppBuilder builder = BuildAvaloniaApp();

            if (args.Contains("--drm"))
            {

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

            exitCode = builder.StartWithClassicDesktopLifetime(args);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return exitCode;
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        GC.KeepAlive(typeof(SvgImageExtension).Assembly);
        GC.KeepAlive(typeof(Avalonia.Svg.Skia.Svg).Assembly);
        return AppBuilder.Configure<MainApplication>()
            .UsePlatformDetect()
            .With(new X11PlatformOptions {EnableMultiTouch = true})
            .LogToTrace();
    }
}