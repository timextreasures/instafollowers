using Newtonsoft.Json;

namespace InstaFollowers.WP.Model.Instagram
{
  public class AccessTokenResponse
  {
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
  }
}
