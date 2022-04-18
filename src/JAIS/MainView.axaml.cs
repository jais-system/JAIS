using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Common;
using Common.Services.AppManager;
using Common.Services.AppManager.Entities;
using JAIS.Dialogs.AppSideloadingRequest;
using JAIS.Dialogs.PowerDialog;
using JAIS.Entities;
using JAIS.Extensions;
using AppInfo = Common.Services.AppManager.Entities.AppInfo;

namespace JAIS;

public enum AppContainers
{
    Primary,
    Secondary
}

public class MainView : UserControl
{
    private readonly IAppManager _appManager;

    private static AppContainers _currentAppContainerUsed = AppContainers.Secondary;
    internal static MainView Instance { get; private set; } = null!;

    private MainViewBindings Bindings { get; } = new ()
    {
        ShowDialog = false
    };

    public MainView()
    {
        _appManager = DependencyInjection.Resolve<IAppManager>();

        _appManager.RegisterSideloadingRequestHandler(AppInstallRequest);

        Instance = this;
        DataContext = this;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private bool AppInstallRequest(SideloadingRequest request)
    {
        AppSideloadingRequestDialog dialog = null!;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            dialog = new AppSideloadingRequestDialog(request);
            ShowDialog(dialog);
        }).Wait();

        while (dialog.Result == null)
        {
            Thread.Sleep(500);
        }

        return (bool) dialog.Result!;
    }

    public static void ShowDialog(UserControl dialog)
    {
        var dialogControl = Instance.Find<StackPanel>("DialogContent");

        if (dialogControl.Children.Count > 0)
        {
            dialogControl.Children.RemoveAt(0);
        }

        dialogControl.Children.Add(dialog);

        Instance.Bindings.ShowDialog = true;
    }

    public static void CloseModal()
    {
        Instance.Bindings.ShowDialog = false;
    }

    private static void RemoveClassIfSet(IStyledElement control, string className)
    {
        if (control.Classes.Contains(className))
        {
            control.Classes.Remove(className);
        }
    }

    public static bool IsCurrentApp(UserControl newApp)
    {
        return Equals(Instance.Bindings.MainApp, newApp);
    }

    public void SetApp(UserControl app)
    {
        var primaryAppContainerOne = this.Find<Border>("PrimaryAppContainerOne");
        var secondaryAppContainer = this.Find<Border>("SecondaryAppContainer");

        if (IsCurrentApp(app))
        {
            return;
        }

        if (_currentAppContainerUsed == AppContainers.Primary)
        {
            Bindings.PreviousMainApp = Bindings.MainApp;
            Bindings.MainApp = app;

            primaryAppContainerOne.Child = Bindings.PreviousMainApp;
            secondaryAppContainer.Child = Bindings.MainApp;

            RemoveClassIfSet(primaryAppContainerOne, "OpenAppAnimation");
            RemoveClassIfSet(secondaryAppContainer, "CloseAppAnimation");

            primaryAppContainerOne.Classes.Add("CloseAppAnimation");
            secondaryAppContainer.Classes.Add("OpenAppAnimation");

            _currentAppContainerUsed = AppContainers.Secondary;

            // var timer = new Timer(1000);
            // timer.Elapsed += (sender, args) =>
            // {
            //     primaryAppContainerOne.Child = null;
            // };
        }
        else
        {
            Bindings.PreviousMainApp = Bindings.MainApp;
            Bindings.MainApp = app;

            secondaryAppContainer.Child = Bindings.PreviousMainApp;
            primaryAppContainerOne.Child = Bindings.MainApp;

            RemoveClassIfSet(primaryAppContainerOne, "CloseAppAnimation");
            RemoveClassIfSet(secondaryAppContainer, "OpenAppAnimation");

            primaryAppContainerOne.Classes.Add("OpenAppAnimation");
            secondaryAppContainer.Classes.Add("CloseAppAnimation");

            _currentAppContainerUsed = AppContainers.Primary;

            // var timer = new Timer(1000);
            // timer.Elapsed += (sender, args) =>
            // {
            //     secondaryAppContainer.Child = null;
            // };
        }
    }
}