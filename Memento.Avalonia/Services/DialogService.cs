using System.Threading.Tasks;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.ViewModels.DialogViewModels;

namespace Memento.Avalonia.Services;

public interface IDialogService
{
    Task ShowDialogAsync<TDialogProvider, TDialogViewModel>(TDialogProvider host, TDialogViewModel dialog)
        where TDialogProvider : IDialogProvider
        where TDialogViewModel : DialogViewModelBase;
}

public class DialogService : IDialogService
{
    public async Task ShowDialogAsync<TDialogProvider, TDialogViewModel>(TDialogProvider host, TDialogViewModel dialog)
        where TDialogProvider : IDialogProvider
        where TDialogViewModel : DialogViewModelBase
    {
        host.DialogViewModel = dialog;
        dialog.Show();

        await dialog.WaitAsync();
    }
}
