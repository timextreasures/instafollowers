using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InstaFollowers.WP.Model;
using InstaFollowers.WP.Services;

namespace InstaFollowers.WP.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private readonly IVideoService _videoService;
    public RelayCommand VideosCommand { get; private set; }
    public RelayCommand FavoriteVideosCommand { get; private set; }
    public RelayCommand<VideoViewModel> PlayCommand { get; private set; }


    private bool _isBusy;
    public bool IsBusy
    {
      get { return _isBusy; }
      set
      {
        _isBusy = value;
        RaisePropertyChanged();
      }
    }

    private int _newVideosCount;

    public int NewVideosCount
    {
      get { return _newVideosCount; }
      set { _newVideosCount = value; RaisePropertyChanged(); }
    }

    private readonly ObservableCollection<VideoViewModel> _videos = new ObservableCollection<VideoViewModel>();
    public ObservableCollection<VideoViewModel> Videos { get { return _videos; } }

    public MainViewModel(IVideoService videoService)
    {
      _videoService = videoService;
      SetNewImagesCount();
      VideosCommand = new RelayCommand(() => App.RootFrame.Navigate(new Uri("/View/VideosView.xaml?type=1", UriKind.Relative)));
      FavoriteVideosCommand = new RelayCommand(() => App.RootFrame.Navigate(new Uri("/View/VideosView.xaml?type=2", UriKind.Relative)));
      PlayCommand = new RelayCommand<VideoViewModel>(PlayExecute);

      AnalyticsService.TrackMainView();
    }

    private void PlayExecute(VideoViewModel video)
    {
      var playingVideo = Videos.SingleOrDefault(i => i.IsPlaying);
      if (playingVideo != null)
      {
        playingVideo.IsPlaying = false;
      }
    
      video.IsPlaying = true;
    }

    private void SetNewImagesCount()
    {
      IsBusy = true;
      _videoService.GetVideos(1, async (videos, exception) =>
      {
        var videosResult = await videos;
        Videos.Clear();
        foreach (var video in videosResult)
        {
          Videos.Add(video);
        }
        IsBusy = false;
      });

      _videoService.GetNewCount((count) =>
     {
       NewVideosCount = count;
     });
    }
  }
}