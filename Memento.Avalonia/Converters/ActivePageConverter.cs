using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Memento.Core.Data;
using Memento.Core.ViewModels;

namespace Memento.Avalonia.Converters;

public sealed class ActivePageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not PageViewModel viewModel || parameter is not ApplicationPageNames pageName)
        {
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        return viewModel.PageName == pageName;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
