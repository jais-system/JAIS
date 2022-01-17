using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using JAIS.Helpers;

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
    }

    private void ShutdownClicked(object? sender, RoutedEventArgs e)
    {
        ShellHelper.ExecuteWithoutResult("sudo shutdown -h now");
    }

    private void QuitGuiClicked(object? sender, RoutedEventArgs e)
    {
        System.Environment.Exit(0);
    }
}