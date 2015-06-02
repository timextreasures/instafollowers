using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using HtmlAgilityPack;
using InstaFollowers.WP.Model.Instagram;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace InstaFollowers.WP.View
{
  public partial class LoginView : PhoneApplicationPage
  {
    public LoginView()
    {
      InitializeComponent();
      Loaded += LoginView_Loaded;
      Unloaded += LoginView_Unloaded;
    }

    async void LoginView_Loaded(object sender, RoutedEventArgs e)
    {
      await Browser.ClearCookiesAsync();
      await Browser.ClearInternetCacheAsync();
      var url =
        string.Format(
          "https://api.instagram.com/oauth/authorize?client_id={0}&redirect_uri={1}&response_type=code&scope=likes+comments",
          InstagramConfiguration.ClientId,
          InstagramConfiguration.RedirectUrl);
      Browser.Navigate(new Uri(url));
      Browser.Navigated += Browser_Navigated;
    }

    private void Browser_Navigated(object sender, NavigationEventArgs e)
    {
      var url = e.Uri.ToString().ToLower();

      if (url.StartsWith(InstagramConfiguration.AuthUrl))
      {
        var html = Browser.SaveToString();

        HtmlDocument dom = new HtmlDocument();
        dom.LoadHtml(html);

        HtmlNode tokenElement = dom.GetElementbyId("token");
        var token = tokenElement.Attributes["value"].Value;
        if (string.IsNullOrEmpty(token))
        {
          return;
        }

        IsolatedStorageSettings.ApplicationSettings.Add("token", token);
        IsolatedStorageSettings.ApplicationSettings.Save();
        InitializeProfile();
      }
    }

    private void InitializeProfile()
    {
      App.RootFrame.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
    }

    void LoginView_Unloaded(object sender, RoutedEventArgs e)
    {
      Browser.Navigated -= Browser_Navigated;
      Loaded -= LoginView_Loaded;
      Unloaded -= LoginView_Unloaded;
    }
  }
}