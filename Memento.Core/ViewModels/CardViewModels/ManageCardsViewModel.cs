using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CardViewModels;

public partial class ManageCardsViewModel : PageViewModel, IDialogProvider
{
    [Reactive]
    private ObservableCollection<CardViewModel> _cards = [];
    
    [Reactive]
    private IReadOnlyCollection<CategoryViewModel> _categories = [];
    
    [Reactive]
    private IReadOnlyCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ICardHttpClient _client;
    private readonly ICategoryHttpClient _categoryClient;
    private readonly ITagHttpClient _tagClient;
    private readonly ICardViewModelFactory _cardViewModelFactory;
    private readonly IDialogService _dialogService;
    private readonly ApiClientOptions _options;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public ManageCardsViewModel()
        : base(ApplicationPageNames.ManageCards)
    {
        _client = null!;
        _categoryClient = null!;
        _tagClient = null!;
        _cardViewModelFactory = null!;
        _dialogService = null!;
        _options = null!;
    }

    public ManageCardsViewModel(
        ICardHttpClient client,
        ICategoryHttpClient categoryClient,
        ITagHttpClient tagClient,
        ICardViewModelFactory cardViewModelFactory,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options)
        : base(ApplicationPageNames.ManageCards)
    {
        _client = client;
        _categoryClient = categoryClient;
        _tagClient = tagClient;
        _cardViewModelFactory = cardViewModelFactory;
        _dialogService = dialogService;
        _options = options.Value;
    }

    public override async Task OnPageSelected()
    {
        var cards = await _client.GetCards();
        var categories = await _categoryClient.GetCategories();
        var tags = await _tagClient.GetTags();

        Cards = new ObservableCollection<CardViewModel>(cards.Select(x => CardViewModel.FromDataModel(x, ImageHelper.GenerateCardImageUrl(x.Image, _options.Host))));
        Categories = categories.Select(x => CategoryViewModel.FromDataModel(x, ImageHelper.GenerateCategoryImageUrl(x.Image, _options.Host))).ToList();
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
    public async Task EditCard(CardViewModel cardViewModel)
    {
        var viewModel = _cardViewModelFactory.CreateEditCardViewModel(cardViewModel, Categories, Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Deleted)
        {
            Cards.Remove(viewModel.Card);
        }
    }
}
