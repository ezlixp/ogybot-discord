using Discord;
using Discord.Interactions;
using ogybot.Bot.Commands.Base;
using ogybot.Communication.Constants;

namespace ogybot.Bot.Commands.Misc;

public class PingChiefsCommand : BaseCommand
{
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