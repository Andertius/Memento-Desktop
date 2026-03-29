using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.TagViews;

public partial class CreateTagView : ReactiveUserControl<CreateTagViewModel>
{
    public CreateTagView()
    {
        InitializeComponent();
    }
}
