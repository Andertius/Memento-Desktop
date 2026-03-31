using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.CategoryViewModels;

public interface IManageCategoriesViewModel : IViewModelBase, IDialogProvider
{
    ObservableCollection<CategoryViewModel> Categories { get; set; }
    
    IReadOnlyCollection<TagViewModel> Tags { get; set; }
    
    ReactiveCommand<Unit, Unit> CreateCategoryCommand { get; }
    
    ReactiveCommand<CategoryViewModel, Unit> EditCategoryCommand { get; }
}
