using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InstaFollowers.WP.Model.Instagram;
using Newtonsoft.Json;

namespace InstaFollowers.WP.Services
{
  public class InstaClient
  {
    private readonly string _token;

    public InstaClient(string token)
    {
      _token = token;
    }

    public async Task<string> Authorize(string code)
    {
      using (var httpClient = new HttpClient())
      {
        var parameters = new List<KeyValuePair<string, string>>
        {
          new KeyValuePair<string, string>("client_id", InstagramConfiguration.ClientId ),
          new KeyValuePair<string, string>("client_secret", InstagramConfiguration.ClientSecret ),
          new KeyValuePair<string, string>("grant_type", "authorization_code" ),
          new KeyValuePair<string, string>("redirect_uri", InstagramConfiguration.RedirectUrl ),
          new KeyValuePair<string, string>("code", code)
        };
        using (var authResponse =
          await
            httpClient.PostAsync("https://api.instagram.com/oauth/access_token", new FormUrlEncodedContent(parameters)))
        {
          var accessTokenResponseJson = await authResponse.Content.ReadAsStringAsync();
          var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(accessTokenResponseJson);
          return accessTokenResponse.AccessToken;
        }
      }
    }

    public async Task<MedialListResponse> GetPopularMedia(string token)
    {
      using (var httpClient = new HttpClient())
      {
        using (
          var response = await httpClient.GetAsync("https://api.instagram.com/v1/media/popular?access_token=" + token))
        {
          var json = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<MedialListResponse>(json);
        }
      }
    }

    public async Task<Media> GetMediaAsync(string id, string token)
    {
      using (var httpClient = new HttpClient())
      {
        using (
          var response = await httpClient.GetAsync(string.Format("https://api.instagram.com/v1/media/{0}?access_token={1}", id, token)))
        {
          var json = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<Media>(json);
        }
      }
    }

    public async Task<MediaByUserResponse> GetMediaByUserAsync(string userId, string token)
    {
      using (var httpClient = new HttpClient())
      {
        using (
          var response = await httpClient.GetAsync(string.Format("https://api.instagram.com/v1/users/{0}/media/recent/?access_token={1}", userId, token)))
        {
          var json = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<MediaByUserResponse>(json);
        }
      }
    }

    public async Task<MedialListResponse> GetMediaByTagAsync(string tag, string token)
    {
      using (var httpClient = new HttpClient())
      {
        using (
          var response = await httpClient.GetAsync(string.Format("https://api.instagram.com/v1/tags/{0}/media/recent?access_token={1}", tag, token)))
        {
          var json = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<MedialListResponse>(json);
        }
      }
    }

    public async Task<MediaByUserResponse> GetNextMediaByUserAsync(string nextUrl)
    {
      using (var httpClient = new HttpClient())
      {
        using (
          var response = await httpClient.GetAsync(nextUrl))
        {
          var json = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<MediaByUserResponse>(json);
        }
      }
    }

    public async Task<GetUsersResponse> GetUserAsync(string userName, string token)
    {
      using (var httpClient = new HttpClient())
      {
        using (
          var response = await httpClient.GetAsync(string.Format("https://api.instagram.com/v1/users/search?q={0}&access_token={1}", userName, token)))
        {
          var json = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<GetUsersResponse>(json);
        }
      }
    }


    public async Task<UserResponse> GetSelfAsync()
    {
      using (var httpClient = new HttpClient())
      {
        using (
          var response = await httpClient.GetAsync(string.Format("https://api.instagram.com/v1/users/self/?&access_token={0}", _token)))
        {
          var json = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<UserResponse>(json);
        }
      }
    }

  }
}
