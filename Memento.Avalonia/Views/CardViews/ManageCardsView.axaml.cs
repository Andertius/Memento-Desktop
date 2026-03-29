using Memento.Core.ViewModels.CardViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.CardViews;

public partial class ManageCardsView : ReactiveUserControl<ManageCardsViewModel>
{
    public ManageCardsView()
    {
        InitializeComponent();
    }
}
