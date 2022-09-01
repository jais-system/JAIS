using AppCore.Helpers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace JAIS.Dialogs.PowerDialog;

public class PowerDialog : UserControl
{
    public PowerDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CancelButtonClicked(object? sender, RoutedEventArgs e)
    {
        MainView.CloseModal();
    }

    private void RebootClicked(object? sender, RoutedEventArgs e)
    {
        ShellHelper.ExecuteWithoutResult("sudo reboot");
        System.Environment.Exit(0);
    }

    private void ShutdownClicked(object? sender, RoutedEventArgs e)
    {
        ShellHelper.ExecuteWithoutResult("sudo shutdown -h now");
        System.Environment.Exit(0);
    }

    private void QuitGuiClicked(object? sender, RoutedEventArgs e)
    {
        System.Environment.Exit(0);
    }
}