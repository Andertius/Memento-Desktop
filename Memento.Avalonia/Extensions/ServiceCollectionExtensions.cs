using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels;
using Memento.Core.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReactiveUI;

namespace Memento.Avalonia.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddViews()
        {
            var openGenericType = typeof(IViewFor<>);
            var types = Assembly.GetExecutingAssembly().GetTypes();

            IEnumerable<(Type ServiceType, Type ServiceImplementation)> query = from type in types
                where !type.IsAbstract && !type.IsGenericTypeDefinition
                let matchingInterface = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
                where matchingInterface is not null
                select (matchingInterface, type);

            foreach (var result in query)
            {
                services.AddTransient(result.ServiceType, result.ServiceImplementation);
            }

            return services;
        }

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
