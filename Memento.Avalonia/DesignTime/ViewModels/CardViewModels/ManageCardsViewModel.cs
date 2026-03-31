using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Avalonia.DesignTime.ViewModels.DialogViewModels;
using Memento.Core.DataModels;
using Memento.Core.Interfaces.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.CardViewModels;

public sealed class ManageCardsViewModel : ViewModelBase, IManageCardsViewModel
{
    public DialogViewModelBase DialogViewModel { get; set; } = new EmptyDialogViewModel();

    public ObservableCollection<CardViewModel> Cards { get; set; } =
    [
        new()
        {
            Word = "Word 1",
            Categories = [new Category { Name = "Category" }],
            Tags = [new Tag { Name = "Tag" }],
        },
        new()
        {
            Word = "Word 2",
            Categories = [new Category { Name = "Category" }],
        },
        new()
        {
            Word = "Word 3",
        },
    ];

    public IReadOnlyCollection<CategoryViewModel> Categories { get; set; } = [];

    public IReadOnlyCollection<TagViewModel> Tags { get; set; } = [];

    public ReactiveCommand<Unit, Unit> CreateCardCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<CardViewModel, Unit> EditCardCommand { get; } = ReactiveCommand.CreateFromTask<CardViewModel, Unit>(_ => Task.FromResult(Unit.Default));
}
