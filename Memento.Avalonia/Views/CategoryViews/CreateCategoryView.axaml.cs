using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Memento.Avalonia.Handlers;
using Memento.Core.DataModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using ReactiveUI.Validation.Extensions;

namespace Memento.Avalonia.Views.CategoryViews;

public partial class CreateCategoryView : ReactiveUserControl<CreateCategoryViewModel>
{
    public CreateCategoryView()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
        {
            return;
        }

        this.WhenActivated(disposables =>
        {
            ViewModel!.OpenFile.RegisterHandler(async context =>
            {
                var result = await FileHandler.OpenImage(this);
                context.SetOutput(result);
            }).DisposeWith(disposables);

            this.BindValidation(ViewModel, vm => vm.Category.Name, view => view.NameError.Text)
                .DisposeWith(disposables);

            this.BindValidation(ViewModel, vm => vm.Category.Description, view => view.DescriptionError.Text)
                .DisposeWith(disposables);
        });
    }

    private void OnTagCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox { DataContext: TagViewModel } checkBox)
        {
            return;
        }

        ViewModel?.Category.Tags = checkBox.IsChecked switch
        {
            true => AddTag(ViewModel.Category.Tags, (TagViewModel)checkBox.DataContext),
            false => RemoveTag(ViewModel.Category.Tags, ((TagViewModel)checkBox.DataContext).Id),
            _ => ViewModel!.Category.Tags,
        };
    }

    private static IReadOnlyCollection<Tag> AddTag(IReadOnlyCollection<Tag> tags, TagViewModel addedTag)
        => tags.Any(x => x.Id == addedTag.Id)
            ? tags
            : tags.Append(addedTag.ToDataModel()).ToList();

    private static IReadOnlyCollection<Tag> RemoveTag(IReadOnlyCollection<Tag> tags, int id)
    {
        var tag = tags.FirstOrDefault(x => x.Id == id);

        return tag is null
            ? tags
            : tags.Except([tag]).ToList();
    }
}
