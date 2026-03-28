using CommunityToolkit.Mvvm.Input;

namespace Memento.Avalonia.ViewModels.DialogViewModels;

public partial class DeleteConfirmationDialogViewModel : DialogViewModelBase
{
    public string? DeletedObjectName { get; init; }

    public bool Confirmed { get; private set; }

    public string Message => $"Are you sure you want to delete {DeletedObjectName ?? "<unkown>"}?";

    [RelayCommand]
    public void Confirm()
    {
        Confirmed = true;
        Close();
    }

    [RelayCommand]
    public void Cancel()
    {
        Close();
    }
}
