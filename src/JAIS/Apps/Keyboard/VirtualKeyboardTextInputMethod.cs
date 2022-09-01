using AppCore.Services.AppManager.Entities;
using Avalonia;
using Avalonia.Input.TextInput;

namespace JAIS.Apps.Keyboard;

public class VirtualKeyboardTextInputMethod : ITextInputMethodImpl
{
    private bool _isOpen;
    private TextInputOptionsQueryEventArgs? _textInputOptions;
    public async void SetActive(bool active)
    {
        if (active && !_isOpen && _textInputOptions != null)
        {
            _isOpen = true;
            await VirtualKeyboard.ShowDialog(_textInputOptions);
            _isOpen = false;
            _textInputOptions = null;
            // App.MainWindow.Focus(); // remove focus from the last control (TextBox)
        }
    }

    public void SetCursorRect(Rect rect){}

    public void SetOptions(TextInputOptionsQueryEventArgs? options)
    {
        _textInputOptions = options;
    }

    public void Reset(){}
}