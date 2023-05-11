using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using BookShare.Models;
using BookShare.Services;
using System;
using static BookShare.Views.SignupPage;
using Xamarin.Essentials;
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

        public ICommand TogglePasswordCommand { get; }

        public Command LogInPageCommand { get; }

        public Models.User BindingUser { set; get; }

        private PopupageViewModel _popupageViewModel;


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
        public bool IsEmailEntryEmpty => string.IsNullOrEmpty(BindingUser.Email);


        private string _usernameText;
        public string UsernameText
        {
            get { return _usernameText; }
            set
            {
                if (_usernameText != value)
                {
                    _usernameText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsernameText)));
                }
            }
        }
        public bool IsUsernameEntryEmpty => string.IsNullOrEmpty(BindingUser.Name);

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
        public bool IsPasswordEntryEmpty => string.IsNullOrEmpty(BindingUser.Password);


        private string _selectedFaculty;
        public string SelectedFaculty
        {
            get { return _selectedFaculty; }
            set
            {
                if (_selectedFaculty != value)
                {
                    _selectedFaculty = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFaculty)));
                }
            }
        }
        public bool IsSelectedFacultyEmpty => string.IsNullOrEmpty(BindingUser.Faculty);

        private string _selectedGender;
        public string SelectedGender
        {
            get { return _selectedGender; }
            set
            {
                if (_selectedGender != value)
                {
                    _selectedGender = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedGender)));
                }
            }
        }
        public bool IsSelectedGenderEmpty => string.IsNullOrEmpty(BindingUser.Gender);

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

        public ImageSource PasswordIcon => IsPasswordVisible ? ImageSourceConstants.EyeOff : ImageSourceConstants.Eye;

        private void NavigateToLogInPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }


        public SignupViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            BindingUser = new User();
            CreateAcount = new AsyncCommand(CreateAccount);
            IsPasswordVisible = true;
            TogglePasswordCommand = new MvvmHelpers.Commands.Command(() => IsPasswordVisible = !IsPasswordVisible);
            LogInPageCommand = new Command(NavigateToLogInPage);
            _popupageViewModel = new PopupageViewModel();

        }


        public async Task CreateAccount()
        {
            await _popupageViewModel.ShowLoadingPageAsync();

            EmailText = UsernameText = PasswordText = SelectedFaculty = SelectedGender = string.Empty;

            if (IsEmailEntryEmpty)
            {
                EmailText = "please write your Email !";
            }
            else if (IsUsernameEntryEmpty)
            {
                UsernameText = "please write your Username !";
            }
            else if (IsPasswordEntryEmpty)
            {
                PasswordText = "please write your Password !";
            }
            else if (IsSelectedFacultyEmpty)
            {
                SelectedFaculty = "please select your Faculty !";
            }
            else if (IsSelectedGenderEmpty)
            {
                SelectedGender = "please select your Gender !";
            }
            else if (!IsValidInput)
            {

                EmailText = "Please use your unversity email, example@stu.najah.edu";

            }
            else
            {
                await BookShareDB.CreateUser(BindingUser);
                await _popupageViewModel.HideLoadingPageAsync();
            }

        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
