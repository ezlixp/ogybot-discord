using System.Text;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using ogybot.Bot.Commands.Base;
using ogybot.Bot.Handlers;

namespace ogybot.Bot.Commands.Misc;

public class InfoCommand : BaseCommand
{
    public InfoCommand(IBotExceptionHandler exceptionHandler) : base(exceptionHandler)
    {
    }

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("info", "Displays info about the bot, such as the github repositories.")]
    public async Task ExecuteInfoCommandAsync()
    {
        const string embedTitle = "Info";
        const string embedFooter = "Made by oxzy";
        var user = Context.User;

        var embed = CreateEmbed(embedTitle, embedFooter, user);

        await FollowupAsync(embed: embed);
    }

    private static Embed CreateEmbed(string embedTitle, string embedFooter, SocketUser user)
    {
        var description = BuildEmbedDescription();
        var embed = BuildEmbed(embedTitle, embedFooter, user, description);

        return embed;
    }

    private static string BuildEmbedDescription()
    {
        var description = new StringBuilder()
            .AppendLine("**Github Repos**")
            .AppendLine("Bot: https://github.com/zpdh/ogybot-discord")
            .AppendLine("API: https://github.com/ezlixp/ico_server")
            .ToString();

        return description;
    }

    private static Embed BuildEmbed(string embedTitle, string embedFooter, SocketUser user, string description)
    {
        var embed = new EmbedBuilder()
            .WithTitle(embedTitle)
            .WithFooter(embedFooter)
            .WithDescription(description)
            .WithAuthor(user)
            .WithTimestamp(DateTimeOffset.Now)
            .WithColor(Color.Teal)
            .Build();

        return embed;
    }

}