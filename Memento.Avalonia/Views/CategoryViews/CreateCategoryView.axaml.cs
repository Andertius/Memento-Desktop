using System.Reactive.Disposables.Fluent;
using Memento.Avalonia.Handlers;
using Memento.Core.ViewModels.CategoryViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.CategoryViews;

public partial class CreateCategoryView : ReactiveUserControl<CreateCategoryViewModel>
{
    public CreateCategoryView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
            ViewModel!.OpenFile.RegisterHandler(async context =>
            {
                var result = await FileHandler.OpenImage(this);
                context.SetOutput(result);
            }).DisposeWith(disposables));
    }
}
