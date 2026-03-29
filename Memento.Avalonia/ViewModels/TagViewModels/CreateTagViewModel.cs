using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.ViewModels.DialogViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.TagViewModels;

public partial class CreateTagViewModel : DialogViewModelBase
{
    [Reactive]
    private TagViewModel _tag = new();

    private readonly ITagHttpClient _client;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public CreateTagViewModel()
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

        _client = null!;
    }

    public CreateTagViewModel(ITagHttpClient client)
    {
        _client = client;
    }

    [ReactiveCommand]
    public async Task SaveTag()
    {
        var tag = Tag.ToDataModel();
        Tag.Id = await _client.AddTag(tag);

        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Close();
    }
}
