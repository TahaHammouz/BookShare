using BookShare.Services;
using BookShare.Views;
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
        public LoginViewModel()
        {
            _services = new BookShareDB("https://book-share-9ab66-default-rtdb.firebaseio.com/");
            SubmitCommand = new Command(async () => await SignIn(_email, _password));
            SignUpPageCommand = new Command(NavigateToSignUpPage);
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

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private readonly BookShareDB _services;



        private async Task SignIn(string email, string password)
        {
            await _services.Login(email, password);

        }

        private void NavigateToSignUpPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new SignupPage());
        }
    }
}
