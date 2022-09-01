using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using AppCore.Services.AppManager.Entities;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Input.TextInput;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using JAIS.Apps.Keyboard.Layout;

namespace JAIS.Apps.Keyboard;


public enum VirtualKeyboardState
{
    Default,
    Shift,
    Capslock,
    AltCtrl
}

public partial class VirtualKeyboard : UserControl
{
    private static List<Type> Layouts { get; } = new();
    private static Func<Type> DefaultLayout { get; set; }

    public static void AddLayout<TLayout>() where TLayout : KeyboardLayout => Layouts.Add(typeof(TLayout));

    public static void SetDefaultLayout(Func<Type> getDefaultLayout) => DefaultLayout = getDefaultLayout;

    public static async Task<string?> ShowDialog(TextInputOptionsQueryEventArgs options, Window? owner = null)
    {
        var keyboard = new VirtualKeyboard();

        if (options.Source is TextBox textBox)
        {
            keyboard.TextBox.Text = textBox.Text;
            keyboard.TextBox.PasswordChar = textBox.PasswordChar;
        }

        // var window = new CoporateWindow();
        // window.CoporateContent = keyboard;
        // window.Title = "MyFancyKeyboard";
        // await window.ShowDialog(owner ?? App.MainWindow);
        // if (window.Tag is string s)
        // {
        //     if (options.Source is TextBox tb)
        //         tb.Text = s;
        //     return s;
        // }
        return null;
    }

    public TextBox TextBox { get; }
    // public TransitioningContentControl TransitioningContentControl { get; }

    public IObservable<VirtualKeyboardState> KeyboardStateStream => _keyboardStateStream;
    private readonly BehaviorSubject<VirtualKeyboardState> _keyboardStateStream;

    private Window _parentWindow;

    public VirtualKeyboard()
    {
        InitializeComponent();
        TextBox = this.Get<TextBox>(nameof(TextBox));
        // TransitioningContentControl = this.Get<TransitioningContentControl>(nameof(TransitioningContentControl));
        //
        // Initialized += async (sender, args) =>
        // {
        //     TransitioningContentControl.Content = Activator.CreateInstance(DefaultLayout.Invoke());
        //     _parentWindow = this.GetVisualAncestors().OfType<Window>().First();
        //     await Task.Delay(TimeSpan.FromMilliseconds(100));
        //     Dispatcher.UIThread.Post(() =>
        //     {
        //         TextBox.Focus();
        //         if(!string.IsNullOrEmpty(TextBox.Text))
        //             TextBox.CaretIndex = TextBox.Text.Length;
        //     });
        // };
        KeyDown += (sender, args) =>
        {
            TextBox.Focus();
            if (args.Key == Key.Escape)
            {
                TextBox.Text = "";
            }
            else if(args.Key == Key.Enter)
            {
                _parentWindow.Tag = TextBox.Text;
                _parentWindow.Close();
            }
        };
        _keyboardStateStream = new BehaviorSubject<VirtualKeyboardState>(VirtualKeyboardState.Default);
    }

    public void ProcessText(string text)
    {
        TextBox.Focus();
        InputManager.Instance.ProcessInput(new RawTextInputEventArgs(KeyboardDevice.Instance, (ulong)DateTime.Now.Ticks, (Window)TextBox.GetVisualRoot(), text ));
        if (_keyboardStateStream.Value == VirtualKeyboardState.Shift)
        {
            _keyboardStateStream.OnNext(VirtualKeyboardState.Default);
        }
    }

    public void ProcessKey(Key key)
    {
        if (key == Key.LeftShift || key == Key.RightShift)
        {
            if (_keyboardStateStream.Value == VirtualKeyboardState.Shift)
            {
                _keyboardStateStream.OnNext(VirtualKeyboardState.Default);
            }
            else
            {
                _keyboardStateStream.OnNext(VirtualKeyboardState.Shift);
            }
        }
        else if (key == Key.RightAlt)
        {
            if (_keyboardStateStream.Value == VirtualKeyboardState.AltCtrl)
            {
                _keyboardStateStream.OnNext(VirtualKeyboardState.Default);
            }
            else
            {
                _keyboardStateStream.OnNext(VirtualKeyboardState.AltCtrl);
            }
        }
        else if (key == Key.CapsLock)
        {
            if (_keyboardStateStream.Value == VirtualKeyboardState.Capslock)
            {
                _keyboardStateStream.OnNext(VirtualKeyboardState.Default);
            }
            else
            {
                _keyboardStateStream.OnNext(VirtualKeyboardState.Capslock);
            }
        }
        else
        {
            if (key == Key.Clear)
            {
                TextBox.Text = "";
                TextBox.Focus();
            }
            else if (key == Key.Enter)
            {
                _parentWindow.Tag = TextBox.Text;
                _parentWindow.Close();
            }
            else if (key == Key.Help)
            {
                // _keyboardStateStream.OnNext(VirtualKeyboardState.Default);
                // if (TransitioningContentControl.Content is KeyboardLayout layout)
                // {
                //     int index = Layouts.IndexOf(layout.GetType());
                //     if (Layouts.Count - 1 > index)
                //     {
                //         TransitioningContentControl.Content = Activator.CreateInstance(Layouts[index + 1]);
                //     }
                //     else
                //     {
                //         TransitioningContentControl.Content = Activator.CreateInstance(Layouts[0]);
                //     }
                // }
            }
            else
            {
                TextBox.Focus();
                InputManager.Instance?.ProcessInput(new RawKeyEventArgs(KeyboardDevice.Instance!, (ulong)DateTime.Now.Ticks, (Window)TextBox.GetVisualRoot(), RawKeyEventType.KeyDown, key, RawInputModifiers.None));
                InputManager.Instance?.ProcessInput(new RawKeyEventArgs(KeyboardDevice.Instance!, (ulong)DateTime.Now.Ticks, (Window)TextBox.GetVisualRoot(), RawKeyEventType.KeyUp, key, RawInputModifiers.None));
            }
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}