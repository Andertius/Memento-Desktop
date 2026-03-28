using Memento.Avalonia.ViewModels.DialogViewModels;

namespace Memento.Avalonia.Interfaces;

public interface IDialogProvider
{
    DialogViewModelBase DialogViewModel { get; set; }
}
