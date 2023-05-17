using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Firebase.Auth.Providers;
using Firebase.Auth;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BookShare.Models;
using BookShare.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Windows.Input;
using BookShare.Views;
using System.Diagnostics;
using FirebaseAdmin.Auth;

namespace BookShare.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public Command RefreshCommand { get; }



        private readonly FirebaseAuth _auth;
        public ICommand Logout { get; set; }

        private string _buttonImageSource = "logoutFrom";
        public string ButtonImageSource
        {
            get { return _buttonImageSource; }
            set
            {
                _buttonImageSource = value;
                OnPropertyChanged(nameof(ButtonImageSource));
            }
        }
        private BookShareDB firebaseDataService;
        private ObservableCollection<Book> userBooks;
        public ProfileViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            _auth = FirebaseAuth.DefaultInstance;
            Logout = new Command(async () => await DisplayActionSheet());

            try
            {
                firebaseDataService = new BookShareDB(Constants.BaseUrl);
                _ = LoadPostsAsync();
                _ = LoadUserAsync();
            }
            catch (Exception e) { e.Source = "ProfileViewModel"; }
            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await LoadUserAsync();
                IsRefreshing = false;
            });
        }
        public ICommand DeleteBookCommand { get; set; }

        private Book selectedBook;
        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                if (selectedBook != value)
                {
                    selectedBook = value;
                    OnPropertyChanged(nameof(SelectedBook));
                }
            }
        }

        private ObservableCollection<Book> userposts;
        public ObservableCollection<Book> UserPosts
        {
            get { return userposts; }
            set { userposts = value; OnPropertyChanged(nameof(UserPosts)); }
        }

        private ObservableCollection<Models.User> users;
        public ObservableCollection<Models.User> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged(nameof(Users)); }
        }

        private ObservableCollection<Book> reversedPosts;
        public ObservableCollection<Book> ReversedPosts
        {
            get { return reversedPosts; }
            set { reversedPosts = value; OnPropertyChanged(nameof(ReversedPosts)); }
        }

        public async Task DeleteAndRefresh(Book book)
        {
            await firebaseDataService.DeletePost(book);
            await LoadPostsAsync();
            FilterBooks();
            OnPropertyChanged(nameof(ReversedPosts));
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }
        public async Task LoadPostsAsync()
        {
            try
            {
                IsBusy = true;
                var books = await firebaseDataService.GetBooksAsync();
                userBooks = new ObservableCollection<Book>(books);
                FilterBooks();
            }
            catch (Exception e)
            {
                e.Source = "LoadBooksAsync";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadUserAsync()
        {
            try
            {
                var users = await firebaseDataService.GetUsersAsync();
                Users = new ObservableCollection<Models.User>(users);
                FilterUsers();
                GetUser();
            }
            catch (Exception e)
            {
                e.Source = "LoadUsersAsync";
            }
        }

        private string publisherGender;
        public string PublisherGender
        {
            get { return publisherGender; }
            set
            {
                if (publisherGender != value)
                {
                    publisherGender = value;
                    OnPropertyChanged(nameof(PublisherGender));
                    OnPropertyChanged(nameof(PublisherImageSource));
                }
            }
        }

        public string PublisherImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(PublisherGender))
                    return null;

                if (PublisherGender.Equals("Male", StringComparison.OrdinalIgnoreCase))
                    return "scholar.png";
                else if (PublisherGender.Equals("Female", StringComparison.OrdinalIgnoreCase))
                    return "profile_female.png";
                else
                    return null;
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        public void GetUser()
        {

            if (users != null && users.Count > 0)
            {
                PublisherGender = users[0].Gender;
                Username = users[0].Name;
            }

        }
        public Models.User user = new Models.User { };
        public Models.User GetUserInfo()
        {
            if (users != null && users.Count > 0)
            {
                user.Name = users[0].Name;
                user.Gender = users[0].Gender;
                user.Email = users[0].Email;
                user.Faculty = users[0].Faculty;
                user.Uid = users[0].Uid;
            }
            return user;
        }


        private async void FilterBooks()
        {
            if (userBooks != null)
            {
                string id = await SecureStorage.GetAsync("auth_token");
                var filtered = userBooks.Where(b =>
                     string.IsNullOrEmpty(id) ||
                    !string.IsNullOrEmpty(b.UserId) && b.UserId.ToLower().Equals(id.ToLower()));
                UserPosts = new ObservableCollection<Book>(filtered);
                ReversedPosts = new ObservableCollection<Book>(UserPosts.Reverse());

            }
        }

        private async void FilterUsers()
        {
            if (users != null)
            {
                string id = await SecureStorage.GetAsync("auth_token");
                var filtered = users.Where(b =>
                     string.IsNullOrEmpty(id) ||
                    !string.IsNullOrEmpty(b.Uid) && b.Uid.ToLower().Equals(id.ToLower()));
                users = new ObservableCollection<Models.User>(filtered);

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task DisplayActionSheet()
        {
            var buttons = new List<string>
            {
                "Yes",
                "No",
            };


            string action = await Application.Current.MainPage.DisplayActionSheet("Are you sure you want to log out?", "Cancel", null, buttons.ToArray());
            Debug.WriteLine("Action: " + action);


            switch (action)
            {
                case "Yes":
                    await LogoutFromApp();
                    break;
                case "No":
                    break;

            }
        }


        private async Task LogoutFromApp()
        {
            await SecureStorage.SetAsync("auth_token", "");
            await SecureStorage.SetAsync("issignin", "false");

            var loginPage = new LoginPage();
            await Application.Current.MainPage.Navigation.PushModalAsync(loginPage);
        }




    }

}