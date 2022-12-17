using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ACLinuxManager.Converters;

public class IntToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var zeroValue = 0;
        if (parameter is int p)
            zeroValue = p;
        else if (parameter is string sp)
            zeroValue = int.Parse(sp);

        if (value is int v)
            return v > zeroValue;

        throw new NotSupportedException();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}