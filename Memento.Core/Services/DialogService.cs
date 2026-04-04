using System.Threading.Tasks;
using Memento.Core.Interfaces;
using Memento.Core.ViewModels.DialogViewModels;

namespace Memento.Core.Services;

public interface IDialogService
{
    Task ShowDialogAsync<TDialogProvider, TDialogViewModel>(TDialogProvider host, TDialogViewModel dialog)
        where TDialogProvider : IDialogProvider
        where TDialogViewModel : DialogViewModelBase;
}

public sealed class DialogService : IDialogService
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
