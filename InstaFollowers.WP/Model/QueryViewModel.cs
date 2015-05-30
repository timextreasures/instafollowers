using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLite;

namespace InstaFollowers.WP.Model
{
  //ObservableObject
  public class QueryViewModel
  {
    public List<VideoDto> Videos { get; set; }

    public bool NextAvailable { get; set; }
  }

  public class VideoDto
  {
    [PrimaryKey]
    public string Id { get; set; }
    public string Source { get; set; }
    public string Poster { get; set; }

    public VideoViewModel ToViewModel()
    {
      return new VideoViewModel
      {
        Id = Id,
        Poster = Poster,
      };
    }
  }

  public class VideoViewModel : ObservableObject
  {
    public string Id { get; set; }
    public string Poster { get; set; }

    private MediaElement _meidaControl;

    private MediaElementState _state;

    public string VideoSource
    {
      get
      {
        //IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
        //using (var file = iso.OpenFile(Id + ".mp4", FileMode.Open))
        //{
        //  return file.Name;
        //}
        return Path.Combine(ApplicationData.Current.LocalFolder.Path, Id + ".mp4");
      }
    }

    private string _source;

    public string Source { get { return _source; } set { _source = value; RaisePropertyChanged(); } }

    private bool _isPlaying;
    public bool IsPlaying
    {
      get { return _isPlaying; }
      set
      {
        _isPlaying = value;
        if (_isPlaying)
        {
          _meidaControl.Play();
        }
        else
        {
          _meidaControl.Stop();
        }
        RaisePropertyChanged();
      }
    }

    public VideoViewModel()
    {
      StateChangedCommand = new RelayCommand<RoutedEventArgs>((eventArgs) =>
      {
        if (_meidaControl != null)
          return;

        var media = eventArgs.OriginalSource as MediaElement;
        if (media == null)
        {
          return;
        }

        _meidaControl = media;


        //var state = _meidaControl.CurrentState;
        //if (_state != state)
        //{
        //  _state = state;
        //  switch (_state)
        //  {
        //    case MediaElementState.Stopped:
        //    case MediaElementState.Paused:
        //      IsPlaying = false;
        //      break;

        //  }
        //}
      });
    }

    public RelayCommand<RoutedEventArgs> StateChangedCommand { get; private set; }
    public RelayCommand PauseCommand { get; private set; }
  }
}
