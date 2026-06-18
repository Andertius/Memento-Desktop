using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Memento.Core.Data;
using Memento.Core.DataModels;
using Memento.Core.Mappers;
using Memento.Core.Options;
using Memento.Core.Responses;
using Memento.Core.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Memento.Core.HttpClients;

public interface ICardHttpClient
{
    Task<List<Card>> GetAllCards(string? filter, int currentPage, int pageSize);

    Task<List<Card>> GetCards(int? categoryId = null, IReadOnlyCollection<int>? tagIds = null);

    Task<int> AddCard(Card card);

    Task UpdateCard(Card card);

    Task DeleteCard(int cardId);

    Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds);

    Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds);

    Task<string?> UploadImage(int cardId, ImageData image);

    Task DeleteImage(int cardId);
}

public sealed class CardHttpClient : ICardHttpClient, IDisposable
{
    private readonly HttpClient _apiClient;
    private readonly HttpClient _authClient;

    public CardHttpClient(IHttpClientFactory clientFactory, IOptions<SettingsOptions> _settingsOptions)
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

    public async Task<List<Card>> GetAllCards(string? filter, int currentPage, int pageSize)
    {
        string token = await GetToken();
        
        var query = new Dictionary<string, string?>
        {
            ["filter"] = filter,
            ["skip"] = (currentPage * pageSize).ToString(),
            ["take"] = pageSize.ToString(),
        };

        string uri = QueryHelpers.AddQueryString($"{ApiPaths.CardsApiPath}/all", query);
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Card>>() ?? [];
    }

    public async Task<List<Card>> GetCards(int? categoryId = null, IReadOnlyCollection<int>? tagIds = null)
    {
        string token = await GetToken();

        var query = HttpUtility.ParseQueryString("");
        query[nameof(categoryId)] = categoryId?.ToString();
        query[nameof(tagIds)] = tagIds is { Count: > 0 } ? String.Join(',', tagIds) : "[]";
        string? queryString = query.ToString();

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiPaths.CardsApiPath}?{queryString}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Card>>() ?? [];
    }

    public async Task<int> AddCard(Card card)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.CardsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(card.ToRequest()), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return Int32.TryParse(response.Headers.Location?.OriginalString.Split('/')[^1], out int id)
            ? id
            : 0;
    }

    public async Task UpdateCard(Card card)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, ApiPaths.CardsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(card.ToRequest()), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{ApiPaths.CardsApiPath}/{cardId}/categories");
        request.Content = new StringContent(JsonSerializer.Serialize(new { categoryIds }), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{ApiPaths.CardsApiPath}/{cardId}/tags");
        request.Content = new StringContent(JsonSerializer.Serialize(new { tagIds }), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCard(int cardId)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.CardsApiPath}/{cardId}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<string?> UploadImage(int cardId, ImageData image)
    {
        if (image.File is null || image.FilePath is null)
        {
            return null;
        }

        string fileName = Path.GetFileName(image.FilePath.AbsolutePath);
        string extension = Path.GetExtension(fileName);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiPaths.CardsApiPath}/{cardId}/image");

        using var imageContent = new StreamContent(image.File);
        imageContent.Headers.ContentType = ContentTypeHelper.FileExtensionToMediaTypeHeaderValue(extension);

        var content = new MultipartFormDataContent();
        content.Add(imageContent, "file", fileName);
        request.Content = content;

        string token = await GetToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var imageResponse = await response.Content.ReadFromJsonAsync<ImageResponse>();

        return imageResponse?.FileName;
    }

    public async Task DeleteImage(int cardId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.CardsApiPath}/{cardId}/image");

        string token = await GetToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _apiClient.Dispose();
    }
}
