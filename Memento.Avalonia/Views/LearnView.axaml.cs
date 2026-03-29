using Memento.Core.ViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views;

public partial class LearnView : ReactiveUserControl<LearnViewModel>
{
    public LearnView()
    {
        InitializeComponent();
    }
}
