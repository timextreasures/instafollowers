using System;
using System.Windows.Navigation;
using InstaFollowers.WP.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace InstaFollowers.WP.View
{
    public sealed partial class VideosView : PhoneApplicationPage
    {
      public VideosView()
        {
            this.InitializeComponent();
            ViewModel.ImageChanged += ViewModel_ImageChanged;
        }

        private ApplicationBarIconButton _likeButton;
        private ApplicationBarIconButton _saveButton;
        private ApplicationBarIconButton _vkButton;
        private ApplicationBarIconButton _fbButton;

        public VideosViewModel ViewModel
        {
            get { return DataContext as VideosViewModel; }
        }

        private void BuildAppBar()
        {
            ApplicationBar = new ApplicationBar() { IsVisible = true, IsMenuEnabled = true };
            _saveButton = new ApplicationBarIconButton(new Uri("/Assets/AppBarIcons/appbar.save.rest.png", UriKind.Relative));
            _saveButton.Text = "сохранить";
            _saveButton.Click += (sender, args) =>
            {
                ViewModel.SaveImageCommand.Execute(null);
                var button = sender as ApplicationBarIconButton;
                if (button == null)
                    return;

                button.IconUri = new Uri("/Assets/AppBarIcons/check.png", UriKind.Relative);
            };
            ApplicationBar.Buttons.Add(_saveButton);

           

            _likeButton = new ApplicationBarIconButton();
            _likeButton.Text = "улыбнуло";
            _likeButton.Click += LikedClick;
            SwitchLikeButton(ViewModel.CurrentImage != null);
            ApplicationBar.Buttons.Add(_likeButton);

            _fbButton = new ApplicationBarIconButton(new Uri("/Assets/AppBarIcons/facebook_icon.png", UriKind.Relative))
            {
                Text = "поделиться"
            };
            _fbButton.Click += (s, e) => ViewModel.FacebookShare();
            ApplicationBar.Buttons.Add(_fbButton);

            _vkButton = new ApplicationBarIconButton(new Uri("/Assets/AppBarIcons/vk_icon.png", UriKind.Relative))
            {
                Text = "на стену"
            };
            _vkButton.Click += (s, e) => ViewModel.ShareToVk();
            ApplicationBar.Buttons.Add(_vkButton);
        }

        void LikedClick(object sender, EventArgs e)
        {
            var button = sender as ApplicationBarIconButton;
            if (button == null || ViewModel.CurrentImage == null)
                return;

            //if (ViewModel.CurrentImage.LikedByMe)
            //    ViewModel.DisikeImageCommand.Execute(null);
            //else
            //    ViewModel.LikeImageCommand.Execute(null);

            //SwitchLikeButton(ViewModel.CurrentImage != null && ViewModel.CurrentImage.LikedByMe);
        }

        void ViewModel_ImageChanged(object sender, EventArgs e)
        {
            if (ViewModel.Videos.Count < ViewModel.CurrentImageIndex)
                return;
            var currentImage = ViewModel.Videos[ViewModel.CurrentImageIndex];
            if (ViewModel.CurrentImage == null || _likeButton == null)
                return;

            //SwitchLikeButton(ViewModel.CurrentImage != null && ViewModel.CurrentImage.LikedByMe);
            if (_saveButton != null)
            {
                _saveButton.IconUri = new Uri("/Assets/AppBarIcons/appbar.save.rest.png", UriKind.Relative);
            }
        }

        private void SwitchLikeButton(bool likedByMe)
        {
            _likeButton.IconUri = likedByMe ? new Uri("/Assets/AppBarIcons/Laugh.png", UriKind.Relative) : new Uri("/Assets/AppBarIcons/Sad.png", UriKind.Relative);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BuildAppBar();
        }
    }
}
