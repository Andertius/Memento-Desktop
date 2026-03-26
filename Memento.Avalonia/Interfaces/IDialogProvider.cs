using System.Threading.Tasks;
using Memento.Avalonia.ViewModels;

namespace Memento.Avalonia.Interfaces;

public interface IDialogProvider
{
    DialogViewModel DialogViewModel { get; set; }
}
