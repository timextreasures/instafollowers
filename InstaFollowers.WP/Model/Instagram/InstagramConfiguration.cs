using System;

namespace InstaFollowers.WP.Model.Instagram
{
  public class InstagramConfiguration
  {
    public const string Host = "http://localhost:90";
    public const string AuthUrl = Host + "/instagramauth";

    public const string ClientId = "741faf8713474a7fabf5026a4035a181";

    public static string ClientSecret
    {
      get { return "8c8d7b6e961749419e6b6bf8636846ea"; }
    }

    public const string Token = "1580338596.741faf8.eaf16e98db924280bf69d19619946a58";

    public static string RedirectUrl
    {
      get { return Host + "/instagramauth/oauth"; }
    }

    public static int LikesDeadLine { get { return 50000; } }


    public static DateTime DeadLine
    {
      get { return DateTime.Now.AddDays(-6); }
    }

    public const int PageCount = 11;
  }
}
