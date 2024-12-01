namespace ogybot.Communication.Constants;

public static class ErrorMessages
{
    public const string UnknownError = "An unknown error occurred while executing this command. Contact a developer.";
    public const string InvalidChannelError = "This command cannot be used in this channel.";
    public const string NoPermissionError = "You don't have permissions to use this command.";
    public const string InvalidRaidTypeError = "This raid type is invalid.";
    public const string InvalidCharactersError = "The provided username contains one or more invalid characters.";
    public const string UsernameTooLongError = "The provided username is too long.";
    public const string UsernameTooShortError = "The provided username is too short.";
    public const string UsernameNotOnListError = "One or more provided username(s) are not on the list.";
    public const string UserAlreadyOnListError = "This user is already present on the list.";
    public const string InvalidIndexError = "The provided index(es) are invalid (either too high or equal to or less than 0).";
}