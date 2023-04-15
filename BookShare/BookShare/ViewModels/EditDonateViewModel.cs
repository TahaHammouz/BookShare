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
    internal class EditDonateViewModel:BaseViewModel
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
            SelectedBook = selectedBook;
            firebaseDataService = new BookShareDB("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
            SaveCommand = new Xamarin.Forms.Command(async () => await UpdatePost(selectedBook));

        }
        public EditDonateViewModel()
        {
            
        }
        
        public async Task UpdatePost (Book book)
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
                catch(Exception ex)
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
