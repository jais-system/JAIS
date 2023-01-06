using System.Threading;
using AppCore;
using AppCore.Services.AppManager;
using AppCore.Services.AppManager.Entities;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using JAIS.Apps.Keyboard;
using JAIS.Apps.Keyboard.Layout;
using JAIS.Dialogs.AppSideloadingRequest;
using JAIS.Entities;

namespace JAIS;

public class MainView : UserControl
{
    internal static MainView Instance { get; private set; } = null!;

    private MainViewBindings Bindings { get; } = new ()
    {
        ShowDialog = false
    };

    public MainView()
    {
        var appManager = DependencyInjector.Resolve<IAppManager>();
        appManager.RegisterSideloadingRequestHandler(AppInstallRequest);

        Instance = this;
        DataContext = this;

        VirtualKeyboard.AddLayout<VirtualKeyboardLayoutUS>();
        VirtualKeyboard.SetDefaultLayout(() => typeof(VirtualKeyboardLayoutUS));
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

        if (IsCurrentApp(app))
        {
            return;
        }
        
        Bindings.MainApp = app;

        primaryAppContainerOne.Child = Bindings.MainApp;
    }
}