using System.Collections.Generic;
using System.Reactive;
using Memento.Core.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.CardViewModels;

public interface IEditCardViewModel : IViewModelBase, IDialogProvider
{
    CardViewModel Card { get; set; }

    IReadOnlyCollection<CategoryViewModel> AvailableCategories { get; set; }

    IReadOnlyCollection<TagViewModel> AvailableTags { get; set; }

    ReactiveCommand<Unit, Unit> SaveCardCommand { get; }
    
    ReactiveCommand<Unit, Unit> DeleteCardCommand { get; }

    ReactiveCommand<Unit, Unit> CancelCommand { get; }

    ReactiveCommand<Unit, Unit> UploadImageCommand { get; }

    ReactiveCommand<Unit, Unit> DeleteImageCommand { get; }
}
