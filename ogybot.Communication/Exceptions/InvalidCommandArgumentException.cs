using ogybot.Communication.Constants;

namespace ogybot.Communication.Exceptions;

public class InvalidCommandArgumentException(string message) : OgybotException(message);