using System;
using System.ComponentModel;
using System.Windows.Input;
using BookShare.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookShare.ViewModels
{
    public class ContactViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public ICommand CopyCommand { get; }

        private string genderImageSource;
        public string GenderImageSource
        {
            get { return genderImageSource; }
            set
            {
                if (genderImageSource != value)
                {
                    genderImageSource = value;
                    OnPropertyChanged(nameof(GenderImageSource));
                }
            }
        }

        public ContactViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
        }
        public void contact()
        {
            if (selectedBook.Contactlink == "WhatsApp")
            {
                selectedBook.ContactIcon = "WhatsApp.png";
            }
            if (selectedBook.Contactlink == "Facebook")
            {
                selectedBook.ContactIcon = "facebook_icon.png";
            }
            if (selectedBook.Contactlink == "LinkedIn")
            {
                selectedBook.ContactIcon = "linkedin.png";
            }

        }
        public void publisherGender()
        {
            if (selectedBook.PublisherGender == "Male")
            {
                selectedBook.ProfileIcon = "male.png";
            }
            if (selectedBook.PublisherGender == "Female")
            {
                selectedBook.ProfileIcon = "female.png";
            }
        }


        public ContactViewModel(Book selectedBook)
        {
            SelectedBook = selectedBook;

            contact();
            publisherGender();
            CopyCommand = new Command(CopyButton_Clicked);
        }
        private void CopyButton_Clicked(object parameter)
        {
            if (parameter is Label contactMethodLabel)
            {
                string textToCopy = contactMethodLabel.Text;
                Clipboard.SetTextAsync(textToCopy);
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}