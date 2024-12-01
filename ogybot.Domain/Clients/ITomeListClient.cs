using ogybot.Domain.Entities;

namespace ogybot.Domain.Clients;

public interface ITomeListClient
{
    Task<IList<TomeListUser>> GetListAsync();
    Task AddUserAsync(TomeListUser user);
    Task RemoveUserAsync(TomeListUser user);
}