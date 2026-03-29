using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class ManageCategoriesViewModel : PageViewModel, IDialogProvider
{
    [Reactive]
    private ObservableCollection<CategoryViewModel> _categories = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ICategoryHttpClient _client;
    private readonly ICategoryViewModelFactory _categoryViewModelFactory;
    private readonly IDialogService _dialogService;
    private readonly ApiClientOptions _options;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public ManageCategoriesViewModel()
        : base(ApplicationPageNames.ManageCategories)
    {
        _client = null!;
        _dialogService = null!;
        _categoryViewModelFactory = null!;
        _options = null!;
    }

    public ManageCategoriesViewModel(
        ICategoryHttpClient client,
        ICategoryViewModelFactory categoryViewModelFactory,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options)
        : base(ApplicationPageNames.ManageCategories)
    {
        _client = client;
        _categoryViewModelFactory = categoryViewModelFactory;
        _dialogService = dialogService;
        _options = options.Value;
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetCategories();
        Categories = new ObservableCollection<CategoryViewModel>(result.Select(x => CategoryViewModel.FromDataModel(x, ImageHelper.GenerateCategoryImageUrl(x.Image, _options.Host))));
    }

    [ReactiveCommand]
    public async Task CreateCategoryAsync()
    {
        var viewModel = _categoryViewModelFactory.CreateCreateCategoryViewModel();
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Category.Id != 0)
        {
            Categories.Add(viewModel.Category);
        }
    }

    [ReactiveCommand]
    public async Task EditCategoryAsync(CategoryViewModel cardViewModel)
    {
        var viewModel = _categoryViewModelFactory.CreateEditCategoryViewModel(cardViewModel);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Deleted)
        {
            Categories.Remove(viewModel.Category);
        }
    }
}
