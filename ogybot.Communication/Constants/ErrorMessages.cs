namespace ogybot.Communication.Constants;

public static class ErrorMessages
{
    public const string UnknownError = "An unknown error occurred while executing this command.";
    public const string InvalidChannelError = "This command cannot be used in this channel";
    public const string NoPermissionError = "You don't have permissions to use this command";

    public const string GetTokenError = "Error getting token";
    public const string AddUserToListError = "Error adding user to list";
    public const string DecrementUserAspectsError = "Error decrementing user aspect count";
    public const string RemoveUserFromListError = "Error removing user from list";
}