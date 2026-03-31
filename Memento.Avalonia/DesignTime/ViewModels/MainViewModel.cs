using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels;

public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    public string Username { get; set; } = "Spaghet";

    public IPageViewModel CurrentPage { get; set; } = new HomePageViewModel();

    public ReactiveCommand<ApplicationPageNames, Unit> GoToPageCommand { get; } = ReactiveCommand.CreateFromTask<ApplicationPageNames, Unit>(_ => Task.FromResult(Unit.Default));
}
