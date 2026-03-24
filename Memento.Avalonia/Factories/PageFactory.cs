using System;
using Memento.Avalonia.Data;
using Memento.Avalonia.ViewModels;

namespace Memento.Avalonia.Factories;

public interface IPageFactory
{
    PageViewModel GetPageViewModel(ApplicationPageNames pageName);
}

public sealed class PageFactory(Func<ApplicationPageNames, PageViewModel> _factory) : IPageFactory
{
    public PageViewModel GetPageViewModel(ApplicationPageNames pageName) => _factory.Invoke(pageName);
}
