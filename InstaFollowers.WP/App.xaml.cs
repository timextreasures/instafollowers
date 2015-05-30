using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Windows.Storage;
using GalaSoft.MvvmLight.Threading;
using InstaFollowers.WP.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace InstaFollowers.WP
{
  public partial class App : Application
  {
    public const string DbName = "viewek.sqlite";
    public const string VK_APP_ID = "4583699";
    public const string FB_APP_ID = "1565353623695838";

    public static string DbPath
    {
      get
      {
        return Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
      }
    }

    public static PhoneApplicationFrame RootFrame { get; private set; }

    public App()
    {
      UnhandledException += Application_UnhandledException;

      DispatcherHelper.Initialize();

      InitializeComponent();

      InitializePhoneApplication();

      InitializeLanguage();

      if (Debugger.IsAttached)
      {
        Application.Current.Host.Settings.EnableFrameRateCounter = true;
      }

    }

    private async void Application_Launching(object sender, LaunchingEventArgs e)
    {
      StorageFile dbFile = null;
      try
      {
        dbFile = await StorageFile.GetFileFromPathAsync(DbName);
      }
      catch (FileNotFoundException)
      {
        if (dbFile == null)
        {
          IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

          using (Stream input = GetResourceStream(new Uri(DbName, UriKind.Relative)).Stream)
          {
            using (IsolatedStorageFileStream output = iso.CreateFile(DbName))
            {
              byte[] readBuffer = new byte[4096];
              int bytesRead = -1;

              while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
              {
                output.Write(readBuffer, 0, bytesRead);
              }
            }
          }
        }
      }
    }

    private void Application_Activated(object sender, ActivatedEventArgs e)
    {
    }

    private void Application_Deactivated(object sender, DeactivatedEventArgs e)
    {
    }

    private void Application_Closing(object sender, ClosingEventArgs e)
    {
    }

    private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      if (Debugger.IsAttached)
      {
        Debugger.Break();
      }
    }

    private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
    {
      if (Debugger.IsAttached)
      {
        Debugger.Break();
      }
    }

    #region Phone application initialization

    private bool phoneApplicationInitialized = false;

    private void InitializePhoneApplication()
    {
      if (phoneApplicationInitialized)
        return;

      RootFrame = new PhoneApplicationFrame();
      RootFrame.Navigated += CompleteInitializePhoneApplication;

      RootFrame.NavigationFailed += RootFrame_NavigationFailed;

      RootFrame.Navigated += CheckForResetNavigation;

      phoneApplicationInitialized = true;
    }

    private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
    {
      if (RootVisual != RootFrame)
        RootVisual = RootFrame;

      RootFrame.Navigated -= CompleteInitializePhoneApplication;
    }

    private void CheckForResetNavigation(object sender, NavigationEventArgs e)
    {
      if (e.NavigationMode == NavigationMode.Reset)
        RootFrame.Navigated += ClearBackStackAfterReset;
    }

    private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
    {
      RootFrame.Navigated -= ClearBackStackAfterReset;

      if (e.NavigationMode != NavigationMode.New &&
          e.NavigationMode != NavigationMode.Refresh)
        return;

      while (RootFrame.RemoveBackEntry() != null)
      {
        ; // do nothing
      }
    }

    #endregion

    private void InitializeLanguage()
    {
      try
      {
        RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

        var flow =
            (FlowDirection)
            Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
        RootFrame.FlowDirection = flow;
      }
      catch
      {
        if (Debugger.IsAttached)
        {
          Debugger.Break();
        }

        throw;
      }
    }
  }
}