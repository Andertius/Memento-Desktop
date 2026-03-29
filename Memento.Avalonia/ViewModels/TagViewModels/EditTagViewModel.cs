using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.TagViewModels;

public partial class EditTagViewModel : DialogViewModelBase, IDialogProvider
{
    private readonly ITagHttpClient _client;
    private readonly IDialogService _dialogService;

    [Reactive]
    private TagViewModel _tag;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public EditTagViewModel()
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

        _client = null!;
        _dialogService = null!;
        _tag = new TagViewModel();
    }

    public EditTagViewModel(
        ITagHttpClient client,
        IDialogService dialogService,
        TagViewModel tag)
    {
        _client = client;
        _dialogService = dialogService;
        _tag = tag;
    }

    public bool Deleted { get; private set; }

    [ReactiveCommand]
    public async Task SaveTagAsync()
    {
        await _client.UpdateTag(Tag.ToDataModel());
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Close();
    }

    [ReactiveCommand]
    public async Task DeleteTagAsync()
    {
        var confirmViewModel = new DeleteConfirmationDialogViewModel { DeletedObjectName = Tag.Name };
        await _dialogService.ShowDialogAsync(this, confirmViewModel);

        if (!confirmViewModel.Confirmed)
        {
            return;
        }

        await _client.DeleteTag(Tag.Id);

        Deleted = true;
        Close();
    }
}
