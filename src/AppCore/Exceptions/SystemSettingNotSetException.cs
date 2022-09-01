namespace AppCore.Exceptions;

public class SystemSettingNotSetException : Exception
{
    public string Name { get; set; }

    public SystemSettingNotSetException(string name)
    {
        Name = name;
    }
}