using System;
using System.ComponentModel;
using BookShare.Models;
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
            if (selectedBook.Contactlink == "linkedin")
            {
                selectedBook.ContactIcon = "linkedin_icon.png";
            }

        }
        public void publisherGender()
        {
            if (selectedBook.PublisherGender == "Male")
            {
                selectedBook.ProfileIcon = "male.png";
            }
            if(selectedBook.PublisherGender =="Female")
            {
                selectedBook.ProfileIcon = "female.png";
            }
        }


        public ContactViewModel(Book selectedBook)
        {
            SelectedBook = selectedBook;
          
            contact(); 
            publisherGender();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}