namespace ogybot.Communication.Constants;

public static class ExceptionMessages
{
    public const string InvalidBotToken = "The provided bot token is invalid.";
    public const string InvalidApiToken = "An error occurred requesting a token from the API.";
    public const string NullContent = "The parsed JSON content returned is null.";
    public const string InvalidSocketMessageArgument = "One or more fields provided are invalid.";
    public const string UnsuccessfulWebsocketStartup = "An error occurred and could not start the listener up.";
}