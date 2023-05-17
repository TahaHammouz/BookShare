using BookShare.Models;
using BookShare.Services;
using BookShare.Views;
using Firebase.Auth;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookShare.ViewModels
{
    internal class EditDonateViewModel : BaseViewModel
    {
        private BookShareDB firebaseDataService;
        public ICommand SaveCommand { get; }
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

        public EditDonateViewModel(Book selectedBook)
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            SelectedBook = selectedBook;
            firebaseDataService = new BookShareDB(Constants.BaseUrl);
            SaveCommand = new Xamarin.Forms.Command(async () => await UpdatePost(selectedBook));

        }
        public EditDonateViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
        }

        public async Task UpdatePost(Book book)
        {
            if ((selectedBook.Equals(book)))
            {
                try
                {
                    await firebaseDataService.UpdateBook(selectedBook);
                    await App.Current.MainPage.DisplayAlert("Success", "Book Edited Successfully!", "OK");
                    MessagingCenter.Send(this, "NavigateToNewPage");
                    OnPropertyChanged(nameof(Book));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Editing Failed: {ex.Message}");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failed", "Book didnt modified!", "OK");
            }
        }

    }
}
