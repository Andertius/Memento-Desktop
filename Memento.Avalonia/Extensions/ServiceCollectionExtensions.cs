using System;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels;
using Memento.Avalonia.ViewModels.CardViewModels;
using Memento.Avalonia.ViewModels.CategoryViewModels;
using Memento.Avalonia.ViewModels.TagViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Memento.Avalonia.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddViewModels()
        {
            services.AddSingleton<MainViewModel>();
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<LearnViewModel>();
            services.AddTransient<ManageCardsViewModel>();
            services.AddTransient<ManageCategoriesViewModel>();
            services.AddTransient<ManageTagsViewModel>();

            return services;
        }

        public IServiceCollection AddFactories()
        {
            services.AddTransient<IPageViewModelFactory, PageViewModelFactory>();
            services.AddTransient<ICardViewModelFactory, CardViewModelFactory>();
            services.AddTransient<ICategoryViewModelFactory, CategoryViewModelFactory>();
            services.AddTransient<ITagViewModelFactory, TagViewModelFactory>();

            services.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(sp => name => name switch
            {
                ApplicationPageNames.HomePage => sp.GetRequiredService<HomePageViewModel>(),
                ApplicationPageNames.Learn => sp.GetRequiredService<LearnViewModel>(),
                ApplicationPageNames.Settings => sp.GetRequiredService<SettingsViewModel>(),
                ApplicationPageNames.ManageCards => sp.GetRequiredService<ManageCardsViewModel>(),
                ApplicationPageNames.ManageCategories => sp.GetRequiredService<ManageCategoriesViewModel>(),
                ApplicationPageNames.ManageTags => sp.GetRequiredService<ManageTagsViewModel>(),
                _ => throw new InvalidOperationException("Provided page does not exist."),
            });

            return services;
        }

        public IServiceCollection AddOptions(IConfiguration configuration)
        {
            services.Configure<ApiClientOptions>(configuration.GetSection(nameof(ApiClientOptions)));

            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddTransient<IDialogService, DialogService>();

            return services;
        }

        public IServiceCollection AddClients()
        {
            services.AddHttpClient(ClientNames.ApiClientName, (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ApiClientOptions>>();
                client.BaseAddress = new Uri($"{options.Value.Host}");
            });

            services.AddTransient<ICardHttpClient, CardHttpClient>();
            services.AddTransient<ICategoryHttpClient, CategoryHttpClient>();
            services.AddTransient<ITagHttpClient, TagHttpClient>();

            return services;
        }
    }
}
