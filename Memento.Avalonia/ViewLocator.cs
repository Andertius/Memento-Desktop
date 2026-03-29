using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Memento.Core.ViewModels;
using ReactiveUI;
using Splat;

namespace Memento.Avalonia;

public sealed class ViewLocator : IDataTemplate
{
    public static bool SupportsRecycling => false;

    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        object? view = AppLocator.Current.GetService(typeof(IViewFor<>).MakeGenericType(data.GetType()));

        return view as Control ?? new TextBlock { Text = "Not Found: " + view?.GetType().FullName };
    }

    public bool Match(object? data)
        => data is ViewModelBase;
}
