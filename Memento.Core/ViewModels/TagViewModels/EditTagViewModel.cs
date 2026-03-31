using System;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.TagViewModels;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace Memento.Core.ViewModels.TagViewModels;

public partial class EditTagViewModel : DialogViewModelBase, IEditTagViewModel, IValidatableViewModel
{
    private readonly ITagHttpClient _client;
    private readonly IDialogService _dialogService;

    [Reactive]
    private TagViewModel _tag;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    public EditTagViewModel(ITagHttpClient client, IDialogService dialogService, TagViewModel tag)
    {
        _client = client;
        _dialogService = dialogService;
        _tag = tag;

        var canSave = this.WhenAnyValue(x => x.Tag.Name, name => !String.IsNullOrWhiteSpace(name));
        SaveTagCommand = ReactiveCommand.CreateFromTask(SaveTagAsync, canSave);

        this.ValidationRule(
            viewModel => viewModel.Tag.Name,
            canSave,
            "Name cannot be empty");
    }

    public bool Deleted { get; private set; }

    public IValidationContext ValidationContext { get; } = new ValidationContext();

    public ReactiveCommand<Unit, Unit> SaveTagCommand { get; }

    public bool Canceled { get; private set; }

    public async Task SaveTagAsync()
    {
        await _client.UpdateTag(Tag.ToDataModel());
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Canceled = true;
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
