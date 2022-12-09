using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ACLinuxManager.Converters;

public class BooleanInverter : IValueConverter
{
    public object? Invert(object? value) => !(bool)(value ?? true);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Invert(value);
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => Invert(value);
}