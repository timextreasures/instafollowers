using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InstaFollowers.WP.Model;
using SQLite;

namespace InstaFollowers.WP.Services
{
  public interface IVideoRepository
  {
    IEnumerable<VideoDto> GetVideos();
    Task<VideoDto> SaveVideo(VideoDto video);
  }

  public class VideosRepository : IVideoRepository
  {
    public VideosRepository()
    {
      try
      {
        var connection = new SQLiteConnection(App.DbPath);
        connection.CreateTable<VideoDto>();
        connection.Close();
        connection.Dispose();
      }
      catch (Exception e)
      {
        AnalyticsService.TrackException(e, "VideosRepository");
      }
    }

    public IEnumerable<VideoDto> GetVideos()
    {
      using (var connection = new SQLiteConnection(App.DbPath))
      {
        return connection.Table<VideoDto>().ToList();
      }
    }

    public async Task<VideoDto> SaveVideo(VideoDto video)
    {
      IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

      byte[] data;
      using (var http = new HttpClient())
      {
        var response = await http.GetAsync(video.Source);
        using (response)
        {
          if (!response.IsSuccessStatusCode)
          {
            return null;
          }

          data = await response.Content.ReadAsByteArrayAsync();
        }
      }
      var fileName = video.Id + ".mp4";
      using (IsolatedStorageFileStream output = iso.CreateFile(fileName))
      {
        await output.WriteAsync(data, 0, data.Length);
        output.Flush();
      }
      return video;
    }
  }
}
