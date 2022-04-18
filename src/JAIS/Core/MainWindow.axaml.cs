using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JAIS.Core;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        // Program.ServerThread.Interrupt();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}