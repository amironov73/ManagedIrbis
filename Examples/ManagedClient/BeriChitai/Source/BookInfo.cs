using Newtonsoft.Json;

public class BookInfo
{
    [JsonProperty("selected")]
    public bool Selected { get; set; }

    [JsonProperty("mfn")]
    public int Mfn { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}
