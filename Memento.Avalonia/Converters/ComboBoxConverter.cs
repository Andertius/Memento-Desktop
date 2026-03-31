using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Memento.Core.DataModels;

namespace Memento.Avalonia.Converters;

public sealed class ComboBoxConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        => values[0] is int id && values[1] is IEnumerable<IEntity> entities && entities.Any(x => x.Id == id);

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
