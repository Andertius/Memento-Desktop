using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Memento.Core.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.CardViewModels;

public interface IManageCardsViewModel : IViewModelBase, IDialogProvider
{
    ObservableCollection<CardViewModel> Cards { get; set; }

    IReadOnlyCollection<CategoryViewModel> Categories { get; set; }

    IReadOnlyCollection<TagViewModel> Tags { get; set; }
    
    ReactiveCommand<Unit, Unit> CreateCardCommand { get; }
    
    ReactiveCommand<CardViewModel, Unit> EditCardCommand { get; }
}
