using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Windows.Storage;
using GalaSoft.MvvmLight.Ioc;
using InstaFollowers.WP.Services;
using Microsoft.Practices.ServiceLocation;

namespace InstaFollowers.WP.ViewModel
{
    public class ViewModelLocator
    {
        private static async Task Init()
        {
            StorageFile dbFile = null;
            try
            {
                // Try to get the 
                dbFile = await StorageFile.GetFileFromPathAsync(App.DbName);
            }
            catch (FileNotFoundException)
            {
                if (dbFile == null)
                {
                    // Copy file from installation folder to local folder.
                    // Obtain the virtual store for the application.
                    IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

                    // Create a stream for the file in the installation folder.
                    using (Stream input = App.GetResourceStream(new Uri(App.DbName, UriKind.Relative)).Stream)
                    {
                        // Create a stream for the new file in the local folder.
                        using (IsolatedStorageFileStream output = iso.CreateFile(App.DbName))
                        {
                            // Initialize the buffer.
                            byte[] readBuffer = new byte[4096];
                            int bytesRead = -1;

                            // Copy the file from the installation folder to the local folder. 
                            while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                            {
                                output.Write(readBuffer, 0, bytesRead);
                            }
                        }
                    }
                }
            }
        }

        static ViewModelLocator()
        {
            //Init().Wait();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IVideoRepository, VideosRepository>();
            SimpleIoc.Default.Register<IVideoService, VideoService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<VideosViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public VideosViewModel ImagesModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VideosViewModel>();
            }
        }
        public static void Cleanup()
        {

        }
    }
}