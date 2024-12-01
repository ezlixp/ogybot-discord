using ogybot.Domain.Clients;
using ogybot.Domain.Entities;
using ogybot.Domain.Security;

namespace ogybot.Data.Clients;

public class AspectListClient : BaseClient, IAspectListClient
{
    private const string Endpoint = "aspects";

    private readonly ITokenRequester _tokenRequester;

    public AspectListClient(HttpClient httpClient, ITokenRequester tokenRequester) : base(httpClient)
    {
        _tokenRequester = tokenRequester;
    }

    public async Task<IList<AspectListUser>> GetListAsync()
    {
        var method = HttpMethod.Get;

        var response = await MakeAndSendRequestAsync(method, Endpoint);

        var listOfUsers = await ParseResponseAsync<IList<AspectListUser>>(response);

        return listOfUsers;
    }

    public async Task DecrementAspectAsync(AspectListUser user)
    {
        var method = HttpMethod.Post;
        var token = await _tokenRequester.GetTokenAsync();

        var response = await MakeAndSendRequestAsync(method, Endpoint, user, token);
    }
}