using System;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace JAIS.Apps.Keyboard;

public class VirtualKey : TemplatedControl
{
    public static readonly StyledProperty<ICommand> ButtonCommandProperty = AvaloniaProperty.Register<VirtualKey, ICommand>(nameof(ButtonCommand));

        public ICommand ButtonCommand
        {
            get => GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        public static readonly StyledProperty<string> NormalKeyProperty =  AvaloniaProperty.Register<VirtualKey, string>(nameof(NormalKey));

        public string NormalKey
        {
            get => GetValue(NormalKeyProperty);
            set => SetValue(NormalKeyProperty, value);
        }

        public static readonly StyledProperty<string> ShiftKeyProperty =  AvaloniaProperty.Register<VirtualKey, string>(nameof(ShiftKey));

        public string ShiftKey
        {
            get => GetValue(ShiftKeyProperty);
            set => SetValue(ShiftKeyProperty, value);
        }
        public static readonly StyledProperty<string> AltCtrlKeyProperty =  AvaloniaProperty.Register<VirtualKey, string>(nameof(AltCtrlKey));

        public string AltCtrlKey
        {
            get => GetValue(AltCtrlKeyProperty);
            set => SetValue(AltCtrlKeyProperty, value);
        }

        public static readonly StyledProperty<object> CaptionProperty = AvaloniaProperty.Register<VirtualKey, object>(nameof(Caption));

        public object Caption
        {
            get => GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public static readonly StyledProperty<Key> SpecialKeyProperty =  AvaloniaProperty.Register<VirtualKey, Key>(nameof(SpecialKey));

        public Key SpecialKey
        {
            get => GetValue(SpecialKeyProperty);
            set => SetValue(SpecialKeyProperty, value);
        }
        
        private ToggleButton? _toggleButton;

        public VirtualKey()
        {
            DataContext = this;

            Initialized += (sender, args) =>
            {
                VirtualKeyboard keyboard = null;
                if (!Design.IsDesignMode)
                {
                    keyboard = this.GetVisualAncestors().OfType<VirtualKeyboard>().First();

                    keyboard.KeyboardStateStream.Subscribe(state =>
                    {
                        if (!string.IsNullOrEmpty(NormalKey))
                        {
                            switch (state)
                            {
                                case VirtualKeyboardState.Default:
                                    Caption = NormalKey;
                                    break;
                                case VirtualKeyboardState.Shift:
                                case VirtualKeyboardState.Capslock:
                                    Caption = ShiftKey;
                                    break;
                                case VirtualKeyboardState.AltCtrl:
                                    Caption = AltCtrlKey;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
                            }
                        }
                    });
                    //
                    // ButtonCommand = new RelayCommand(() =>
                    // {
                    //     if (string.IsNullOrEmpty(NormalKey))
                    //     {
                    //         keyboard.ProcessKey(SpecialKey);
                    //     }
                    //     else
                    //     {
                    //         if(Caption is string s && !string.IsNullOrEmpty(s))
                    //             keyboard.ProcessText(s);
                    //     }
                    // });
                }

                if (SpecialKey == Key.LeftShift || SpecialKey == Key.RightShift || SpecialKey == Key.CapsLock || SpecialKey == Key.RightAlt)
                {
                    _toggleButton = new ToggleButton
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush(Color.Parse("Black")),
                        [!ToggleButton.WidthProperty] = new Binding("Width"),
                        [!ToggleButton.HeightProperty] = new Binding("Height"),
                        [!ToggleButton.ContentProperty] = new Binding("Caption"),
                        [!ToggleButton.CommandProperty] = new Binding("ButtonCommand"),
                    };
                    Template = new FuncControlTemplate((control, scope) => _toggleButton);

                    if (keyboard != null)
                    {
                        keyboard.KeyboardStateStream.Subscribe(state =>
                        {
                            switch (state)
                            {
                                case VirtualKeyboardState.Default:
                                    _toggleButton.IsChecked = false;
                                    break;
                                case VirtualKeyboardState.Shift:
                                    if (SpecialKey == Key.LeftShift || SpecialKey == Key.RightShift)
                                        _toggleButton.IsChecked = true;
                                    else
                                    {
                                        _toggleButton.IsChecked = false;
                                    }
                                    break;
                                case VirtualKeyboardState.Capslock:
                                    _toggleButton.IsChecked = SpecialKey == Key.CapsLock;
                                    break;
                                case VirtualKeyboardState.AltCtrl:
                                    _toggleButton.IsChecked = SpecialKey == Key.RightAlt;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
                            }
                        });
                    }
                }
                else
                {
                    Template = new FuncControlTemplate((control, scope) =>
                    {
                        return new Button
                        {
                            BorderThickness = new Thickness(1),
                            BorderBrush = new SolidColorBrush(Color.Parse("Black")),
                            [!Button.WidthProperty] = new Binding("Width"),
                            [!Button.HeightProperty] = new Binding("Height"),
                            [!Button.ContentProperty] = new Binding("Caption"),
                            [!Button.CommandProperty] = new Binding("ButtonCommand"),
                        };
                    });
                }

                if (string.IsNullOrEmpty(NormalKey))
                {
                    // special cases
                    // switch (SpecialKey)
                    // {
                    //     case Key.Tab:
                    //     {
                    //         var stackPanel = new StackPanel();
                    //         stackPanel.Orientation = Orientation.Vertical;
                    //         var first = new MaterialIcon();
                    //         first.Kind = SpecialIcon;
                    //         var second = new MaterialIcon();
                    //         second.Kind = SpecialIcon;
                    //         second.RenderTransform = new RotateTransform(180.0);
                    //         stackPanel.Children.Add(first);
                    //         stackPanel.Children.Add(second);
                    //         Caption = stackPanel;
                    //         IsEnabled = false;
                    //     }
                    //         break;
                    //     case Key.Space:
                    //     {
                    //         Caption = null;
                    //     }
                    //         break;
                    //     default:
                    //         Caption = new MaterialIcon
                    //         {
                    //             Kind = SpecialIcon
                    //         };
                    //         break;
                    // }
                }
                else
                {
                    Caption = NormalKey;
                }
            };
        }
}