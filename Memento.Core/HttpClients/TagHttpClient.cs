using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.DataModels;
using Memento.Core.Options;
using Memento.Core.Responses;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Memento.Core.HttpClients;

public interface ITagHttpClient
{
    Task<List<Tag>> GetTags();

    Task<int> AddTag(Tag tag);

    Task UpdateTag(Tag tag);

    Task DeleteTag(int tagId);
}

public sealed class TagHttpClient : ITagHttpClient, IDisposable
{
    private readonly HttpClient _apiClient;
    private readonly HttpClient _authClient;

    public TagHttpClient(IHttpClientFactory clientFactory, IOptions<SettingsOptions> _settingsOptions)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var settings = deserializer.Deserialize<SettingsData>(File.ReadAllText(_settingsOptions.Value.SettingsPath));

        _apiClient = clientFactory.CreateClient(ClientNames.GetApiClientName(settings.ShouldUseVpn));
        _authClient = clientFactory.CreateClient(ClientNames.GetAuthClientName(settings.ShouldUseVpn));
    }

    private async Task<string> GetToken()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.TokenApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(new { Username = "Spaghet", Password = "82634239" }), Encoding.UTF8, "application/json");

        var response = await _authClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;
    }

    public async Task<List<Tag>> GetTags()
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Get, ApiPaths.TagsApiPath);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Tag>>() ?? [];
    }

    public async Task<int> AddTag(Tag tag)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.TagsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return Int32.TryParse(response.Headers.Location?.OriginalString.Split('/')[^1], out int id)
            ? id
            : 0;
    }

    public async Task UpdateTag(Tag tag)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, ApiPaths.TagsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTag(int tagId)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.TagsApiPath}/{tagId}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _apiClient.Dispose();
    }
}
