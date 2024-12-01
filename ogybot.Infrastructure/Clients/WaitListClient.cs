using ogybot.Domain.Clients;
using ogybot.Domain.Entities;
using ogybot.Domain.Security;

namespace ogybot.Data.Clients;

public class WaitListClient : BaseClient, IWaitListClient
{
    private const string Endpoint = "waitlist";

    private readonly ITokenRequester _tokenRequester;

    public WaitListClient(HttpClient httpClient, ITokenRequester tokenRequester) : base(httpClient)
    {
        _tokenRequester = tokenRequester;
    }

    public async Task<IList<WaitListUser>> GetListAsync()
    {
        var method = HttpMethod.Get;

        var response = await MakeAndSendRequestAsync(method, Endpoint);

        var listOfUsers = await ParseResponseAsync<IList<WaitListUser>>(response);

        return listOfUsers;
    }

    public async Task AddUserAsync(WaitListUser user)
    {
        var method = HttpMethod.Post;
        var token = await _tokenRequester.GetTokenAsync();

        var response = await MakeAndSendRequestAsync(method, Endpoint, user, token);
    }

    public async Task RemoveUserAsync(WaitListUser user)
    {
        var method = HttpMethod.Delete;
        var token = await _tokenRequester.GetTokenAsync();

        var response = await MakeAndSendRouteRequestAsync(method, Endpoint, user.Username!, token);
    }
}