using System.Reactive.Disposables.Fluent;
using Memento.Avalonia.Handlers;
using Memento.Avalonia.ViewModels.CardViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.CardViews;

public partial class EditCardView : ReactiveUserControl<EditCardViewModel>
{
    public EditCardView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
            ViewModel!.OpenFile.RegisterHandler(async context =>
            {
                var result = await FileHandler.OpenImage(this);
                context.SetOutput(result);
            }).DisposeWith(disposables));
    }
}
