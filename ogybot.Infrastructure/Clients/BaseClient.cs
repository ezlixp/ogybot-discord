using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ogybot.Communication.Constants;
using ogybot.Communication.Exceptions;

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

        AddOptionalFieldsToRequest(content, token, request);

        return await SendHttpRequest(request);
    }

    private static void AddOptionalFieldsToRequest(object? content, string? token, HttpRequestMessage request)
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

    private async Task<HttpResponseMessage> SendHttpRequest(HttpRequestMessage request)
    {
        return await _httpClient.SendAsync(request);
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