using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.Interfaces.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.CategoriesViewModels;

public sealed class CreateCategoryViewModel : ViewModelBase, ICreateCategoryViewModel
{
    public CategoryViewModel Category { get; set; } = new()
    {
        Name = "Name",
        Description = "Description",
    };

    public IReadOnlyCollection<TagViewModel> AvailableTags { get; set; } =
    [
        new() { Name = "Tag 1" },
        new() { Name = "Tag 2" },
    ];

    public ReactiveCommand<Unit, Unit> SaveCategoryCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> CancelCommand { get; } = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default);

    public ReactiveCommand<Unit, Unit> UploadImageCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> DeleteImageCommand { get; } = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default);
}
