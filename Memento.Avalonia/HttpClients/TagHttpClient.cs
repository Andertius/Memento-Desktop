using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Memento.Avalonia.Data;
using Memento.Avalonia.DataModels;
using Memento.Avalonia.Responses;

namespace Memento.Avalonia.HttpClients;

public interface ITagHttpClient
{
    Task<List<Tag>> GetTags();

    Task<int> AddTag(Tag tag);

    Task UpdateTag(Tag tag);

    Task DeleteTag(int tagId);
}

public sealed class TagHttpClient(IHttpClientFactory _clientFactory) : ITagHttpClient, IDisposable
{
    private readonly HttpClient _client = _clientFactory.CreateClient(ClientNames.ApiClientName);

    private static async Task<string> GetToken()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5091");

        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.TokenApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(new { Username = "test", Password = "test" }), Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;
    }

    public async Task<List<Tag>> GetTags()
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Get, ApiPaths.TagsApiPath);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Tag>>() ?? [];
    }

    public async Task<int> AddTag(Tag tag)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.TagsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
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

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTag(int tagId)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.TagsApiPath}/{tagId}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
