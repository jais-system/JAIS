using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AppCore.Controls;

public partial class BackButton : UserControl
{
    public event EventHandler<RoutedEventArgs> Click = delegate { };

    public BackButton()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnClick(object? sender, RoutedEventArgs e)
    {
        Click.Invoke(this, e);
    }
}