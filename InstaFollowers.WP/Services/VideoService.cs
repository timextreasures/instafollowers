using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InstaFollowers.WP.Model;
using Newtonsoft.Json;

namespace InstaFollowers.WP.Services
{
  public interface IVideoService
  {
    void GetVideos(int page, Action<Task<List<VideoViewModel>>, Exception> callback);
    void GetNewCount(Action<int> callback);
  }

  public class VideoService : IVideoService
  {
    private readonly IVideoRepository _videoRepository;

    public VideoService(IVideoRepository videoRepository)
    {
      _videoRepository = videoRepository;
    }


    public void GetNewCount(Action<int> callback)
    {
      callback(1);
    }

    public void GetVideos(int page, Action<Task<List<VideoViewModel>>, Exception> callback)
    {
      callback(GetVideos(page), null);
    }

    private async Task<List<VideoViewModel>> GetVideos(int page)
    {
      using (var http = new HttpClient())
      {

        var response = await http.GetAsync("http://localhost:90/api/mobile/query/" + page);
        using (response)
        {
          var json = await response.Content.ReadAsStringAsync();
          var videos = JsonConvert.DeserializeObject<QueryViewModel>(json);
          List<VideoViewModel> videoViewModels = new List<VideoViewModel>();
          List<Task> tasks = new List<Task>();
          foreach (var video in videos.Videos)
          {
            var thisVideo = video;
            var result = _videoRepository.SaveVideo(thisVideo);
            result.ContinueWith(i =>
            {
              if (i.IsCompleted)
              {
                videoViewModels.Add(i.Result.ToViewModel());
              }
            });
            tasks.Add(result);
          }

          await Task.WhenAll(tasks);

          return videoViewModels;
        }
      }
    }
  }
}
