using Discord;
using Discord.WebSocket;
using ogybot.Communication.Exceptions;
using ogybot.Domain.Entities;
using ogybot.Domain.Sockets.ChatSocket;
using ogybot.Utility.Services;

namespace ogybot.Data.Sockets.Chat;

public class ChatSocketMessageHandler : IChatSocketMessageHandler
{
    private const int DelayBetweenMessages = 250;

    public async Task FormatAndSendEmbedAsync(IMessageChannel channel, ChatSocketMessage chatSocketResponse)
    {
        var messageEmbed = FormatMessageIntoEmbed(chatSocketResponse);
        await SendEmbedAsync(channel, messageEmbed);
    }

    private static Embed FormatMessageIntoEmbed(ChatSocketMessage message)
    {
        var embedBuilder = new EmbedBuilder();
        var cleanedString = CleanUpResponseString(message, embedBuilder);

        var embed = CreateEmbed(embedBuilder, cleanedString);

        return embed;
    }

    private static Embed CreateEmbed(EmbedBuilder embedBuilder, string cleanedString)
    {

        embedBuilder.WithDescription(cleanedString);

        var embed = embedBuilder.Build();
        return embed;
    }

    private static string CleanUpResponseString(ChatSocketMessage message, EmbedBuilder embedBuilder)
    {

        var formattedMessage = message.TextContent;

        // Can return null if message type is guild message
        var newFormattedMessage = FormatEmbedAccordingToMessageType(message, embedBuilder);
        formattedMessage = newFormattedMessage ?? formattedMessage;

        var cleanedString = WhitespaceRemovalService.RemoveExcessWhitespaces(formattedMessage);

        return cleanedString;
    }

    private static string? FormatEmbedAccordingToMessageType(ChatSocketMessage response, EmbedBuilder embedBuilder)
    {
        string? message = null;

        switch (response.MessageType)
        {
            case SocketMessageType.ChatMessage:
                embedBuilder
                    .WithColor(Color.Blue);

                message = $"**{response.HeaderContent}:** {response.TextContent}";
                break;

            case SocketMessageType.DiscordMessage:
                embedBuilder
                    .WithAuthor("Discord Only")
                    .WithColor(Color.Purple);

                message = $"**{response.HeaderContent}:** {response.TextContent}";
                break;

            case SocketMessageType.GuildMessage:
                embedBuilder
                    .WithAuthor(response.HeaderContent)
                    .WithColor(Color.Teal);

                break;

            default:
                throw new InvalidSocketArgumentException();
        }

        return message;
    }

    private static async Task SendEmbedAsync(IMessageChannel channel, Embed embed)
    {
        // Small delay to prevent going over discord's rate limit
        await Task.Delay(DelayBetweenMessages);
        await channel.SendMessageAsync(embed: embed);
    }

    public async Task SendLoggingMessageAsync(IMessageChannel channel, string message)
    {
        var logEmbed = CreateLogEmbed(message);

        await channel.SendMessageAsync(embed: logEmbed);
    }

    private static Embed CreateLogEmbed(string message)
    {
        var messageAsEmbed = new EmbedBuilder()
            .WithColor(Color.Teal)
            .WithTitle("Websocket Log")
            .WithDescription(message)
            .Build();

        return messageAsEmbed;
    }

    public string AddReplyAuthorToField(SocketUserMessage message, string author)
    {
        var replyAuthor = message.ReferencedMessage.Author;
        author += $" (Replying to {replyAuthor})";
        return author;
    }
}