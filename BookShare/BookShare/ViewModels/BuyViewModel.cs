using System.Collections.ObjectModel;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Database.Query;
using BookShare.Models;
using System.ComponentModel;

namespace BookShare.ViewModels
{
    public class BuyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Order> _bookList;
        public ObservableCollection<Order> BookList
        {
            get { return _bookList; }
            set
            {
                _bookList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BookList)));
            }
        }

        public BuyViewModel()
        {
            LoadBooks();
        }

        private async void LoadBooks()
        {
            var firebaseClient = new FirebaseClient("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
            var orders = await firebaseClient.Child("List").OnceAsync<Order>();

            BookList = new ObservableCollection<Order>();
            foreach (var book in orders)
            {
                BookList.Add(new Order
                {
                    Bookname = book.Object.Bookname,
                    BookPrice = book.Object.BookPrice,
                    Description = book.Object.Description,
                    Publisher = book.Object.Publisher,
                    Language = book.Object.Language,
                    NumberOfPages = book.Object.NumberOfPages,
                    Dimensions = book.Object.Dimensions
                });

            }
        }
    }
}