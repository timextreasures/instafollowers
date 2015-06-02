using System;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows.Interop;
using GoogleAds;
using HtmlAgilityPack;
using InstaFollowers.WP.Model.Instagram;
using InstaFollowers.WP.Services;
using InstaFollowers.WP.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;

namespace InstaFollowers.WP
{
  public partial class MainPage : PhoneApplicationPage
  {
    public MainPage()
    {
      InitializeComponent();
      Loaded += MainPage_Loaded;
    }

    private MainViewModel VM { get { return DataContext as MainViewModel; } }

    async void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
      if (IsolatedStorageSettings.ApplicationSettings.Contains("token"))
      {
        await InitializeProfile();
      }
      else
      {
        StartAuth();
      }
    }

    private async Task InitializeProfile()
    {
      VM.IsBusy = true;
      var token = IsolatedStorageSettings.ApplicationSettings["token"].ToString();
      var insta = new InstaClient(token);
      var self = await insta.GetSelfAsync();
      if (self == null || self.Meta.Code != 200)
      {
        StartAuth();
      }
      else
      {
        VM.InitializeWithUser(self);
      }
      VM.IsBusy = false;
    }

    private void StartAuth()
    {
      App.RootFrame.Navigate(new Uri("/View/LoginView.xaml", UriKind.Relative));
    }

    private void OnAdReceived(object sender, AdEventArgs e)
    {
      AnalyticsService.TrackAdMobStatus("Received ad successfully");
    }

    private void OnFailedToReceiveAd(object sender, AdErrorEventArgs errorCode)
    {
      AnalyticsService.TrackAdMobStatus("Failed to receive ad with error " + errorCode.ErrorCode);
    }
  }
}