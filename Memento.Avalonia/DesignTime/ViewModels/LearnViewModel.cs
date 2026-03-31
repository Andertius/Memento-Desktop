using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;

namespace Memento.Avalonia.DesignTime.ViewModels;

public partial class LearnViewModel() : PageViewModel(ApplicationPageNames.Learn), ILearnViewModel
{
    public string Test { get; set; } = "Learn view";
}
