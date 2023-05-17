using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BookShare.Models;
using BookShare.Services;
using Xamarin.Forms;

namespace BookShare.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private BookShareDB firebaseDataService;
        private ObservableCollection<Book> allBooks;

        public SearchViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            try
            {
                firebaseDataService = new BookShareDB(Constants.BaseUrl);
                IsLoading = true;
                _ = LoadBooksAsync();
            }
            catch (Exception e)
            {
                e.Source = "SearchViewModel";
            }

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await LoadBooksAsync();
                IsRefreshing = false;
            });
        }

        private ObservableCollection<Book> books;
        public ObservableCollection<Book> Books
        {
            get { return books; }
            set { books = value; OnPropertyChanged(); }
        }

        private string filterText = "";
        public string FilterText
        {
            get { return filterText; }
            set
            {
                if (filterText != value)
                {
                    filterText = value;
                    OnPropertyChanged();
                    FilterBooks();
                }
            }
        }

        private ObservableCollection<Book> filteredBooks;
        public ObservableCollection<Book> FilteredBooks
        {
            get { return filteredBooks; }
            set { filteredBooks = value; OnPropertyChanged(); }
        }


        private ObservableCollection<Book> reverseBooks;
        public ObservableCollection<Book> ReverseBooks
        {
            get { return reverseBooks; }
            set { reverseBooks = value; OnPropertyChanged(); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public Command RefreshCommand { get; }

        private async Task LoadBooksAsync()
        {
            try
            {
                var books = await firebaseDataService.GetBooksAsync();
                allBooks = new ObservableCollection<Book>(books);
                FilterBooks();
                IsLoading = false;
            }
            catch (Exception e)
            {
                e.Source = "LoadBooksAsync";
            }
        }


        private void FilterBooks()
        {
            if (allBooks != null)
            {
                var filtered = allBooks.Where(b =>
                    string.IsNullOrEmpty(filterText) ||
                    b.Bookname.ToLower().Contains(filterText.ToLower()) ||
                    b.Details.ToLower().Contains(filterText.ToLower()));
                FilteredBooks = new ObservableCollection<Book>(filtered);
                ReverseBooks = new ObservableCollection<Book>(FilteredBooks.Reverse());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
