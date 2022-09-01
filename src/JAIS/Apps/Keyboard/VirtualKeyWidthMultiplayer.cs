using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace JAIS.Apps.Keyboard;

public class VirtualKeyWidthMultiplayer : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double v = double.Parse(value.ToString());
        double p = double.Parse(parameter.ToString());
        return v * (p / 10.0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}