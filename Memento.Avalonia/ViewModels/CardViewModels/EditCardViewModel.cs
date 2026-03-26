using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class EditCardViewModel(CardViewModel card) : DialogViewModel
{
    [ObservableProperty]
    private CardViewModel _card = card;

    public bool Canceled { get; private set; }

    [RelayCommand]
    public void SaveCard()
    {
        Canceled = false;
        Close();
    }

    [RelayCommand]
    public void Cancel()
    {
        Canceled = true;
        Close();
    }
}
