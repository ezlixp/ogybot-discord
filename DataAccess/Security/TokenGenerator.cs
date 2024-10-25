using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ogybot.DataAccess.Entities;

namespace ogybot.DataAccess.Security;

public class TokenGenerator
{
    private readonly HttpClient _client;
    private readonly string _validationKey;

    public TokenGenerator(HttpClient client, string validationKey)
    {
        _client = client;
        _validationKey = validationKey;
    }

    /// <summary>
    /// Requests and gets a validation token from API
    /// </summary>
    /// <returns>
    /// Token or null if request is invalid
    /// </returns>
    public async Task<string?> GetTokenAsync()
    {
        // Serialize validation key to JSON
        var json = JsonConvert.SerializeObject(new
        {
            validationKey = _validationKey
        });

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        // Get token response from API
        var response = await _client.PostAsync("auth/get-token", content);

        if (!response.IsSuccessStatusCode) return null;

        var apiResponse = await response.Content.ReadFromJsonAsync<TokenApiResponse>();
        return apiResponse!.Token;
    }
}
