using Discord.WebSocket;

namespace ogybot.Domain.Entities;

public class EmbedContent
{

    public EmbedContent()
    {
    }

    public EmbedContent(SocketUser socketUser, string queueSize, string description)
    {
        SocketUser = socketUser;
        QueueSize = queueSize;
        Description = description;
    }

    public SocketUser SocketUser { get; set; } = default!;
    public string? QueueSize { get; set; }
    public string? Description { get; set; }
}