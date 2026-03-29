using System.Threading.Tasks;
using Memento.Core.HttpClients;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.TagViewModels;

public partial class CreateTagViewModel(ITagHttpClient client) : DialogViewModelBase
{
    [Reactive]
    private TagViewModel _tag = new();

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public CreateTagViewModel() : this(null!)
    {
    }

    [ReactiveCommand]
    public async Task SaveTag()
    {
        var tag = Tag.ToDataModel();
        Tag.Id = await client.AddTag(tag);

        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Close();
    }
}
