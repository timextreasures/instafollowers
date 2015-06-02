using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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

    private int _isBusy;
    public bool IsBusy
    {
      get { return _isBusy != 0; }
      set
      {
        if (value)
        {
          Interlocked.Increment(ref _isBusy);
        }
        else
        {
          Interlocked.Decrement(ref _isBusy);
          
        }

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