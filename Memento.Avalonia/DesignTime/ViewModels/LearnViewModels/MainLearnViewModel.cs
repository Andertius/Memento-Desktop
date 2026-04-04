using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.DataModels;
using Memento.Core.Interfaces.ViewModels.LearnViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.LearnViewModels;

public partial class MainLearnViewModel() : PageViewModel(ApplicationPageNames.Learn), IMainLearnViewModel
{
    public IReadOnlyCollection<Tag> Tags { get; set; } = [];

    public IReadOnlyCollection<Category> Categories { get; set; } =
    [
        new() { Name = "Category 1" },
        new() { Name = "Category 2" },
    ];

    public string? Error { get; set; } = "Error text";

    public IReadOnlyList<Card> Cards { get; set; } = [];

    public Category? SelectedCategory { get; set; }

    public IReadOnlyCollection<Tag> SelectedTags { get; set; } = [];

    public string CombinedTags => "";

    public DialogViewModelBase? DialogViewModel { get; set; }

    public ReactiveCommand<Unit, Unit> StartLearnCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));
}
