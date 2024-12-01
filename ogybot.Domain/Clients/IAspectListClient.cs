using ogybot.Domain.Entities;

namespace ogybot.Domain.Clients;

public interface IAspectListClient
{
    Task<IList<AspectListUser>> GetListAsync();
    Task DecrementAspectAsync(AspectListUser user);
}