using Memento.Core.ViewModels.LearnViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.LearnViews;

public partial class LearnDialogView : ReactiveUserControl<LearnDialogViewModel>
{
    public LearnDialogView()
    {
        InitializeComponent();
    }
}
