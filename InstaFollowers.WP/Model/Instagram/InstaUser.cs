using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InstaFollowers.WP.Model.Instagram
{
  public class InstaUser
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("username")]
    public string UserName { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; }

    [JsonProperty("profile_picture")]
    public string ProfilePicture { get; set; }

    [JsonProperty("bio")]
    public string Bio { get; set; }

    [JsonProperty("website")]
    public string Website { get; set; }

    [JsonProperty("counts")]
    public UserCounts Counts { get; set; }

  }

  public class UserCounts
  {
    [JsonProperty("media")]
    public int Media { get; set; }

    [JsonProperty("follows")]
    public int Follows { get; set; }

    [JsonProperty("followed_by")]
    public int FollowedBy { get; set; }
  }
}
