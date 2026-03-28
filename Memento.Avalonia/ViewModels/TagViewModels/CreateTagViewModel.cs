using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.ViewModels.DialogViewModels;

namespace Memento.Avalonia.ViewModels.TagViewModels;

public partial class CreateTagViewModel : DialogViewModelBase
{
    [ObservableProperty]
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

    [RelayCommand]
    public async Task SaveTag()
    {
        var tag = Tag.ToDataModel();
        Tag.Id = await _client.AddTag(tag);

        Close();
    }

    [RelayCommand]
    public void Cancel()
    {
        Close();
    }
}
