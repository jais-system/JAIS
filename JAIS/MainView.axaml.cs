using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using JAIS.Entities;

namespace JAIS;

public enum AppContainers
{
    Primary,
    Secondary
}

public class MainView : UserControl
{
    private static AppContainers _currentAppContainerUsed = AppContainers.Secondary;
    public static MainView Instance { get; private set; }

    private MainViewBindings Bindings { get; } = new ()
    {
        ShowDialog = false
    };

    public MainView()
    {
        Instance = this;
        DataContext = this;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
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

    public void SetApp(UserControl app)
    {
        var primaryAppContainerOne = this.Find<Border>("PrimaryAppContainerOne");
        var secondaryAppContainer = this.Find<Border>("SecondaryAppContainer");

        if (Equals(Bindings.MainApp, app))
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
        }
        else
        {
            Bindings.MainApp = app;
            primaryAppContainerOne.Child = Bindings.MainApp;

            RemoveClassIfSet(primaryAppContainerOne, "CloseAppAnimation");
            RemoveClassIfSet(secondaryAppContainer, "OpenAppAnimation");

            primaryAppContainerOne.Classes.Add("OpenAppAnimation");
            secondaryAppContainer.Classes.Add("CloseAppAnimation");

            _currentAppContainerUsed = AppContainers.Primary;
        }
    }
}