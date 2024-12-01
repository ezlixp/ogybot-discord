using ogybot.Domain.Clients;
using ogybot.Domain.Entities;
using ogybot.Domain.Security;

namespace ogybot.Data.Clients;

public class TomeListClient : BaseClient, ITomeListClient
{
    private const string Endpoint = "tomes";

    private readonly ITokenRequester _tokenRequester;

    public TomeListClient(HttpClient httpClient, ITokenRequester tokenRequester) : base(httpClient)
    {
        _tokenRequester = tokenRequester;
    }

    public async Task<IList<TomeListUser>> GetListAsync()
    {
        var method = HttpMethod.Get;

        var response = await MakeAndSendRequestAsync(method, Endpoint);

        var listOfUsers = await ParseResponseAsync<IList<TomeListUser>>(response);

        return listOfUsers;
    }

    public async Task AddUserAsync(TomeListUser user)
    {
        var method = HttpMethod.Post;
        var token = await _tokenRequester.GetTokenAsync();

        var response = await MakeAndSendRequestAsync(method, Endpoint, user, token);
    }

    public async Task RemoveUserAsync(TomeListUser user)
    {
        var method = HttpMethod.Delete;
        var token = await _tokenRequester.GetTokenAsync();

        var response = await MakeAndSendRouteRequestAsync(method, Endpoint, user.Username!, token);
    }
}