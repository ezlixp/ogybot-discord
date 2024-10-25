using Discord;
using ogybot.Communication.Constants;

namespace ogybot.Bot.Commands.Base;

public class BasePermissionRequiredCommand : BaseCommand
{
    /// <summary>
    /// Validates the user and channel the message has been sent to determine whether the context is valid or not.
    /// </summary>
    /// <param name="channelId"></param>
    /// <returns></returns>
    protected async Task<bool> IsInvalidContextAsync(ulong channelId)
    {
        return await ValidateChannelAndRolesAsync(channelId);
    }

    private async Task<bool> ValidateChannelAndRolesAsync(ulong channelId)
    {

        if (await IsInvalidChannelAsync(channelId)) return true;

        return await ValidateUserRolesAsync();

    }

    private async Task<bool> ValidateUserRolesAsync()
    {

        var user = Context.User as IGuildUser;

        var roles = user!
            .RoleIds
            .Where(role => role is GuildRoleIds.AdminRole
                or GuildRoleIds.ChiefRole
                or GuildRoleIds.DeveloperRole);

        if (roles.Any()) return false;

        await FollowupAsync(ErrorMessages.NoPermissionError);

        return true;
    }
}