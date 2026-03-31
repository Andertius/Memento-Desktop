using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.Interfaces.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.CardViewModels;

public sealed class CreateCardViewModel : ViewModelBase, ICreateCardViewModel
{
    public CardViewModel Card { get; set; } = new()
    {
        Word = "Word",
        Translation = "Translation",
        Definition = "Definition",
        Hint = "Hint",
    };

    public IReadOnlyCollection<CategoryViewModel> AvailableCategories { get; set; } =
    [
        new() { Name = "Category 1" },
        new() { Name = "Category 2" },
    ];

    public IReadOnlyCollection<TagViewModel> AvailableTags { get; set; } =
    [
        new() { Name = "Tag 1" },
        new() { Name = "Tag 2" },
    ];

    public ReactiveCommand<Unit, Unit> SaveCardCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> CancelCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> UploadImageCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> DeleteImageCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));
}
