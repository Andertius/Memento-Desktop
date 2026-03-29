using System.Threading.Tasks;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.TagViewModels;

public partial class EditTagViewModel(
    ITagHttpClient client,
    IDialogService dialogService,
    TagViewModel tag)
    : DialogViewModelBase, IDialogProvider
{
    [Reactive]
    private TagViewModel _tag = tag;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public EditTagViewModel()
        : this(null!, null!, new TagViewModel())
    {
    }

    public bool Deleted { get; private set; }

    [ReactiveCommand]
    public async Task SaveTagAsync()
    {
        await client.UpdateTag(Tag.ToDataModel());
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
        await dialogService.ShowDialogAsync(this, confirmViewModel);

        if (!confirmViewModel.Confirmed)
        {
            return;
        }

        await client.DeleteTag(Tag.Id);

        Deleted = true;
        Close();
    }
}
