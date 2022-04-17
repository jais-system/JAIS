using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Common;
using JAIS.Dialogs.PowerDialog;
using JAIS.Entities;
using JAIS.Services.SystemService;
using JAIS.Services.SystemService.Entities;

namespace JAIS.Apps.Dock;

public class Dock : UserControl
{
    private readonly SystemConfig _systemConfig;

    private ObservableCollection<AppInfo> Apps { get; }
    private HashSet<AppInfo> _recentApps = new HashSet<AppInfo>();

    public Dock()
    {
        var systemService = DependencyInjection.Resolve<ISystemService>();
        _systemConfig = systemService.CurrentSystemConfig;
        Apps = new ObservableCollection<AppInfo>(GetApps());
        DataContext = this;
        InitializeComponent();

        Initialized += OnInitialized;
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

    private IEnumerable<AppInfo> GetApps()
    {
        var apps =
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            let attributes = type.GetCustomAttributes(typeof(AppAttribute), true)
            where attributes is { Length: > 0 }
            select new AppInfo { Type = type, Attribute = attributes.Cast<AppAttribute>().First() };

        return apps;
    }

    private void SetLastUsedApp()
    {
        string lastUsedAppName = _systemConfig.LastUsedApp;

        if (string.IsNullOrEmpty(lastUsedAppName))
        {
            return;
        }

        AppInfo? lastUsedApp = Apps.FirstOrDefault(app => app.Attribute.AppName == lastUsedAppName);

        if (lastUsedApp != null)
        {
            SetApp(lastUsedApp);
        }
    }

    private void SetApp(AppInfo appInfo)
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

        MainView.Instance.SetApp(appInfo.Instance);

        _recentApps.Add(appInfo);

        _systemConfig.LastUsedApp = appInfo.Attribute.AppName;
    }

    private void AppClicked(object sender, RoutedEventArgs eventArgs)
    {
        var appInfo = (AppInfo) ((Border) sender).DataContext!;
        SetApp(appInfo);
    }
}