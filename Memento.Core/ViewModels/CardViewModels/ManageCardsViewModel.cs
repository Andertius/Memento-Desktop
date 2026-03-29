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

namespace Memento.Core.ViewModels.CardViewModels;

public partial class ManageCardsViewModel : PageViewModel, IDialogProvider
{
    [Reactive]
    private ObservableCollection<CardViewModel> _cards = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ICardHttpClient _client;
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
        _cardViewModelFactory = null!;
        _dialogService = null!;
        _options = null!;
    }

    public ManageCardsViewModel(
        ICardHttpClient client,
        ICardViewModelFactory cardViewModelFactory,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options)
        : base(ApplicationPageNames.ManageCards)
    {
        _client = client;
        _cardViewModelFactory = cardViewModelFactory;
        _dialogService = dialogService;
        _options = options.Value;
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetCards();
        Cards = new ObservableCollection<CardViewModel>(result.Select(x => CardViewModel.FromDataModel(x, ImageHelper.GenerateCardImageUrl(x.Image, _options.Host))));
    }

    [ReactiveCommand]
    public async Task CreateCardAsync()
    {
        var viewModel = _cardViewModelFactory.CreateCreateCardViewModel();
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Card.Id != 0)
        {
            Cards.Add(viewModel.Card);
        }
    }

    [ReactiveCommand]
    public async Task EditCard(CardViewModel cardViewModel)
    {
        var viewModel = _cardViewModelFactory.CreateEditCardViewModel(cardViewModel);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Deleted)
        {
            Cards.Remove(viewModel.Card);
        }
    }
}
