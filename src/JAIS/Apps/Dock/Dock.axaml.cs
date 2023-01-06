using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppCore;
using AppCore.Services.AppManager;
using AppCore.Services.AppManager.Entities;
using AppCore.Services.CoreSystem;
using AppCore.Services.CoreSystem.Entities;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using JAIS.Dialogs.PowerDialog;

namespace JAIS.Apps.Dock;

public class Dock : UserControl
{
    private readonly SystemConfig _systemConfig;
    private readonly IAppManager _appManager;

    private ObservableCollection<App> Apps { get; }
    private HashSet<App> _recentApps = new();

    public Dock()
    {
        _appManager = DependencyInjector.Resolve<IAppManager>();
        var systemService = DependencyInjector.Resolve<IJaisSystem>();
        _systemConfig = systemService.Configuration;

        Apps = new ObservableCollection<App>(_appManager.LoadApps());
        LoadStandardApps();

        DataContext = this;
        InitializeComponent();

        Initialized += OnInitialized;
        _appManager.OnNewAppInstalled += OnNewAppInstalled;
    }

    private void AddApps(IEnumerable<App> apps)
    {
        foreach (App app in apps)
        {
            Apps.Add(app);
        }
    }

    private void LoadStandardApps()
    {
        AddApps(_appManager.GetAppsFromAssembly(typeof(Settings.MainWindow).Assembly, "com.jais.Settings"));
        AddApps(_appManager.GetAppsFromAssembly(typeof(Spotify.SpotifyMainWindow).Assembly, "com.jais.Spotify"));
        AddApps(_appManager.GetAppsFromAssembly(typeof(Maps.MainWindow).Assembly, "com.jais.Maps"));
    }

    private void OnNewAppInstalled(object? sender, AppInfo newAppInfo)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            App? existingApp = Apps.FirstOrDefault(app => app.BundleId == newAppInfo.BundleId);
            var apps = _appManager.LoadApp(newAppInfo).ToList();

            if (existingApp != null)
            {
                int index = Apps.IndexOf(existingApp);

                App? existingNewApp = apps.FirstOrDefault(app => app.Name == existingApp.Name);

                if (existingNewApp != null)
                {
                    _recentApps.Remove(existingApp);

                    Apps[index] = existingNewApp;

                    if (_systemConfig.LastUsedApp == existingNewApp.BundleId)
                    {
                        SetApp(existingNewApp);
                    }

                    var remainingApps = apps.Where(app => app.Name != existingApp.Name);
                    AddApps(remainingApps);
                }
            }
            else
            {
                AddApps(apps);
            }
        });
    }

    private void OnInitialized(object? sender, EventArgs e)
    {
        SetLastUsedApp();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void PowerButtonClicked(object? sender, RoutedEventArgs e)
    {
        MainView.ShowDialog(new PowerDialog());
    }

    private void SetLastUsedApp()
    {
        string? lastUsedAppBundleId = _systemConfig.LastUsedApp;

        if (string.IsNullOrEmpty(lastUsedAppBundleId))
        {
            return;
        }

        App? lastUsedApp = Apps.FirstOrDefault(app => app.BundleId == lastUsedAppBundleId);

        if (lastUsedApp != null)
        {
            SetApp(lastUsedApp);
        }
    }

    private void SetApp(App appInfo)
    {
        if (string.IsNullOrEmpty(appInfo.Id))
        {
            appInfo.Id = Guid.NewGuid().ToString();
        }

        if (appInfo.Instance == null && appInfo.Type != null)
        {
            appInfo.Instance = Activator.CreateInstance(appInfo.Type) as UserControl;

            if (appInfo.Instance == null)
            {
                return;
            }
        }

        if (appInfo.Instance != null)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (App recentApp in _recentApps)
                {
                    recentApp.IsActive = false;
                }
                appInfo.IsActive = true;
                MainView.Instance.SetApp(appInfo.Instance);
            });
            _recentApps.Add(appInfo);
            _systemConfig.LastUsedApp = appInfo.BundleId;
        }
    }

    private void AppClicked(object sender, RoutedEventArgs _)
    {
        var appInfo = (App) ((Button) sender).DataContext!;
        SetApp(appInfo);
    }
}