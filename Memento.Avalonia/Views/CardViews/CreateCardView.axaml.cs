using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Memento.Avalonia.Handlers;
using Memento.Core.DataModels;
using Memento.Core.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using ReactiveUI.Validation.Extensions;

namespace Memento.Avalonia.Views.CardViews;

public partial class CreateCardView : ReactiveUserControl<CreateCardViewModel>
{
    public CreateCardView()
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

            this.BindValidation(ViewModel, vm => vm.Card.Word, view => view.WordError.Text)
                .DisposeWith(disposables);

            this.BindValidation(ViewModel, vm => vm.Card.Translation, view => view.TranslationError.Text)
                .DisposeWith(disposables);
        });
    }

    private void OnCategoryCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox { DataContext: CategoryViewModel } checkBox)
        {
            return;
        }

        ViewModel?.Card.Categories = checkBox.IsChecked switch
        {
            true => AddCategory(ViewModel.Card.Categories, (CategoryViewModel)checkBox.DataContext),
            false => RemoveCategory(ViewModel.Card.Categories, ((CategoryViewModel)checkBox.DataContext).Id),
            _ => ViewModel!.Card.Categories,
        };
    }

    private void OnTagCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox { DataContext: TagViewModel } checkBox)
        {
            return;
        }

        ViewModel?.Card.Tags = checkBox.IsChecked switch
        {
            true => AddTag(ViewModel.Card.Tags, (TagViewModel)checkBox.DataContext),
            false => RemoveTag(ViewModel.Card.Tags, ((TagViewModel)checkBox.DataContext).Id),
            _ => ViewModel!.Card.Tags,
        };
    }

    private static IReadOnlyCollection<Category> AddCategory(IReadOnlyCollection<Category> categories, CategoryViewModel addedCategory)
        => categories.Any(x => x.Id == addedCategory.Id)
            ? categories
            : categories.Append(addedCategory.ToDataModel()).ToList();

    private static IReadOnlyCollection<Category> RemoveCategory(IReadOnlyCollection<Category> categories, int id)
    {
        var category = categories.FirstOrDefault(x => x.Id == id);

        return category is null
            ? categories
            : categories.Except([category]).ToList();
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
