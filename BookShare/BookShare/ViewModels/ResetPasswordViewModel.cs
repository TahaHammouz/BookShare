using System.Windows.Input;
using System;
using MvvmHelpers;
using BookShare.Services;
using MvvmHelpers.Commands;
namespace BookShare.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private BookShareDB _firebaseAuthService;
        private string _email;


        public ResetPasswordViewModel()
        {

            _firebaseAuthService = new BookShareDB(Constants.BaseUrl);
            ResetPasswordCommand = new Command(ResetPassword);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public ICommand ResetPasswordCommand { get; }

        private async void ResetPassword()
        {
            try
            {
                await _firebaseAuthService.ResetPassword(Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
