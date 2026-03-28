using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Avalonia.ViewModels.DialogViewModels;

public abstract partial class DialogViewModelBase : ViewModelBase
{
    [ObservableProperty]
    private bool _isDialogOpen;

    protected TaskCompletionSource _closeTask = new();

    public async Task WaitAsync()
    {
        await _closeTask.Task;
    }

    public void Show()
    {
        if (_closeTask.Task.IsCanceled)
        {
            _closeTask = new TaskCompletionSource();
        }

        IsDialogOpen = true;
    }

    protected void Close()
    {
        IsDialogOpen = false;
        _closeTask.TrySetResult();
    }
}
