using System;

namespace InstaFollowers.WP.Services
{
  public class AnalyticsService
  {
    public static void StartTrackViews(string viewName)
    {
      GoogleAnalytics.EasyTracker.GetTracker().SendView(viewName);
    }

    public static void TrackException(Exception exception, string mehodName)
    {
      GoogleAnalytics.EasyTracker.GetTracker().SendException(mehodName + " / " + exception.Message + " / " + exception.StackTrace, true);
    }

    public static void TrackAdMobStatus(string status)
    {
      GoogleAnalytics.EasyTracker.GetTracker().SendException(status, false);
    }

    public static void TrackMainView()
    {
      GoogleAnalytics.EasyTracker.GetTracker().SendView("Main");
    }
  }
}
