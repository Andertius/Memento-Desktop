using System;
using System.IO;
using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;
using Memento.Core.Options;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Memento.Core.ViewModels;

public partial class SettingsViewModel : PageViewModel, ISettingsViewModel
{
    private readonly SettingsOptions _options;
    private readonly SettingsData _settings;
    private readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    [Reactive]
    private bool _shouldUseVpn;

    public SettingsViewModel(IOptions<SettingsOptions> options)
        : base(ApplicationPageNames.Settings)
    {
        _options = options.Value;

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _settings = deserializer.Deserialize<SettingsData>(File.ReadAllText(_options.SettingsPath));
        _shouldUseVpn = _settings.ShouldUseVpn;

        this.WhenAnyValue(x => x.ShouldUseVpn)
            .Subscribe(UpdateSettings);
    }

    private void UpdateSettings(bool shouldUseVpn)
    {
        _settings.ShouldUseVpn = shouldUseVpn;
        string yaml = _serializer.Serialize(_settings);
        File.WriteAllText(_options.SettingsPath, yaml);
    }
}
