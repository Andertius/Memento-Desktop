using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Avalonia.ViewModels.CategoryViewModels;

public partial class ManageCategoriesViewModel : PageViewModel, IDialogProvider
{
    [ObservableProperty]
    private ObservableCollection<CategoryViewModel> _categories = [];

    [ObservableProperty]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ICategoryHttpClient _client;
    private readonly ICategoryViewModelFactory _categoryViewModelFactory;
    private readonly IDialogService _dialogService;
    private readonly IOptions<ApiClientOptions> _options;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public ManageCategoriesViewModel()
        : base(ApplicationPageNames.ManageCategories)
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

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
        _options = options;
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetCategories();
        Categories = new ObservableCollection<CategoryViewModel>(result.Select(x => CategoryViewModel.FromDataModel(x, $"{_options.Value.Host}/{ApiPaths.CategoriesImagesPath}/{x.Image}")));
    }

    [RelayCommand]
    public async Task CreateCategoryAsync()
    {
        var viewModel = _categoryViewModelFactory.CreateCreateCategoryViewModel();
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Category.Id != 0)
        {
            Categories.Add(viewModel.Category);
        }
    }

    [RelayCommand]
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
