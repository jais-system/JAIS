using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.TextInput;
using Avalonia.Markup.Xaml;
using JAIS.Apps.Keyboard;

namespace JAIS.Core;

public class MainSingleView : UserControl, ITextInputMethodRoot
{
    public MainSingleView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    // private VirtualKeyboardTextInputMethod virtualKeyboardTextInput = new VirtualKeyboardTextInputMethod();

    public IAccessKeyHandler AccessKeyHandler { get; }
    public IKeyboardNavigationHandler KeyboardNavigationHandler { get; }
    public IInputElement? PointerOverElement { get; set; }
    public bool ShowAccessKeys { get; set; }
    public IMouseDevice? MouseDevice { get; }
    // public ITextInputMethodImpl InputMethod { get; }
    public ITextInputMethodImpl InputMethod => null!;
    
    
}