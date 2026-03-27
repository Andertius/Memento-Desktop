using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.Data;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Microsoft.Extensions.Options;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class ManageCardsViewModel : PageViewModel, IDialogProvider
{
    [ObservableProperty]
    private ObservableCollection<CardViewModel> _cards = [];

    [ObservableProperty]
    private DialogViewModel? _dialogViewModel;

    private readonly ICardHttpClient _client;
    private readonly IDialogService _dialogService;
    private readonly IFilesService _filesService;
    private readonly IOptions<CardClientOptions> _options;

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
        _dialogService = null!;
        _filesService = null!;
        _options = null!;
    }

    public ManageCardsViewModel(ICardHttpClient client,
        IDialogService dialogService,
        IFilesService filesService,
        IOptions<CardClientOptions> options)
        : base(ApplicationPageNames.ManageCards)
    {
        _client = client;
        _dialogService = dialogService;
        _filesService = filesService;
        _options = options;
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetCards();
        Cards = new ObservableCollection<CardViewModel>(result.Select(x => CardViewModel.FromDataModel(x, $"{_options.Value.Host}/images/cards/{x.Id}.png")));
    }

    [RelayCommand]
    public async Task CreateCardAsync()
    {
        var viewModel = new CreateCardViewModel(_client, _filesService, _options);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Card.Id != 0)
        {
            Cards.Add(viewModel.Card);
        }
    }

    [RelayCommand]
    public async Task EditCard(CardViewModel cardViewModel)
    {
        var viewModel = new EditCardViewModel(_client, _filesService, _options, cardViewModel);
        await _dialogService.ShowDialogAsync(this, viewModel);
    }
}
