using System;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.TagViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace Memento.Core.ViewModels.TagViewModels;

public partial class CreateTagViewModel : DialogViewModelBase, ICreateTagViewModel, IValidatableViewModel
{
    private readonly ITagHttpClient _client;

    [Reactive]
    private TagViewModel _tag = new();

    public CreateTagViewModel(ITagHttpClient client)
    {
        _client = client;

        var canSave = this.WhenAnyValue(x => x.Tag.Name, name => !String.IsNullOrWhiteSpace(name));
        SaveTagCommand = ReactiveCommand.CreateFromTask(SaveTagAsync, canSave);

        this.ValidationRule(
            viewModel => viewModel.Tag.Name,
            canSave,
            "Name cannot be empty");
    }

    public IValidationContext ValidationContext { get; } = new ValidationContext();

    public ReactiveCommand<Unit, Unit> SaveTagCommand { get; }

    public async Task SaveTagAsync()
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
