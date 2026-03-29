using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.DialogViewModels;

public partial class DeleteConfirmationDialogViewModel : DialogViewModelBase
{
    [Reactive]
    private string? _deletedObjectName;

    public bool Confirmed { get; private set; }

    public string Message => $"Are you sure you want to delete {DeletedObjectName ?? "<unkown>"}?";

    [ReactiveCommand]
    public void Confirm()
    {
        Confirmed = true;
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Close();
    }
}
