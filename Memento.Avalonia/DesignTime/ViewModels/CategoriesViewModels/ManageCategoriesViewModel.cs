using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Avalonia.DesignTime.ViewModels.DialogViewModels;
using Memento.Core.DataModels;
using Memento.Core.Interfaces.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.CategoriesViewModels;

public sealed class ManageCategoriesViewModel : ViewModelBase, IManageCategoriesViewModel
{
    public DialogViewModelBase DialogViewModel { get; set; } = new EmptyDialogViewModel();

    public ObservableCollection<CategoryViewModel> Categories { get; set; } =
    [
        new()
        {
            Name = "Name 1",
            Tags = [new Tag { Name = "Tag" }],
        },
        new()
        {
            Name = "Name 2",
            Tags = [new Tag { Name = "Tag" }],
        },
    ];

    public IReadOnlyCollection<TagViewModel> Tags { get; set; } = [];

    public ReactiveCommand<Unit, Unit> CreateCategoryCommand { get; } = ReactiveCommand.CreateFromTask<Unit, Unit>(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<CategoryViewModel, Unit> EditCategoryCommand { get; } = ReactiveCommand.CreateFromTask<CategoryViewModel, Unit>(_ => Task.FromResult(Unit.Default));
}
