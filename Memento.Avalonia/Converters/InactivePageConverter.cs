using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;

namespace Memento.Avalonia.Converters;

public sealed class InactivePageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is IPageViewModel viewModel && parameter is ApplicationPageNames pageName
            ? viewModel.PageName != pageName
            : new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
