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
        public Command ResetPasswordCommand { get; }

        private string _email;
        private string _password;
        private bool _isLoading;
        private readonly BookShareDB _services;
        private PopupageViewModel _popupageViewModel;


        public LoginViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            _services = new BookShareDB("https://book-share-9ab66-default-rtdb.firebaseio.com/");
            SubmitCommand = new Command(async () => await SignIn(_email, _password));
            SignUpPageCommand = new Command(NavigateToSignUpPage);
            ResetPasswordCommand = new Command(NavigateToResetPage);
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

            if (string.IsNullOrEmpty(email))
            {
                EmailText = "Please enter your email.";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                PasswordText = "Please enter your password.";
                return;
            }

            try
            {
                await _popupageViewModel.ShowLoadingPageAsync();
                await _services.Login(email, password);

                EmailText = PasswordText = String.Empty;
                await _popupageViewModel.HideLoadingPageAsync();
            }
            catch (Exception ex)
            {
                EmailText = "Invalid email or password.";
                Console.WriteLine(ex);
            }
            finally
            {
                await _popupageViewModel.HideLoadingPageAsync();
            }
        }




        private void NavigateToSignUpPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new SignupPage());
        }
        private void NavigateToResetPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new ResetPasswordPage());
        }
    }
}
