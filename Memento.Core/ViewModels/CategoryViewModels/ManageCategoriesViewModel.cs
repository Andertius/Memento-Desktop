using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
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
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class ManageCategoriesViewModel : PageViewModel, IManageCategoriesViewModel
{
    private readonly ICategoryHttpClient _categoryClient;
    private readonly ITagHttpClient _tagClient;
    private readonly ICategoryViewModelFactory _categoryViewModelFactory;
    private readonly IDialogService _dialogService;
    private readonly ApiClientOptions _options;

    private readonly int _pageSize = 20;
    private int _currentPage;
    private bool _endReached;

    [Reactive]
    private ObservableCollection<CategoryViewModel> _categories = [];
    
    [Reactive]
    private IReadOnlyCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    [Reactive]
    private string? _filter;

    public ManageCategoriesViewModel(
        ICategoryHttpClient categoryClient,
        ITagHttpClient tagClient,
        ICategoryViewModelFactory categoryViewModelFactory,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options)
        : base(ApplicationPageNames.ManageCategories)
    {
        _categoryClient = categoryClient;
        _tagClient = tagClient;
        _categoryViewModelFactory = categoryViewModelFactory;
        _dialogService = dialogService;
        _options = options.Value;

        this.WhenAnyValue(x => x.Filter).Throttle(TimeSpan.FromMilliseconds(400)).SelectMany(x => LoadFilteredCategoriesCommand.Execute(x)).Subscribe();
    }

    public override async Task OnPageSelected()
    {
        await LoadFilteredCategories(null);

        var tags = await _tagClient.GetTags();
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
    public async Task EditCategoryAsync(CategoryViewModel categoryViewModel)
    {
        var viewModel = _categoryViewModelFactory.CreateEditCategoryViewModel(categoryViewModel.Clone(), Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Canceled)
        {
            return;
        }

        if (viewModel.Deleted)
        {
            Categories.Remove(categoryViewModel);

            return;
        }

        int index = Categories.IndexOf(categoryViewModel);

        if (index != -1)
        {
            Categories[index] = viewModel.Category;
        }
    }

    [ReactiveCommand]
    public async Task LoadFilteredCategories(string? filter)
    {
        _currentPage = 0;
        var cards = await _categoryClient.GetAllCategories(filter, _currentPage, _pageSize);

        Categories = new ObservableCollection<CategoryViewModel>(cards.Select(x => CategoryViewModel.FromDataModel(x, ImageHelper.GenerateCategoryImageUrl(x.Image, _options.LocalApiHost))));
    }

    [ReactiveCommand]
    public async Task LoadNextCategories()
    {
        if (_endReached)
        {
            return;
        }

        _currentPage++;

        var categories = await _categoryClient.GetAllCategories(Filter, _currentPage, _pageSize);

        if (categories.Count == 0)
        {
            _endReached = true;
            return;
        }

        foreach (var card in categories)
        {
            Categories.Add(CategoryViewModel.FromDataModel(card, ImageHelper.GenerateCategoryImageUrl(card.Image, _options.LocalApiHost)));
        }
    }
}
