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
using Memento.Core.Mappers;
using Memento.Core.Options;
using Memento.Core.Responses;
using Memento.Core.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Memento.Core.HttpClients;

public interface ICategoryHttpClient
{
    Task<List<Category>> GetAllCategories(string? filter, int? currentPage, int? pageSize);

    Task<int> AddCategory(Category category);

    Task UpdateCategory(Category category);

    Task DeleteCategory(int categoryId);

    Task UpdateCategoryTags(int categoryId, IReadOnlyCollection<int> tagIds);

    Task<string?> UploadImage(int categoryId, ImageData image);

    Task DeleteImage(int categoryId);
}

public sealed class CategoryHttpClient : ICategoryHttpClient, IDisposable
{
    private readonly HttpClient _apiClient;
    private readonly HttpClient _authClient;

    public CategoryHttpClient(IHttpClientFactory clientFactory, IOptions<SettingsOptions> _settingsOptions)
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

    public async Task<List<Category>> GetAllCategories(string? filter, int? currentPage, int? pageSize)
    {
        string token = await GetToken();
        var query = new Dictionary<string, string?>
        {
            ["filter"] = filter,
            ["skip"] = (currentPage * pageSize).ToString(),
            ["take"] = pageSize.ToString(),
        };

        string uri = QueryHelpers.AddQueryString(ApiPaths.CategoriesApiPath, query);
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<Category>>() ?? [];
    }

    public async Task<int> AddCategory(Category category)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Post, ApiPaths.CategoriesApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(category.ToRequest()), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return Int32.TryParse(response.Headers.Location?.OriginalString.Split('/')[^1], out int id)
            ? id
            : 0;
    }

    public async Task UpdateCategory(Category category)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, ApiPaths.CategoriesApiPath);
        request.Content = new StringContent(JsonSerializer.Serialize(category.ToRequest()), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategory(int categoryId)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.CategoriesApiPath}/{categoryId}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCategoryTags(int categoryId, IReadOnlyCollection<int> tagIds)
    {
        string token = await GetToken();
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{ApiPaths.CategoriesApiPath}/{categoryId}/tags");
        request.Content = new StringContent(JsonSerializer.Serialize(new { tagIds }), Encoding.UTF8, "application/json");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _apiClient.SendAsync(request);
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

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiPaths.CategoriesApiPath}/{categoryId}/image");

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

    public async Task DeleteImage(int categoryId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiPaths.CategoriesApiPath}/{categoryId}/image");

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
