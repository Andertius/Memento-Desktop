using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class ManageCardsViewModel : PageViewModel, IDialogProvider
{
    [Reactive]
    private ObservableCollection<CardViewModel> _cards = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ICardHttpClient _client;
    private readonly ICardViewModelFactory _cardViewModelFactory;
    private readonly IDialogService _dialogService;
    private readonly IOptions<ApiClientOptions> _options;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public ManageCardsViewModel()
        : base(ApplicationPageNames.ManageCards)
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

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
        _options = options;
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetCards();
        Cards = new ObservableCollection<CardViewModel>(result.Select(x => CardViewModel.FromDataModel(x, $"{_options.Value.Host}/{ApiPaths.CardsImagesPath}/{x.Image}")));
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
