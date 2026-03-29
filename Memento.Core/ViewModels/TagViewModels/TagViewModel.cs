using Memento.Core.DataModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.TagViewModels;

public partial class TagViewModel : ViewModelBase
{
    [Reactive]
    private int _id;

    [Reactive]
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
