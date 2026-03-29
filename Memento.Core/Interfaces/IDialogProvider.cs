using Memento.Core.ViewModels.DialogViewModels;

namespace Memento.Core.Interfaces;

public interface IDialogProvider
{
    DialogViewModelBase DialogViewModel { get; set; }
}
