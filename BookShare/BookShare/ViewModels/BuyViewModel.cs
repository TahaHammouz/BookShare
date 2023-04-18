using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Database.Query;
using BookShare.Models;

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

        private int _maxTextLength = 13;
        public int MaxTextLength
        {
            get { return _maxTextLength; }
            set
            {
                _maxTextLength = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxTextLength)));
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
                BookList.Add(new Order { Bookname = book.Object.Bookname, BookPrice = book.Object.BookPrice, URL = book.Object.URL });
            }

            foreach (var book in BookList)
            {
                book.ShortenedName = ShortenText(book.Bookname);
            }
        }

        private string ShortenText(string text)
        {
            if (text.Length > MaxTextLength)
            {
                return $"{text.Substring(0, MaxTextLength)}...";
            }

            return text;
        }
    }
}
