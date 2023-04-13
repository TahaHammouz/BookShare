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

namespace BookShare.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private BookShareDB firebaseDataService;
        private ObservableCollection<Book> userBooks;
        public ProfileViewModel()
        {

            try
            {
                firebaseDataService = new BookShareDB("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
                _ = LoadPostsAsync();
                //DeleteBookCommand = new Command(async () => await DeleteAndRefresh());
            }
            catch (Exception e) { e.Source = "ProfileViewModel"; }
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
            OnPropertyChanged(nameof(UserPosts));
        }

        public async Task LoadPostsAsync()
        {
            try
            {
                var books = await firebaseDataService.GetBooksAsync();
                userBooks = new ObservableCollection<Book>(books);
                FilterBooks();
            }
            catch (Exception e)
            {
                e.Source = "LoadBooksAsync";
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

                if (UserPosts != null && UserPosts.Count > 0)
                {
                    PublisherGender = UserPosts[0].PublisherGender;
                    Username = UserPosts[0].Username;
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
