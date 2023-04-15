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

namespace BookShare.ViewModels
{
    public class DonateViewModel : INotifyPropertyChanged
    {
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
                OnSelectedContactMethodChanged(); // added call to update visibility
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
        public bool IsSelectedContacEmpty => string.IsNullOrEmpty(BindingBook.ContactMethod);



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

        private readonly BookShareDB _services = new BookShareDB("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");

        public event PropertyChangedEventHandler PropertyChanged;

        public DonateViewModel()
        {
            BindingBook = new Models.Book();
            Donate = new AsyncCommand(DonateBook);

        }
        public bool IsBooknameEntryEmpty => string.IsNullOrEmpty(BindingBook.Bookname);
           public bool IsSelectedStatusEmpty => Status == null;

        public bool IsDetailsEntryEmpty => string.IsNullOrEmpty(BindingBook.Details);
        public bool IsSelectedContactEmpty => string.IsNullOrEmpty(BindingBook.ContactMethod);
        public async Task DonateBook()
        {
            try
            {
                BooknameText = SelectedStatus = DetailsText = SelectedContact = string.Empty;

                if (IsBooknameEntryEmpty)
                {
                    BooknameText = "please write your Book name !";
                }
                else if (IsSelectedStatusEmpty)
                {
                    SelectedStatus = "please select your book status !";
                }
                else if (IsDetailsEntryEmpty)
                {
                    DetailsText = "please write your descrption!";
                }
                else if (IsSelectedContactEmpty)
                {
                    SelectedContact = "please select your contact method !";
                }

                else
                {

                    BindingBook.Status = Status;
                    BindingBook.Contactlink = SelectedContactMethod;
                    await _services.AddBook(BindingBook);
                    BooknameText = string.Empty;
                    DetailsText = string.Empty;
                    SelectedStatus = string.Empty;
                    SelectedContact = string.Empty;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }

        }
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}