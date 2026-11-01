using System;
using System.Globalization;
using System.Windows.Data;

namespace Cartamentis.Wpf.Converters;

/// <summary>
/// Value converter that adds a constant offset to a double value.
/// Used for positioning connection lines relative to topic boxes.
/// </summary>
[ValueConversion(typeof(double), typeof(double))]
public class AddOffsetConverter : IValueConverter
{
    /// <summary>
    /// Converts a value by adding the specified offset.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The target type (unused).</param>
    /// <param name="parameter">The offset to add (as string).</param>
    /// <param name="culture">The culture (unused).</param>
    /// <returns>The original value plus the offset.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue && parameter is string paramString && double.TryParse(paramString, out double offset))
        {
            return doubleValue + offset;
        }
        
        return value;
    }

    /// <summary>
    /// Converts back by subtracting the offset (not typically used).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue && parameter is string paramString && double.TryParse(paramString, out double offset))
        {
            return doubleValue - offset;
        }
        
        return value;
    }
}
