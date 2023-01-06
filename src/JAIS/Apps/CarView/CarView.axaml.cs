using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JAIS.Apps.CarView;

public partial class CarView : UserControl
{
    public CarView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}