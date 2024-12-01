using ogybot.Domain.Clients;
using ogybot.Domain.Entities;

namespace ogybot.Data.Clients;

public class OnlineClient : BaseClient, IOnlineClient
{
    private const string Endpoint = "wynn/online";


    public OnlineClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IList<OnlineUser>> GetListAsync()
    {
        var method = HttpMethod.Get;

        var response = await MakeAndSendRequestAsync(method, Endpoint);

        var listOfUsers = await ParseResponseAsync<IList<OnlineUser>>(response);

        return listOfUsers;
    }
}