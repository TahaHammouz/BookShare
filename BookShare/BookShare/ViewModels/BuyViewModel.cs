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
using Xamarin.Essentials;
using MvvmHelpers;

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

        private ObservableCollection<Order> _orderList;
        public ObservableCollection<Order> OrderList
        {
            get { return _orderList; }
            set
            {
                _orderList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderList)));
            }
        }

        public BuyViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
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
                    SortOrdersByPriceHighToLow();
                    break;
                case "Price (Low to High)":
                    SortOrdersByPriceLowToHigh();
                    break;
                case "Alphabetically":
                    SortOrdersAlphabetically();
                    break;
            }
        }

        private void SortOrdersByPriceHighToLow()
        {
            OrderList = new ObservableCollection<Order>(OrderList.OrderByDescending(book => book.BookPrice));
            FilterBooks();


        }


        private void SortOrdersByPriceLowToHigh()
        {
            OrderList = new ObservableCollection<Order>(OrderList.OrderBy(book => book.BookPrice));
            FilterBooks();
        }

        private void SortOrdersAlphabetically()
        {
            OrderList = new ObservableCollection<Order>(OrderList.OrderBy(book => book.Bookname));
            FilterBooks();
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
            var firebaseClient = new FirebaseClient(Constants.BaseUrl);
            var orders = await firebaseClient.Child("List").OnceAsync<Order>();
            OrderList = new ObservableCollection<Order>();
            foreach (var book in orders)
            {
                OrderList.Add(new Order
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

            foreach (var book in OrderList)
            {
                book.ShortenedName = ShortenText(book.Bookname);
            }

            FilterBooks();
        }

        private string ShortenText(string text)
        {
            if (text.Length > MaxTextLength)
            {
                return $"{text.Substring(0, MaxTextLength)}...";
            }

            return text;
        }
        private ObservableCollection<Order> filteredBooks;
        public ObservableCollection<Order> FilteredBooks
        {
            get { return filteredBooks; }
            set { filteredBooks = value; OnPropertyChanged(); }
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

        private void FilterBooks()
        {
            if (OrderList != null)
            {
                var filtered = OrderList.Where(b =>
                    string.IsNullOrEmpty(filterText) ||
                    b.Bookname.ToLower().Contains(filterText.ToLower()) ||
                    b.BookPrice.Contains(filterText)
                    );

                FilteredBooks = new ObservableCollection<Order>(filtered);
            }
        }

    }
}

