using AppCore.Services.System.Entities;

namespace AppCore.Services.System;

public interface IJaisSystem
{
    SystemConfig Configuration { get; }
    void ChangeTheme(bool dark);
    void Reboot();
    void Shutdown();
}