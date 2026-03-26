using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Memento.Avalonia.Constants;
using Memento.Avalonia.DataModels;

namespace Memento.Avalonia.HttpClients;

public interface ICardHttpClient
{
    Task<List<Card>> GetCards();

    Task<int> AddCard(Card card);

    Task UpdateCard(Card card);
}

public sealed class CardHttpClient(IHttpClientFactory _clientFactory) : ICardHttpClient
{
    private readonly HttpClient _client = _clientFactory.CreateClient(ClientNames.CardClientName);

    private async Task<string> GetToken()
    {
        var client = new HttpClient { BaseAddress = new Uri("http://localhost:5091") };

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

    private record Token(string AccessToken);
}
