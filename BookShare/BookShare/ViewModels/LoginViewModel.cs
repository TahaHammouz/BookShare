using BookShare.Services;
using BookShare.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BookShare.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Command SubmitCommand { get; }
        public Command SignUpPageCommand { get; }

        private string _email;
        private string _password;
        private bool _isLoading;
        private readonly BookShareDB _services;
        private PopupageViewModel _popupageViewModel;


        public LoginViewModel()
        {
            _services = new BookShareDB("https://book-share-9ab66-default-rtdb.firebaseio.com/");
            SubmitCommand = new Command(async () => await SignIn(_email, _password));
            SignUpPageCommand = new Command(NavigateToSignUpPage);
            _popupageViewModel = new PopupageViewModel();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        private string _emailText;
        public string EmailText
        {
            get { return _emailText; }
            set
            {
                if (_emailText != value)
                {
                    _emailText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmailText)));
                }
            }
        }

        public bool IsEmailEntryEmpty => string.IsNullOrEmpty(Email);

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private string _passwordText;
        public string PasswordText
        {
            get { return _passwordText; }
            set
            {
                if (_passwordText != value)
                {
                    _passwordText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordText)));
                }
            }
        }

        public bool IsPasswordEntryEmpty => string.IsNullOrEmpty(Password);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }

        private async Task SignIn(string email, string password)
        {
            EmailText = PasswordText = String.Empty;

            if (IsEmailEntryEmpty)
            {
                EmailText = "please write your Email !";
            }
            else if (IsPasswordEntryEmpty)
            {
                PasswordText = "please write your Password !";
            }
            else
            {
                try
                {
                    await _popupageViewModel.ShowLoadingPageAsync();
                    await _services.Login(email, password);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    await _popupageViewModel.HideLoadingPageAsync();
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
            }
        }


        private void NavigateToSignUpPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new SignupPage());
        }
    }
}
