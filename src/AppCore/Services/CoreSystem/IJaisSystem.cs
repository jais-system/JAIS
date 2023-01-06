using AppCore.Services.CoreSystem.Entities;
using AppCore.Theme;

namespace AppCore.Services.CoreSystem;

public interface IJaisSystem
{
    event EventHandler<FluentThemeMode> ModeChanged;
    SystemConfig Configuration { get; }
    void ChangeTheme(bool dark);
    void Reboot();
    void Shutdown();
}