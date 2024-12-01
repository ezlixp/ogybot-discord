using Discord;
using Discord.Interactions;
using ogybot.Bot.Commands.Base;
using ogybot.Bot.Handlers;
using ogybot.Communication.Constants;

namespace ogybot.Bot.Commands.Misc;

public class PingChiefsCommand : BaseCommand
{
    public PingChiefsCommand(IBotExceptionHandler exceptionHandler) : base(exceptionHandler)
    {
    }

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("chiefs", "Pings the 'Active Chief' role")]
    public async Task ExecuteChiefsCommandAsync()
    {
        if (await IsInvalidChannelAsync(GuildChannels.WarChannel))
        {
            return;
        }

        await FollowupAsync($"<@&{GuildRoleIds.ActiveChiefRole}>", allowedMentions: AllowedMentions.All);
    }
}