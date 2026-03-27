using System.Threading.Tasks;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.ViewModels;

namespace Memento.Avalonia.Services;

public interface IDialogService
{
    Task ShowDialogAsync<TDialogProvider, TDialogViewModel>(TDialogProvider host, TDialogViewModel dialog)
        where TDialogProvider : IDialogProvider
        where TDialogViewModel : DialogViewModel;
}

public class DialogService : IDialogService
{
    public async Task ShowDialogAsync<TDialogProvider, TDialogViewModel>(TDialogProvider host, TDialogViewModel dialog)
        where TDialogProvider : IDialogProvider
        where TDialogViewModel : DialogViewModel
    {
        host.DialogViewModel = dialog;
        dialog.Show();

        await dialog.WaitAsync();
    }
}
