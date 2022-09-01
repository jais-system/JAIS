using Avalonia.Controls;

namespace JAIS.Apps.Keyboard.Layout;

public abstract class KeyboardLayout : UserControl
{
    public string LayoutName { get; } = null!;
}