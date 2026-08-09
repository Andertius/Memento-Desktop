using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.CardViewModels;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CardViewModels;

public partial class ManageCardsViewModel : PageViewModel, IManageCardsViewModel
{
    private readonly ICardHttpClient _cardClient;
    private readonly ICategoryHttpClient _categoryClient;
    private readonly ITagHttpClient _tagClient;
    private readonly ICardViewModelFactory _cardViewModelFactory;
    private readonly IDialogService _dialogService;
    private readonly ApiClientOptions _options;

    private readonly int _pageSize = 20;
    private int _currentPage;
    private bool _endReached;

    [Reactive]
    private ObservableCollection<CardViewModel> _cards = [];

    [Reactive]
    private IReadOnlyCollection<CategoryViewModel> _categories = [];

    [Reactive]
    private IReadOnlyCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    [Reactive]
    private string? _filter;

    public ManageCardsViewModel(
        ICardHttpClient cardClient,
        ICategoryHttpClient categoryClient,
        ITagHttpClient tagClient,
        ICardViewModelFactory cardViewModelFactory,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options)
        : base(ApplicationPageNames.ManageCards)
    {
        _cardClient = cardClient;
        _categoryClient = categoryClient;
        _tagClient = tagClient;
        _cardViewModelFactory = cardViewModelFactory;
        _dialogService = dialogService;
        _options = options.Value;

        this.WhenAnyValue(x => x.Filter).Throttle(TimeSpan.FromMilliseconds(400)).SelectMany(x => LoadFilteredCardsCommand.Execute(x)).Subscribe();
    }

    public override async Task OnPageSelected()
    {
        await LoadFilteredCards(null);

        var categories = await _categoryClient.GetAllCategories(null, null, null);
        var tags = await _tagClient.GetTags();

        Categories = categories.Select(x => CategoryViewModel.FromDataModel(x, ImageHelper.GenerateCategoryImageUrl(x.Image, _options.VpnApiHost))).ToList();
        Tags = tags.Select(TagViewModel.FromDataModel).ToList();
    }

    [ReactiveCommand]
    public async Task CreateCardAsync()
    {
        var viewModel = _cardViewModelFactory.CreateCreateCardViewModel(Categories, Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Card.Id != 0)
        {
            Cards.Add(viewModel.Card);
        }
    }

    [ReactiveCommand]
    public async Task EditCardAsync(CardViewModel cardViewModel)
    {
        var viewModel = _cardViewModelFactory.CreateEditCardViewModel(cardViewModel.Clone(), Categories, Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Canceled)
        {
            return;
        }

        if (viewModel.Deleted)
        {
            Cards.Remove(cardViewModel);

            return;
        }

        int index = Cards.IndexOf(cardViewModel);

        if (index != -1)
        {
            Cards[index] = viewModel.Card;
        }
    }

    [ReactiveCommand]
    public async Task LoadFilteredCards(string? filter)
    {
        _endReached = false;
        _currentPage = 0;
        var cards = await _cardClient.GetAllCards(filter, _currentPage, _pageSize);

        Cards = new ObservableCollection<CardViewModel>(cards.Select(x => CardViewModel.FromDataModel(x, ImageHelper.GenerateCardImageUrl(x.Image, _options.VpnApiHost))));
    }

    [ReactiveCommand]
    public async Task LoadNextCards()
    {
        if (_endReached)
        {
            return;
        }
        
        _currentPage++;

        var cards = await _cardClient.GetAllCards(Filter, _currentPage, _pageSize);

        if (cards.Count == 0)
        {
            _endReached = true;
            return;
        }

        foreach (var card in cards)
        {
            Cards.Add(CardViewModel.FromDataModel(card, ImageHelper.GenerateCardImageUrl(card.Image, _options.VpnApiHost)));
        }
    }
}
