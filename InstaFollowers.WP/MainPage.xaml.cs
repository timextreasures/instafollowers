using GoogleAds;
using InstaFollowers.WP.Services;
using Microsoft.Phone.Controls;

namespace InstaFollowers.WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
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