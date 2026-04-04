using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Memento.Core.DataModels;
using Memento.Core.ViewModels.LearnViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views.LearnViews;

public partial class LearnMainView : ReactiveUserControl<MainLearnViewModel>
{
    public LearnMainView()
    {
        InitializeComponent();
    }

    private void OnTagCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox { DataContext: Memento.Core.DataModels.Tag } checkBox)
        {
            return;
        }

        ViewModel?.SelectedTags = checkBox.IsChecked switch
        {
            true => AddTag(ViewModel.SelectedTags, (Tag)checkBox.DataContext),
            false => RemoveTag(ViewModel.SelectedTags, ((Tag)checkBox.DataContext).Id),
            _ => ViewModel!.SelectedTags,
        };
    }

    private static IReadOnlyCollection<Tag> AddTag(IReadOnlyCollection<Tag> tags, Tag addedTag)
        => tags.Any(x => x.Id == addedTag.Id)
            ? tags
            : tags.Append(addedTag).ToList();

    private static IReadOnlyCollection<Tag> RemoveTag(IReadOnlyCollection<Tag> tags, int id)
    {
        var tag = tags.FirstOrDefault(x => x.Id == id);

        return tag is null
            ? tags
            : tags.Except([tag]).ToList();
    }
}
