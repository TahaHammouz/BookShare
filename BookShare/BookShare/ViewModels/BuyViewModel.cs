using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Database.Query;
using BookShare.Models;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BookShare.ViewModels
{
    public class BuyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand DisplayActionSheetCommand { get; set; }

        private string _buttonImageSource = "gray";
        public string ButtonImageSource
        {
            get { return _buttonImageSource; }
            set
            {
                _buttonImageSource = value;
                OnPropertyChanged(nameof(ButtonImageSource));
            }
        }

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
            DisplayActionSheetCommand = new Command(async () => await DisplayActionSheet());
            LoadBooks();
        }
        private async Task DisplayActionSheet()
        {
            var buttons = new List<string>
            {
                "Price (High to Low)",
                "Price (Low to High)",
                "Alphabetically"
            };


            string action = await Application.Current.MainPage.DisplayActionSheet("Sort By", "Cancel", null, buttons.ToArray());
            Debug.WriteLine("Action: " + action);


            switch (action)
            {
                case "Price (High to Low)":
                    await SortByPriceHighToLow();
                    break;
                case "Price (Low to High)":
                    await SortByPriceLowToHigh();
                    break;
                case "Alphabetically":
                    await SortAlphabetically();
                    break;
            }
        }

        private async Task SortByPriceHighToLow()
        {
            // Sort items by price high to low
        }

        private async Task SortByPriceLowToHigh()
        {
            // Sort items by price low to high
        }

        private async Task SortAlphabetically()
        {
            // Sort items alphabetically
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    Dimensions = book.Object.Dimensions,
                    URL = book.Object.URL
                });
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