using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InstaFollowers.WP.Model;
using InstaFollowers.WP.Model.Instagram;
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

    private UserViewModel _user;

    public UserViewModel User
    {
      get { return _user; }
      set { _user = value; RaisePropertyChanged(); }
    }

    public void InitializeWithUser(UserResponse userResponse)
    {
      User = new UserViewModel(userResponse.Data);
    }

    public MainViewModel(IVideoService videoService)
    {
      _videoService = videoService;
      VideosCommand = new RelayCommand(() => App.RootFrame.Navigate(new Uri("/View/VideosView.xaml?type=1", UriKind.Relative)));
      FavoriteVideosCommand = new RelayCommand(() => App.RootFrame.Navigate(new Uri("/View/VideosView.xaml?type=2", UriKind.Relative)));


      AnalyticsService.TrackMainView();
    }

  }
}