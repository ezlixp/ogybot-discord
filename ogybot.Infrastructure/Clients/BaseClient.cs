using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ogybot.Communication.Constants;
using ogybot.Communication.Exceptions;
using ogybot.Domain.Entities;

namespace ogybot.Data.Clients;

public abstract class BaseClient
{
    private readonly HttpClient _httpClient;

    protected BaseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<HttpResponseMessage> MakeAndSendRequestAsync(
        HttpMethod method,
        string endpoint,
        object? content = null,
        string? token = null)
    {
        var request = new HttpRequestMessage(method, endpoint);

        AddOptionalFieldsToRequest(request, content, token);

        var response = await SendHttpRequestAsync(request);

        await EnsureSuccessStatusCodeAsync(response);

        return response;
    }

    private static void AddOptionalFieldsToRequest(HttpRequestMessage request, object? content = null, string? token = null)
    {
        if (content != null)
        {
            AddContentToRequest(content, request);
        }

        if (token != null)
        {
            AddTokenToHeader(token, request);
        }
    }

    private static void AddContentToRequest(object? content, HttpRequestMessage request)
    {
        request.Content = JsonContent.Create(content);
    }

    private static void AddTokenToHeader(string? token, HttpRequestMessage request)
    {
        if (request.Headers.Contains("Authorization"))
        {
            request.Headers.Remove("Authorization");
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request)
    {
        return await _httpClient.SendAsync(request);
    }

    private static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
    {

        if (!response.IsSuccessStatusCode)
        {
            var error = await ParseResponseAsync<ApiError>(response);

            throw new ApiException(error.Error);
        }
    }

    protected async Task<HttpResponseMessage> MakeAndSendRouteRequestAsync(
        HttpMethod method,
        string endpoint,
        string route,
        string? token = null)
    {
        var request = new HttpRequestMessage(method, $"{endpoint}/{route}");

        AddOptionalFieldsToRequest(request, token: token);

        return await SendHttpRequestAsync(request);
    }

    protected static async Task<T> ParseResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadFromJsonAsync<T>();

        EnsureContentIsNotNull(content);

        return content;
    }

    private static void EnsureContentIsNotNull<T>([NotNull] T? content)
    {
        if (content is null)
        {
            throw new CommunicationException(ExceptionMessages.NullContent);
        }
    }
}