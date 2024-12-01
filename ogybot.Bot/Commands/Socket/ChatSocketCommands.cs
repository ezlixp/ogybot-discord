using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using ogybot.Bot.Commands.Base;
using ogybot.Bot.Handlers;
using ogybot.Communication.Constants;
using ogybot.Domain.Clients;
using ogybot.Domain.Entities;

namespace ogybot.Bot.Commands.Socket;

public class ChatSocketCommands : BaseCommand
{
    private readonly IOnlineClient _client;

    public ChatSocketCommands(IOnlineClient client, IBotExceptionHandler exceptionHandler) : base(exceptionHandler)
    {
        _client = client;
    }

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("online", "Lists online players with the mod.")]
    public async Task ExecuteOnlineCommandAsync()
    {
        if (await IsInvalidChannelAsync(GuildChannels.WebsocketLogChannel))
        {
            return;
        }

        var embed = await CreateEmbedAsync();

        await FollowupAsync(embed: embed);
    }

    private async Task<Embed> CreateEmbedAsync()
    {
        // Create class to store this info later
        var (user, queueSize, description) = await GetEmbedContentAsync();

        var embedBuilder = new EmbedBuilder()
            .WithAuthor(user.Username, user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
            .WithTitle("Online List")
            .WithDescription(description)
            .WithColor(Color.Teal)
            .WithCurrentTimestamp();

        return embedBuilder.Build();
    }

    private async Task<(SocketUser user, string queueSize, string description)> GetEmbedContentAsync()
    {
        var user = Context.User;

        var list = await _client.GetListAsync();

        var queueSize = "Players in queue: " + list.Count;

        var description = CreateEmbedDescription(list);

        return (user, queueSize, description);
    }

    private static string CreateEmbedDescription(IList<OnlineUser> list)
    {
        var description = "";

        var counter = 1;

        foreach (var onlineUser in list)
        {
            description += $"{counter}. {onlineUser.Username}\n";

            counter++;
        }

        return description;
    }
}