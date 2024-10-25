using ogybot.Communication.Constants;
using ogybot.Communication.Exceptions;

namespace ogybot.Entities.Exceptions;

public class InvalidBotTokenException() : OgybotException(ExceptionMessages.InvalidBotToken);