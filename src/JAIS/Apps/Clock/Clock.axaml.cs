using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JAIS.Apps.Clock;

public class Clock : UserControl
{
    public Clock()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}