namespace ogybot.Communication.Requests;

public class GetTokenRequest
{

    public GetTokenRequest(string validationKey)
    {
        ValidationKey = validationKey;
    }

    public GetTokenRequest()
    {

    }

    public string ValidationKey { get; set; } = string.Empty;
}