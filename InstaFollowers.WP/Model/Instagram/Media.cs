using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaFollowers.WP.Model.Instagram
{
  public abstract class InstaResponse
  {
    [JsonProperty("meta")]
    public Meta Meta { get; set; }
  }

  public class Media : InstaResponse
  {
    [JsonProperty("data")]
    public Datum Data { get; set; }
  }

  public class MediaByUserResponse : MedialListResponse
  {
    [JsonProperty("pagination")]
    public Pagination Pagination { get; set; }
  }

  public class Pagination
  {
     [JsonProperty("next_url")]
    public string NextUrl { get; set; }

     [JsonProperty("next_max_id")]
    public string NextMaxId { get; set; }
  }

  public class GetUsersResponse : InstaResponse
  {
    [JsonProperty("data")]
    public List<InstaUserShort> Data { get; set; }

  }

  public class UserResponse : InstaResponse
  {
    [JsonProperty("data")]
    public InstaUser Data { get; set; }

  }
}
