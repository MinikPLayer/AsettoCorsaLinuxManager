using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ACLinuxManager.Converters;

public class BitmapValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s && (targetType == typeof(IImage) || targetType == typeof(IBitmap)))
        {
            if (string.IsNullOrEmpty(s))
                return null;
            
            var uri = new Uri(s, UriKind.RelativeOrAbsolute);
            var scheme = uri.IsAbsoluteUri ? uri.Scheme : "file";

            switch (scheme)
            {
                case "file":
                    return new Bitmap(s);

                default:
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    return new Bitmap(assets.Open(uri));
            }
        }

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}