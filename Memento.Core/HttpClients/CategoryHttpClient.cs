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

public interface ICategoryHttpClient
{
    Task<List<Category>> GetCategories();

    Task<int> AddCategory(Category category);

    Task UpdateCategory(Category category);

    Task DeleteCategory(int categoryId);

    Task<string?> UploadImage(int categoryId, ImageData image);

    Task DeleteImage(int categoryId);
}

public sealed class CategoryHttpClient(IHttpClientFactory _clientFactory) : ICategoryHttpClient, IDisposable
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

    public async Task<List<Category>> GetCategories()
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Get, ApiPaths.CategoriesApiPath);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Category>>() ?? [];
    }

    public async Task<int> AddCategory(Category category)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.CategoriesApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return Int32.TryParse(response.Headers.Location?.OriginalString.Split('/')[^1], out int id)
            ? id
            : 0;
    }

    public async Task UpdateCategory(Category category)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, ApiPaths.CategoriesApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategory(int categoryId)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.CategoriesApiPath}/{categoryId}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<string?> UploadImage(int categoryId, ImageData image)
    {
        if (image.File is null || image.FilePath is null)
        {
            return null;
        }

        string fileName = Path.GetFileName(image.FilePath.AbsolutePath);
        string extension = Path.GetExtension(fileName);

        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.CategoriesApiPath + $"/{categoryId}/image");

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

    public async Task DeleteImage(int categoryId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, ApiPaths.CategoriesApiPath + $"/{categoryId}/image");

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
