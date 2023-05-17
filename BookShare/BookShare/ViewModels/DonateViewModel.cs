using BookShare.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Auth;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using System;
using BookShare.Models;

namespace BookShare.ViewModels
{
    public class DonateViewModel : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand Donate { set; get; }
        public Models.Book BindingBook { set; get; }
        private string _booknameText;
        public string BooknameText
        {
            get => _booknameText;
            set
            {
                if (_booknameText != value)
                {
                    _booknameText = value;
                    OnPropertyChanged(nameof(BooknameText));
                }
            }
        }
        private string _bookname;

        public string Bookname
        {
            get { return _bookname; }
            set
            {
                if (_bookname != value)
                {
                    _bookname = value;
                    OnPropertyChanged(nameof(Bookname));
                }
            }
        }

        private string _details;
        public string Details
        {
            get => _details;
            set
            {
                if (_details != value)
                {
                    _details = value;
                    OnPropertyChanged(nameof(Details));
                }
            }
        }
        private string _detailsText;
        public string DetailsText
        {
            get => _detailsText;
            set
            {
                if (_detailsText != value)
                {
                    _detailsText = value;
                    OnPropertyChanged(nameof(DetailsText));
                }
            }
        }

        public bool IsStatusEmpty => string.IsNullOrEmpty(BindingBook.Status);


        private string _publisherGender;
        public string PublisherGender
        {
            get { return _publisherGender; }
            set
            {
                if (_publisherGender != value)
                {
                    _publisherGender = value;
                    OnPropertyChanged(nameof(PublisherGender));
                }
            }
        }
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }



        private bool _isContactInputVisible;
        public bool IsContactInputVisible
        {
            get { return _isContactInputVisible; }
            set
            {
                _isContactInputVisible = value;
                OnPropertyChanged(nameof(IsContactInputVisible));
            }
        }
        private void OnSelectedContactMethodChanged()
        {
            IsContactInputVisible = !string.IsNullOrEmpty(SelectedContactMethod);
        }

        private string _selectedContactMethod;
        public string SelectedContactMethod
        {
            get { return _selectedContactMethod; }
            set
            {
                _selectedContactMethod = value;
                OnPropertyChanged(nameof(SelectedContactMethod));
                OnPropertyChanged(nameof(ContactIcon));
                OnPropertyChanged(nameof(ContactInputType));
                OnSelectedContactMethodChanged();
            }
        }


        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

 



        private string _selectedStatus;
        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
                }
            }
        }


        private string _selectedContact;
        public string SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                if (_selectedContact != value)
                {
                    _selectedContact = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedContact)));
                }
            }
        }
         



        public string ContactIcon => SelectedContactMethod switch
        {
            "Facebook" => "facebook_icon.png",
            "WhatsApp" => "WhatsApp.png",
            "LinkedIn" => "linkedin_icon.png",
            _ => "",
        };

        public Keyboard ContactInputType => SelectedContactMethod switch
        {
            "Facebook" => Keyboard.Url,
            "WhatsApp" => Keyboard.Telephone,
            "LinkedIn" => Keyboard.Email,
            _ => Keyboard.Default,
        };

        public bool IsContactMethodEmpty => string.IsNullOrEmpty(BindingBook.Contactlink);

        private string _loggedInUsername;
        public string LoggedInUsername
        {
            get => _loggedInUsername;
            set
            {
                if (_loggedInUsername != value)
                {
                    _loggedInUsername = value;
                    OnPropertyChanged(nameof(LoggedInUsername));
                }
            }
        }

        private readonly BookShareDB _services = new BookShareDB(Constants.BaseUrl);

        public DonateViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            BindingBook = new Models.Book();
            Donate = new AsyncCommand(DonateBook);
            _popupageViewModel = new PopupageViewModel();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private bool IsBooknameEntryEmpty => string.IsNullOrEmpty(BindingBook.Bookname);
        private bool IsSelectedStatusEmpty => string.IsNullOrEmpty(BindingBook.Status);

        private bool IsDetailsEntryEmpty => string.IsNullOrEmpty(BindingBook.Details);
        private bool IsSelectedContactEmpty => string.IsNullOrEmpty(BindingBook.ContactMethod);
        private PopupageViewModel _popupageViewModel;


        private async Task DonateBook()
        {
            await _popupageViewModel.ShowLoadingPageAsync();
            try
            {
                if (IsBooknameEntryEmpty)
                {
                    BooknameText = "Please write your book name!";
                    return;
                }

                if (IsSelectedStatusEmpty)
                {
                    SelectedStatus = "Please select your book status!";
                    return;
                }

                if (IsDetailsEntryEmpty)
                {
                    DetailsText = "Please write your description!";
                    return;
                }

                if (IsSelectedContactEmpty)
                {
                    SelectedContact = "Please select your contact method!";
                    return;
                }
                BindingBook.Contactlink = SelectedContactMethod;
                await _services.AddBook(BindingBook);
                
                ResetInputs();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await _popupageViewModel.HideLoadingPageAsync();
            }
        }

        private void ResetInputs()
        {
            BindingBook = new Book();
            BooknameText = string.Empty;
           SelectedStatus = string.Empty;
            DetailsText = string.Empty;
           Status = string.Empty;
            SelectedContact = string.Empty;
           SelectedContactMethod = string.Empty;
            PublisherGender = string.Empty;
            UserName = string.Empty;
     
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}