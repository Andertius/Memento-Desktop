using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.CategoryViewModels;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class ManageCategoriesViewModel(
    ICategoryHttpClient _categoryClient,
    ITagHttpClient _tagClient,
    ICategoryViewModelFactory _categoryViewModelFactory,
    IDialogService _dialogService,
    IOptions<ApiClientOptions> options)
    : PageViewModel(ApplicationPageNames.ManageCategories), IManageCategoriesViewModel
{
    [Reactive]
    private ObservableCollection<CategoryViewModel> _categories = [];
    
    [Reactive]
    private IReadOnlyCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ApiClientOptions _options = options.Value;

    public override async Task OnPageSelected()
    {
        var categories = await _categoryClient.GetCategories();
        var tags = await _tagClient.GetTags();

        Categories = new ObservableCollection<CategoryViewModel>(categories.Select(x => CategoryViewModel.FromDataModel(x, ImageHelper.GenerateCategoryImageUrl(x.Image, _options.Host))));
        Tags = tags.Select(TagViewModel.FromDataModel).ToList();
    }

    [ReactiveCommand]
    public async Task CreateCategoryAsync()
    {
        var viewModel = _categoryViewModelFactory.CreateCreateCategoryViewModel(Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Category.Id != 0)
        {
            Categories.Add(viewModel.Category);
        }
    }

    [ReactiveCommand]
    public async Task EditCategoryAsync(CategoryViewModel cardViewModel)
    {
        var viewModel = _categoryViewModelFactory.CreateEditCategoryViewModel(cardViewModel, Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Deleted)
        {
            Categories.Remove(viewModel.Category);
        }
    }
}
