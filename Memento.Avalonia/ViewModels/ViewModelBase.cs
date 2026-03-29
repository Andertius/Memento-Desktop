using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ReactiveUI;

namespace Memento.Avalonia.ViewModels;

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    protected ViewModelBase()
    {
        this.WhenActivated(disposables =>
        {
            Disposable
                .Create(() => { })
                .DisposeWith(disposables);
        });
    }
}
