using ogybot.Domain.Entities;

namespace ogybot.Domain.Clients;

public interface IOnlineClient
{
    Task<IList<OnlineUser>> GetListAsync();
}