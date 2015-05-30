using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook;
using Facebook.Client;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InstaFollowers.WP.Model;
using InstaFollowers.WP.Services;
using VK.WindowsPhone.SDK;
using VK.WindowsPhone.SDK.Pages;

namespace InstaFollowers.WP.ViewModel
{
  public class VideosViewModel : ViewModelBase
  {
    private bool _isVkAuthentificated;
    private bool _isVkSharing;
    private int _previousImageIndex;
    private const int PreloadIndex = 4;
    private readonly IVideoService _videoService;
    private readonly IVideoRepository _videoRepository;
    public event EventHandler ImageChanged;


    private bool _isBusy;
    public bool IsBusy
    {
      get { return _isBusy; }
      set
      {
        _isBusy = value;
        RaisePropertyChanged("IsBusy");
      }
    }

    private int _currentImageIndex;
    public int CurrentImageIndex
    {
      get { return _currentImageIndex; }
      set
      {
        if (_previousImageIndex == 0 && value != 1)
          value = 0;

        _currentImageIndex = ChechImagesOverflow(value);
        if (value == -1)
          return;

        RaisePropertyChanged("CurrentImageIndex");
        if (ImageChanged != null)
          ImageChanged(this, new EventArgs());
      }
    }

    private int ChechImagesOverflow(int index)
    {
      if (index <= 30) return index;

      var firstImages = Videos.Take(10).ToList();
      foreach (var firstImage in firstImages)
      {
        Videos.Remove(firstImage);
      }
      return (index - 10);
    }

    private VideoDto _currentImage;

    public VideoDto CurrentImage
    {
      get
      {
        return _currentImage;
      }
      set
      {
        _currentImage = value;
        RaisePropertyChanged("CurrentImage");
        LikeImageCommand.RaiseCanExecuteChanged();
        DisikeImageCommand.RaiseCanExecuteChanged();
      }
    }

    public RelayCommand BackCommand { get; private set; }
    public RelayCommand NavigateToImageCommand { get; private set; }
    public RelayCommand SaveImageCommand { get; private set; }
    public RelayCommand LikeImageCommand { get; private set; }
    public RelayCommand DisikeImageCommand { get; private set; }


    private readonly ObservableCollection<VideoDto> _videos = new ObservableCollection<VideoDto>();
    public ObservableCollection<VideoDto> Videos { get { return _videos; } }

    public VideosViewModel(IVideoService videoService, IVideoRepository videoRepository)
    {
      _videoService = videoService;
      _videoRepository = videoRepository;
      BackCommand = new RelayCommand(() => App.RootFrame.GoBack());
      App.RootFrame.Navigated += RootFrame_Navigated;
    }

    void RootFrame_Navigated(object sender, NavigationEventArgs e)
    {
      string type = string.Empty;
      var page = e.Content as Page;
      page.NavigationContext.QueryString.TryGetValue("type", out type);

      Initialize();
    }


    public async void ShareToVk()
    {
      if (!_isVkAuthentificated)
      {
        VKSDK.Authorize(new List<string> { VKScope.WALL, VKScope.PHOTOS, });
        return;
      }
      var image = Videos[CurrentImageIndex];
      using (var client = new HttpClient())
      {
        _isVkSharing = true;
        var stream = await client.GetAsync("", HttpCompletionOption.ResponseContentRead);
        var inputData = new VKPublishInputData
        {
          Text = "*",
          Image = await stream.Content.ReadAsStreamAsync(),
          ExternalLink = new VKPublishInputData.VKLink
          {
            Title = "Mp.Mobi",
            Subtitle = "Миллион приколов",
            Uri = "http://vk.com/onemp"
          }
        };

        VKSDK.Publish(inputData);
      }
    }

    public async void FacebookShare()
    {
      var facebookSessionClient = new FacebookSessionClient(App.FB_APP_ID);
      string accessToken = string.Empty;
      try
      {
        var session = await facebookSessionClient.LoginAsync("user_about_me,publish_actions");
        accessToken = session.AccessToken;
      }
      catch (InvalidOperationException e)
      {
      }
      if (string.IsNullOrEmpty(accessToken))
        return;

      var facebookClient = new FacebookClient(accessToken);
      var image = Videos[CurrentImageIndex];
      var postParams = new
      {
        name = "1mp.mobi - Миллион приколов",
        caption = string.Empty,
        description = string.Empty,
        link = "http://1mp.mobi/",
        picture = ""
      };

      try
      {
        dynamic fbPostTaskResult = await facebookClient.PostTaskAsync("/me/feed", postParams);
        var result = (IDictionary<string, object>)fbPostTaskResult;

      }
      catch (Exception ex)
      {
      }
    }

    public void Initialize()
    {
      IsBusy = true;
      //_videoService.GetVideos(1, async (videos, exception) =>
      //{
      //  var videosResult = await videos;
      //  Videos.Clear();
      //  foreach (var video in videosResult.Videos)
      //  {
      //    Videos.Add(video);
      //  }
      //  IsBusy = false;
      //});


      VKSDK.Initialize(App.VK_APP_ID);

      VKSDK.AccessTokenReceived += (sender, args) =>
      {
        _isVkAuthentificated = true;
        //опять идем шарить текущую картинку
        ShareToVk();
      };
    }

    public override void Cleanup()
    {
      Videos.Clear();
      //App.RootFrame.Navigated -= RootFrame_Navigated;
      base.Cleanup();
    }
  }
}
