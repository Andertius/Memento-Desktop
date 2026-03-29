using System;
using Memento.Core.Data;
using Memento.Core.ViewModels;

namespace Memento.Core.Factories;

public interface IPageViewModelFactory
{
    PageViewModel GetPageViewModel(ApplicationPageNames pageName);
}

public sealed class PageViewModelFactory(Func<ApplicationPageNames, PageViewModel> _factory) : IPageViewModelFactory
{
    public PageViewModel GetPageViewModel(ApplicationPageNames pageName)
        => _factory.Invoke(pageName);
}
