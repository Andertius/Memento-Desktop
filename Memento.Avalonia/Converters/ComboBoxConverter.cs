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
    {
        if (values[0] is not int id || values[1] is not IEnumerable<IEntity> categories)
        {
            return false;
        }

        return categories.Any(x => x.Id == id);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
