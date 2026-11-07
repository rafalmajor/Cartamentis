using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Cartamentis.Wpf.Converters;

/// <summary>
/// Value converter that converts a boolean to a FontWeight.
/// True -> Bold, False -> Normal
/// </summary>
[ValueConversion(typeof(bool), typeof(FontWeight))]
public class BooleanToFontWeightConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to FontWeight.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? FontWeights.Bold : FontWeights.Normal;
        }
        
        return FontWeights.Normal;
    }

    /// <summary>
    /// Converts FontWeight back to boolean (not typically used).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is FontWeight fontWeight)
        {
            return fontWeight == FontWeights.Bold;
        }
        
        return false;
    }
}
