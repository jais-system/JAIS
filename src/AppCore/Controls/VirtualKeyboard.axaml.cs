using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AppCore.Controls;

public partial class VirtualKeyboard : UserControl
{
    public VirtualKeyboard()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}