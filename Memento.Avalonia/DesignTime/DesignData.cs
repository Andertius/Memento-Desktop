using Memento.Avalonia.DesignTime.ViewModels;
using Memento.Avalonia.DesignTime.ViewModels.CardViewModels;
using Memento.Avalonia.DesignTime.ViewModels.CategoriesViewModels;
using Memento.Avalonia.DesignTime.ViewModels.TagViewModels;

namespace Memento.Avalonia.DesignTime;

public static partial class DesignData
{
    public static MainViewModel MainViewModel { get; } = new();
    public static HomePageViewModel HomePageViewModel { get; } = new();
    public static SettingsViewModel SettingsViewModel { get; } = new();
    public static LearnViewModel LearnViewModel { get; } = new();

    public static CreateCardViewModel CreateCardViewModel { get; set; } = new();
    public static EditCardViewModel EditCardViewModel { get; set; } = new();
    public static ManageCardsViewModel ManageCardsViewModel { get; set; } = new();

    public static CreateCategoryViewModel CreateCategoryViewModel { get; set; } = new();
    public static EditCategoryViewModel EditCategoryViewModel { get; set; } = new();
    public static ManageCategoriesViewModel ManageCategoriesViewModel { get; set; } = new();

    public static CreateTagViewModel CreateTagViewModel { get; set; } = new();
    public static EditTagViewModel EditTagViewModel { get; set; } = new();
    public static ManageTagsViewModel ManageTagsViewModel { get; set; } = new();
}
