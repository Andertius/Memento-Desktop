using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;

namespace Memento.Avalonia.ViewModels.TagViewModels;

public partial class EditTagViewModel : DialogViewModelBase, IDialogProvider
{
    private readonly ITagHttpClient _client;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private TagViewModel _tag;

    [ObservableProperty]
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

    [RelayCommand]
    public async Task SaveTag()
    {
        await _client.UpdateTag(Tag.ToDataModel());
        Close();
    }

    [RelayCommand]
    public void Cancel()
    {
        Close();
    }

    [RelayCommand]
    public async Task DeleteTag()
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
