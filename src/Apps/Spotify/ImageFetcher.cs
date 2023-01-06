using System.Globalization;
using AppCore.Converters;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Visuals.Media.Imaging;

namespace Spotify;

public class ImageFetcher
{
    private const string CachePath = "Cache"; 
    
    private static string GetFileName(string rawName)
    {
        return Uri.EscapeDataString(rawName);
    }
    
    public static async Task<Bitmap?> LoadImage(string url)
    {
        try
        {
            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
            }
        
            string filePath = Path.Join(CachePath, GetFileName(url));
        
            if (File.Exists(filePath))
            {
                FileStream fileStream = File.OpenRead(filePath);

                if (fileStream.Length > 0)
                {
                    var bitmapKnownImage = new Bitmap(fileStream);
                    return bitmapKnownImage.CreateScaledBitmap(new PixelSize(200, 200), BitmapInterpolationMode.MediumQuality);
                }
            }
        
            using var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using Stream stream = await response.Content.ReadAsStreamAsync();
            //
            // using var compressedImage = new MagickImage(stream);
            // compressedImage.Format = compressedImage.Format;
            // compressedImage.Resize(40, 40); 
            // compressedImage.Quality = 10;
            //
            // var compressedImageStream = new MemoryStream();
            // await compressedImage.WriteAsync(compressedImageStream);
            // compressedImageStream.Position = 0;

            await using FileStream newFileStream = File.OpenWrite(filePath);
            await stream.CopyToAsync(newFileStream);
            
            stream.Position = 0;
            var bitmap = new Bitmap(stream);
            return bitmap.CreateScaledBitmap(new PixelSize(200, 200), BitmapInterpolationMode.MediumQuality);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            return (Bitmap) BitmapAssetValueConverter.Instance.Convert("avares://Spotify/Assets/MusicCover.png", typeof(Bitmap), null, CultureInfo.InvariantCulture)!;
        }
    }
}