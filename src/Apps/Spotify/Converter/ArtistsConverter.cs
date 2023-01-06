using System.Globalization;
using Avalonia.Data.Converters;
using SpotifyAPI.Web;

namespace Spotify.Converter;

public class ArtistsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is List<SimpleArtist> artists)
        {
            return string.Join(", ", artists.Select(artist => artist.Name));
        }

        return "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}