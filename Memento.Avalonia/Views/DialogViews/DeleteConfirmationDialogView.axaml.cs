using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.DialogViews;

public partial class DeleteConfirmationDialogView : ReactiveUserControl<DeleteConfirmationDialogViewModel>
{
    public DeleteConfirmationDialogView()
    {
        InitializeComponent();
    }
}
