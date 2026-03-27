using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Memento.Avalonia.Data;
using Memento.Avalonia.DataModels;

namespace Memento.Avalonia.HttpClients;

public interface ICardHttpClient
{
    Task<List<Card>> GetCards();

    Task<int> AddCard(Card card);

    Task UpdateCard(Card card);

    Task UploadImage(int cardId, Stream image);
    Task DeleteImage(int cardId);
}

public sealed class CardHttpClient(IHttpClientFactory _clientFactory) : ICardHttpClient, IDisposable
{
    private readonly HttpClient _client = _clientFactory.CreateClient(ClientNames.CardClientName);

    private static async Task<string> GetToken()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5091");

        var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.TokenApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(new { Username = "test", Password = "test" }), Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<Token>())!.AccessToken;
    }

    public async Task<List<Card>> GetCards()
    {
        string token = await GetToken();
        var request = new HttpRequestMessage(HttpMethod.Get, ApiPaths.CardsApiPath);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Card>>() ?? [];
    }

    public async Task<int> AddCard(Card card)
    {
        string token = await GetToken();
        var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.CardsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return Int32.TryParse(response.Headers.Location?.OriginalString.Split('/')[^1], out int id)
            ? id
            : 0;
    }

    public async Task UpdateCard(Card card)
    {
        string token = await GetToken();
        var request = new HttpRequestMessage(HttpMethod.Put, ApiPaths.CardsApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UploadImage(int cardId, Stream image)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.CardsApiPath + $"/{cardId}/image");

        var imageContent = new StreamContent(image);
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Png);

        var content = new MultipartFormDataContent();
        content.Add(imageContent, "file", $"{cardId}.png");
        request.Content = content;

        string token = await GetToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteImage(int cardId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, ApiPaths.CardsApiPath + $"/{cardId}/image");

        string token = await GetToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private record Token(string AccessToken);
}
