using Newtonsoft.Json;

public class OrderResult
{
    [JsonProperty("ok")]
    public bool Ok { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
