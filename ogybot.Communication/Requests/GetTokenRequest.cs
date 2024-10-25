namespace ogybot.Communication.Requests;

public class GetTokenRequest
{
    public string ValidationKey { get; set; } = string.Empty;

    public GetTokenRequest(string validationKey)
    {
        ValidationKey = validationKey;
    }

    public GetTokenRequest()
    {

    }
}