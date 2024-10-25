using System.Net.Http.Json;
using ogybot.Communication.Constants;
using ogybot.Communication.Exceptions;
using ogybot.Communication.Requests;
using ogybot.Communication.Responses;
using ogybot.Domain.Security;

namespace ogybot.Data.Security.Tokens;

public class TokenRequester : ITokenRequester
{
    private const string Endpoint = "auth/gettoken";

    private readonly HttpClient _httpClient;
    private readonly string _validationKey;

    public TokenRequester(HttpClient httpClient, string validationKey)
    {
        _httpClient = httpClient;
        _validationKey = validationKey;
    }

    public async Task<string> GetTokenAsync()
    {
        var response = await RequestTokenAsync();

        EnsureSuccessStatusCode(response);

        return await GetTokenFromResponseAsync(response);
    }

    private static void EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new CommunicationException(ExceptionMessages.InvalidApiToken);
        }
    }

    private async Task<HttpResponseMessage> RequestTokenAsync()
    {
        var request = new GetTokenRequest(_validationKey);

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
        return response;
    }

    private static async Task<string> GetTokenFromResponseAsync(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadFromJsonAsync<GetTokenResponse>();

        return responseContent!.Token;
    }
}