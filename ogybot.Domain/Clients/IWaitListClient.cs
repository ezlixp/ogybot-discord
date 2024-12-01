using ogybot.Domain.Entities;

namespace ogybot.Domain.Clients;

public interface IWaitListClient
{
    Task<IList<WaitListUser>> GetListAsync();
    Task AddUserAsync(WaitListUser user);
    Task RemoveUserAsync(WaitListUser user);
}