using ogybot.Communication.Constants;

namespace ogybot.Communication.Exceptions;

public class WebsocketStartupFailureException() : OgybotException(ExceptionMessages.UnsuccessfulWebsocketStartup);