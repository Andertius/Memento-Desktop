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
using Memento.Core.Responses;
using Memento.Core.Services;

namespace Memento.Core.HttpClients;

public interface ICardHttpClient
{
    Task<List<Card>> GetCards();

    Task<int> AddCard(Card card);

    Task UpdateCard(Card card);

    Task DeleteCard(int cardId);

    Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds);

    Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds);

    Task<string?> UploadImage(int cardId, ImageData image);

    Task DeleteImage(int cardId);
}

public sealed class CardHttpClient(IHttpClientFactory _clientFactory) : ICardHttpClient, IDisposable
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

    public async Task<List<Card>> GetCards()
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Get, ApiPaths.CardsApiPath);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Card>>() ?? [];
    }

    public async Task<int> AddCard(Card card)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.CardsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return Int32.TryParse(response.Headers.Location?.OriginalString.Split('/')[^1], out int id)
            ? id
            : 0;
    }

    public async Task UpdateCard(Card card)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, ApiPaths.CardsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCardCategories(int cardId, IReadOnlyCollection<int> categoryIds)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{ApiPaths.CardsApiPath}/{cardId}/categories");
        request.Content = new StringContent(JsonSerializer.Serialize(new { categoryIds }), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCardTags(int cardId, IReadOnlyCollection<int> tagIds)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{ApiPaths.CardsApiPath}/{cardId}/tags");
        request.Content = new StringContent(JsonSerializer.Serialize(new { tagIds }), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCard(int cardId)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.CardsApiPath}/{cardId}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
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

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var imageResponse = await response.Content.ReadFromJsonAsync<ImageResponse>();

        return imageResponse?.FileName;
    }

    public async Task DeleteImage(int cardId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.CardsApiPath}/{cardId}/image");

        string token = await GetToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
