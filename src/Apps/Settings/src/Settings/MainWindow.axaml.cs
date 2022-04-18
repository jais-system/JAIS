using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using JaisAppCore;

namespace Settings;

[App("Settings", "avares://Settings/Assets/Icon.png")]
public partial class MainWindow : UserControl
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}