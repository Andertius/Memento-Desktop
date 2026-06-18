using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Avalonia.Input;
using Memento.Core.ViewModels.CategoryViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.CategoryViews;

public partial class ManageCategoriesView : ReactiveUserControl<ManageCategoriesViewModel>
{
    public ManageCategoriesView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            Observable.FromEventPattern<PointerWheelEventArgs>(h => Scroll.PointerWheelChanged += h, h => Scroll.PointerWheelChanged -= h)
                .Select(e => e.EventArgs.Delta.Y)
                .Where(delta => Math.Abs(delta + 1) < Double.Epsilon)
                .Select(_ => ViewModel!.LoadNextCategoriesCommand)
                .SelectMany(command => command.Execute())
                .Subscribe()
                .DisposeWith(disposables);
        });
    }
}
