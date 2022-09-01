using Avalonia.Markup.Xaml;

namespace JAIS.Apps.Keyboard.Layout;

// ReSharper disable once InconsistentNaming
public partial class VirtualKeyboardLayoutUS : KeyboardLayout
{
    public new string LayoutName => "en-US";
    
    public VirtualKeyboardLayoutUS()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}