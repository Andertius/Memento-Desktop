using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using ReactiveUI.Validation.Extensions;

namespace Memento.Avalonia.Views.TagViews;

public partial class EditTagView : ReactiveUserControl<EditTagViewModel>
{
    public EditTagView()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
        {
            return;
        }

        this.WhenActivated(disposables =>
        {
            this.BindValidation(ViewModel, vm => vm.Tag.Name, view => view.NameError.Text)
                .DisposeWith(disposables);
        });
    }
}
