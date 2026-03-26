using System;
using Memento.Avalonia.Constants;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels;
using Memento.Avalonia.ViewModels.CardViewModels;
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
            services.AddTransient<IPageFactory, PageFactory>();

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
            services.Configure<CardClientOptions>(configuration.GetSection(nameof(CardClientOptions)));

            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddTransient<IDialogService, DialogService>();

            return services;
        }

        public IServiceCollection AddClients()
        {
            services.AddHttpClient(ClientNames.CardClientName, (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<CardClientOptions>>();
                client.BaseAddress = new Uri($"{options.Value.Host}");
            });

            services.AddTransient<ICardHttpClient, CardHttpClient>();

            return services;
        }
    }
}
