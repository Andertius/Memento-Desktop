using System.Reactive.Disposables.Fluent;
using Memento.Avalonia.Handlers;
using Memento.Avalonia.ViewModels.CardViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.CardViews;

public partial class CreateCardView : ReactiveUserControl<CreateCardViewModel>
{
    public CreateCardView()
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
