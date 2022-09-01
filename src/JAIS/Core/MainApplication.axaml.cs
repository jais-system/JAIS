using System.ComponentModel;
using AppCore;
using AppCore.Services.System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace JAIS.Core;

public class MainApplication : Application
{
    private IJaisSystem? _jaisSystem;
    public static Application MainWindow = null!;

    public override void Initialize()
    {
        _jaisSystem = DependencyInjector.Resolve<IJaisSystem>();

        MainWindow = this;
        AvaloniaXamlLoader.Load(this);

        _jaisSystem.Configuration.PropertyChanged += ThemeChanged;
        _jaisSystem.ChangeTheme(_jaisSystem.Configuration.DarkMode);
    }

    private void ThemeChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(_jaisSystem.Configuration.CurrentTheme))
        {
            return;
        }
        
        if (_jaisSystem?.Configuration.CurrentTheme != null)
        {
            if (MainWindow.Styles.Count <= 0)
            {
                MainWindow.Styles.Add(_jaisSystem.Configuration.CurrentTheme);
            }
        
            MainWindow.Styles[0] = _jaisSystem.Configuration.CurrentTheme;
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = new MainSingleView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}