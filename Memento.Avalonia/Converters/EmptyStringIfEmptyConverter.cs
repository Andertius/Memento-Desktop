using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Memento.Avalonia.Converters;

public sealed class EmptyStringIfEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => String.IsNullOrWhiteSpace(value as string) ? "" : parameter;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
