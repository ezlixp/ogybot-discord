using Discord;
using Discord.WebSocket;
using ogybot.Domain.Entities;
using ogybot.Domain.Sockets.ChatSocket;
using ogybot.Utility.Extensions;
using ogybot.Utility.Services;

namespace ogybot.Data.Sockets.Chat;

public class ChatSocketCommunicationHandler : IChatSocketCommunicationHandler
{
    private readonly IChatSocketMessageHandler _messageHandler;
    private readonly IChatSocketSetupHandler _setupHandler;
    private readonly SocketIOClient.SocketIO _socket;

    public ChatSocketCommunicationHandler(
        IChatSocketMessageHandler messageHandler,
        SocketIOClient.SocketIO socket,
        IChatSocketSetupHandler setupHandler)
    {
        _messageHandler = messageHandler;
        _socket = socket;
        _setupHandler = setupHandler;
    }

    public void SetupEventListeners(IMessageChannel channel)
    {

        #region Websocket Events

        _socket.On("wynnMessage",
            async response => {
                var socketResponse = response.GetValue<ChatSocketMessage>();

                if (!socketResponse.TextContent.IsNullOrWhitespace())
                {
                    await _messageHandler.FormatAndSendEmbedAsync(channel, socketResponse);
                }
            });

        #endregion

        #region Websocket Connectivity Events

        _socket.OnConnected += async (_, _) => {
            const string message = "Successfully connected to Websocket Server";

            Console.WriteLine(message);
            await _messageHandler.SendLoggingMessageAsync(channel, message);
        };

        _socket.OnDisconnected += async (_, reason) => {
            var message = $"Disconnected from Websocket Server. Reason: {reason}";

            Console.WriteLine(message);
            await _messageHandler.SendLoggingMessageAsync(channel, message);

            await _setupHandler.RequestAndRefreshTokenInHeadersAsync();

            await _socket.ConnectAsync();
        };

        _socket.OnReconnectFailed += async (_, _) => {
            const string message = "Could not reconnect to Websocket Server.";

            Console.WriteLine(message);
            await _messageHandler.SendLoggingMessageAsync(channel, message);
        };

        #endregion

    }

    public async Task EmitMessageAsync(SocketUserMessage message)
    {
        var authorField = message.Author.Username;
        var cleanedContent = WhitespaceRemovalService.RemoveExcessWhitespaces(message.CleanContent).Trim();

        if (MessageIsReply(message))
        {
            authorField = _messageHandler.AddReplyAuthorToField(message, authorField);
        }

        await _socket.EmitAsync("discordMessage",
            new DiscordMessage(authorField, cleanedContent));
    }

    public void SetupEmitter(DiscordSocketClient client, IMessageChannel channel)
    {
        client.MessageReceived += message => SetupMessageReceiverAsync(message, channel);
    }

    private static bool MessageIsReply(SocketUserMessage message)
    {
        return message.ReferencedMessage != null;
    }

    private async Task SetupMessageReceiverAsync(SocketMessage message, IMessageChannel channel)
    {
        if (message.Channel.Id != channel.Id) return;
        if (message.Author.IsBot || message is not SocketUserMessage userMessage) return;

        await EmitMessageAsync(userMessage);
    }
}