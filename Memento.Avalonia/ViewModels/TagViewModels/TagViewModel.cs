using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.DataModels;

namespace Memento.Avalonia.ViewModels.TagViewModels;

public partial class TagViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string? _name;

    public static TagViewModel FromDataModel(Tag tag) => new()
    {
        Id = tag.Id,
        Name = tag.Name,
    };

    public Tag ToDataModel() => new()
    {
        Id = Id,
        Name = Name,
    };
}
