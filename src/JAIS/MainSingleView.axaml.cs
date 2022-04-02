using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JAIS;

public class MainSingleView : UserControl
{
    public MainSingleView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}