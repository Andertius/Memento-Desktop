namespace Memento.Core.Interfaces.ViewModels;

public interface ISettingsViewModel : IPageViewModel
{
    bool ShouldUseVpn { get; set; }
}
