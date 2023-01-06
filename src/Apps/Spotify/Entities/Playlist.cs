using System.Globalization;
using AppCore.Converters;
using AppCore.Entities;
using Avalonia.Media.Imaging;

namespace Spotify.Entities;

public class Playlist : Notifiable
{
    private string _id = "";
    private string _contextUri = "";
    private string _name = "";
    private Bitmap _image = (Bitmap) BitmapAssetValueConverter.Instance.Convert("avares://Spotify/Assets/MusicCover.png", typeof(Bitmap), null, CultureInfo.InvariantCulture)!;

    public string Id { get => _id; set => Set(ref _id, value); }
    public string ContextUri { get => _contextUri; set => Set(ref _contextUri, value); }
    public string Name { get => _name; set => Set(ref _name, value); }
    public Bitmap Image { get => _image; set => Set(ref _image, value); }
}