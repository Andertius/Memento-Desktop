using Memento.Avalonia.ViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views;

public partial class HomePageView : ReactiveUserControl<HomePageViewModel>
{
    public HomePageView()
    {
        InitializeComponent();
    }
}
