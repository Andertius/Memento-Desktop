using System.Threading.Tasks;
using Memento.Core.Data;

namespace Memento.Core.Interfaces.ViewModels;

public interface IPageViewModel : IViewModelBase
{
    ApplicationPageNames PageName { get; set; }

    Task OnPageSelected();
}
