using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.TagViewModels;

public partial class ManageTagsViewModel : PageViewModel, IDialogProvider
{
    [Reactive]
    private ObservableCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ITagHttpClient _client;
    private readonly ITagViewModelFactory _tagViewModelFactory;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public ManageTagsViewModel()
        : base(ApplicationPageNames.ManageTags)
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

        _client = null!;
        _dialogService = null!;
        _tagViewModelFactory = null!;
    }

    public ManageTagsViewModel(
        ITagHttpClient client,
        ITagViewModelFactory tagViewModelFactory,
        IDialogService dialogService)
        : base(ApplicationPageNames.ManageTags)
    {
        _client = client;
        _tagViewModelFactory = tagViewModelFactory;
        _dialogService = dialogService;
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetTags();
        Tags = new ObservableCollection<TagViewModel>(result.Select(TagViewModel.FromDataModel));
    }

    [ReactiveCommand]
    public async Task CreateTagAsync()
    {
        var viewModel = _tagViewModelFactory.CreateCreateTagViewModel();
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Tag.Id != 0)
        {
            Tags.Add(viewModel.Tag);
        }
    }

    [ReactiveCommand]
    public async Task EditTagAsync(TagViewModel cardViewModel)
    {
        var viewModel = _tagViewModelFactory.CreateEditTagViewModel(cardViewModel);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Deleted)
        {
            Tags.Remove(viewModel.Tag);
        }
    }
}
