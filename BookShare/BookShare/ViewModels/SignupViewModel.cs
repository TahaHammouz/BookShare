using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using BookShare.Models;
using BookShare.Services;
using BookShare.Views;

namespace BookShare.ViewModels
{
    public static class ImageSourceConstants
    {
        public static ImageSource Eye => ImageSource.FromFile("view.png");
        public static ImageSource EyeOff => ImageSource.FromFile("closeeye.png");
    }
    public class SignupViewModel : INotifyPropertyChanged
    {
        public ICommand CreateAcount { set; get; }
        public Command LogInPageCommand { get; }

        public ICommand TogglePasswordCommand { get; }

        public Models.User bindingUser { set; get; }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged(nameof(IsPasswordVisible));
                OnPropertyChanged(nameof(PasswordIcon));
            }
        }

        private bool _isvalidInput;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValidInput
        {
            get { return _isvalidInput; }
            set
            {
                _isvalidInput = value;
                OnPropertyChanged(nameof(IsValidInput));
            }
        }

        public ImageSource PasswordIcon => IsPasswordVisible ? ImageSourceConstants.Eye : ImageSourceConstants.EyeOff;




        public SignupViewModel()
        {
            bindingUser = new User();

            CreateAcount = new AsyncCommand(CreateAccount);
            IsPasswordVisible = false;
            TogglePasswordCommand = new MvvmHelpers.Commands.Command(() => IsPasswordVisible = !IsPasswordVisible);
            LogInPageCommand = new Command(NavigateToLogInPage);
        }

        public async Task CreateAccount()
        {

            if (IsValidInput)
            {

                await BookShareDB.CreateUser(bindingUser);

            }
            else
            {
                Debug.WriteLine("Invalid");
            }

        }
        private void NavigateToLogInPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}