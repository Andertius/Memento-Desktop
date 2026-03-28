using System;
using Memento.Avalonia.Data;
using Memento.Avalonia.ViewModels;

namespace Memento.Avalonia.Factories;

public interface IPageViewModelFactory
{
    PageViewModel GetPageViewModel(ApplicationPageNames pageName);
}

public sealed class PageViewModelFactory(Func<ApplicationPageNames, PageViewModel> _factory) : IPageViewModelFactory
{
    public PageViewModel GetPageViewModel(ApplicationPageNames pageName)
        => _factory.Invoke(pageName);
}
