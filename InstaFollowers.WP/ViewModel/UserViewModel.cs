using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using InstaFollowers.WP.Model.Instagram;

namespace InstaFollowers.WP.ViewModel
{
  public class UserViewModel : ObservableObject
  {
    public string Id { get; set; }

    private string _userName;
    public string UserName
    {
      get { return _userName; }
      set { _userName = value; RaisePropertyChanged(); }
    }

    private string _fullName;
    public string FullName
    {
      get { return _fullName; }
      set { _fullName = value; RaisePropertyChanged(); }
    }

    private string _profilePicture;
    public string ProfilePicture
    {
      get { return _profilePicture; }
      set { _profilePicture = value; RaisePropertyChanged(); }
    }

    private int _media;
    public int Media
    {
      get { return _media; }
      set { _media = value; RaisePropertyChanged(); }
    }

    private int _follows;
    public int Follows
    {
      get { return _follows; }
      set { _follows = value; RaisePropertyChanged(); }
    }

    private int _followedBy;
    public int FollowedBy
    {
      get { return _followedBy; }
      set { _followedBy = value; RaisePropertyChanged(); }
    }

    public UserViewModel()
    {
    }

    public UserViewModel(InstaUser user)
    {
      Id = user.Id;
      UserName = user.UserName;
      FullName = user.FullName;
      ProfilePicture = user.ProfilePicture;
      Media = user.Counts.Media;
      Follows = user.Counts.Follows;
      FollowedBy = user.Counts.FollowedBy;
    }
  }
}
