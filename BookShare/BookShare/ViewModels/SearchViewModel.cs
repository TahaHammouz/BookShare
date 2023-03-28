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

            try
            {
                firebaseDataService = new BookShareDB("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
                _ = LoadBooksAsync();
            }
            catch (Exception e) { e.Source = "SearchViewModel"; }
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

        private async Task LoadBooksAsync()
        {
            try
            {
                var books = await firebaseDataService.GetBooksAsync();
                allBooks = new ObservableCollection<Book>(books);
                FilterBooks();
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
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
