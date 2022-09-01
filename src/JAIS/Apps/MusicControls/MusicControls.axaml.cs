using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JAIS.Apps.MusicControls;

public partial class MusicControls : UserControl
{
    public MusicControls()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}