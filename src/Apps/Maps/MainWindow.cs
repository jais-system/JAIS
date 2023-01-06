using AppCore;
using AppCore.Services.ConnectionManager;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Maps.ViewModels;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;

namespace Maps;


[App("Maps", "avares://Maps/Assets/Icon.png")]
public partial class MainWindow : UserControl
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
        var mapControl = new Mapsui.UI.Avalonia.MapControl();

        if (mapControl.Map == null)
        {
            return;
        }
        
        mapControl.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        Content = mapControl;
        var centerOfLondonOntario = new MPoint(-81.2497, 42.9837);
        
        var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(centerOfLondonOntario.X, centerOfLondonOntario.Y).ToMPoint();
        mapControl.Map.Home = n => n.NavigateTo(sphericalMercatorCoordinate, mapControl.Map.Resolutions[9]);

    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}