using System.Collections.Generic;
using System.Reactive;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.CategoryViewModels;

public interface ICreateCategoryViewModel : IViewModelBase
{
    CategoryViewModel Category { get; set; }

    IReadOnlyCollection<TagViewModel> AvailableTags { get; set; }

    ReactiveCommand<Unit, Unit> SaveCategoryCommand { get; }

    ReactiveCommand<Unit, Unit> CancelCommand { get; }

    ReactiveCommand<Unit, Unit> UploadImageCommand { get; }

    ReactiveCommand<Unit, Unit> DeleteImageCommand { get; }
}
