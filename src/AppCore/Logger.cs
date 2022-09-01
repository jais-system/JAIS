namespace AppCore;

public static class Logger
{
    private static string FormatMessage(string severity, string message)
    {
        // ReSharper disable once UseFormatSpecifierInInterpolation
        return $"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} {severity} {message}";
    }

    public static void Info(string message)
    {
        Console.WriteLine(FormatMessage("INFO", message));
    }

    public static void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(FormatMessage("GOOD", message));
        Console.ResetColor();
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(FormatMessage("ERR ", message));
        Console.ResetColor();
    }

    public static void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(FormatMessage("WARN", message));
        Console.ResetColor();
    }

    public static void System(string message)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(FormatMessage("SYS ", message));
        Console.ResetColor();
    }

    public static void Exception(Exception exception)
    {
        Logger.Error(exception.ToString());
    }
}