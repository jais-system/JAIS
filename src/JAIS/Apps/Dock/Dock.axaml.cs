using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Common;
using Common.Services.AppManager;
using Common.Services.AppManager.Entities;
using JAIS.Dialogs.PowerDialog;
using JAIS.Services.SystemService;
using JAIS.Services.SystemService.Entities;
using NuGet.Packaging;

namespace JAIS.Apps.Dock;

public class Dock : UserControl
{
    private readonly SystemConfig _systemConfig;
    private readonly IAppManager _appManager;

    private ObservableCollection<App> Apps { get; }
    private HashSet<App> _recentApps = new HashSet<App>();

    public Dock()
    {
        _appManager = DependencyInjection.Resolve<IAppManager>();
        var systemService = DependencyInjection.Resolve<ISystemService>();
        _systemConfig = systemService.CurrentSystemConfig;

        Apps = new ObservableCollection<App>(_appManager.LoadApps());
        LoadStandardApps();

        DataContext = this;
        InitializeComponent();

        Initialized += OnInitialized;
        _appManager.OnNewAppInstalled += OnNewAppInstalled;
    }

    private void LoadStandardApps()
    {
        Apps.AddRange(_appManager.GetAppsFromAssembly(typeof(Settings.MainWindow).Assembly, "com.jais.Settings"));
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
                    Apps.AddRange(remainingApps);
                }
            }
            else
            {
                Apps.AddRange(apps);
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
        string lastUsedAppBundleId = _systemConfig.LastUsedApp;

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

        if (appInfo.Instance == null)
        {
            appInfo.Instance = Activator.CreateInstance(appInfo.Type) as UserControl;

            if (appInfo.Instance == null)
            {
                return;
            }
        }

        Dispatcher.UIThread.InvokeAsync(() => MainView.Instance.SetApp(appInfo.Instance));

        _recentApps.Add(appInfo);

        _systemConfig.LastUsedApp = appInfo.BundleId;
    }

    private void AppClicked(object sender, RoutedEventArgs eventArgs)
    {
        var appInfo = (App) ((Border) sender).DataContext!;
        SetApp(appInfo);
    }
}