using Memento.Avalonia.ViewModels.TagViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.TagViews;

public partial class EditTagView : ReactiveUserControl<EditTagViewModel>
{
    public EditTagView()
    {
        InitializeComponent();
    }
}
