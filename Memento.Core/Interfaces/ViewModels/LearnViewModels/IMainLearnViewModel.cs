using System.Collections.Generic;
using System.Reactive;
using Memento.Core.DataModels;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.LearnViewModels;

public interface IMainLearnViewModel : IPageViewModel
{
    IReadOnlyCollection<Tag> Tags { get; set; }

    IReadOnlyCollection<Category> Categories { get; set; }

    IReadOnlyList<Card> Cards { get; set; }

    Category? SelectedCategory { get; set; }

    IReadOnlyCollection<Tag> SelectedTags { get; }

    string CombinedTags { get; }

    DialogViewModelBase? DialogViewModel { get; set; }

    string? Error { get; set; }

    ReactiveCommand<Unit, Unit> StartLearnCommand { get; }
}
