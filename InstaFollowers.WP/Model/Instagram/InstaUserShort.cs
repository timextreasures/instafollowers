using Newtonsoft.Json;

namespace InstaFollowers.WP.Model.Instagram
{
  public class InstaUserShort
  {
    [JsonProperty("username")]
    public string UserName { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("profile_picture")]
    public string ProfilePicture { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }
  }
}
