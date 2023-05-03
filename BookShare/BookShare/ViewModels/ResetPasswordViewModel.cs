using System.Windows.Input;
using System;
using MvvmHelpers;
using BookShare.Services;
using MvvmHelpers.Commands;

public class ResetPasswordViewModel : BaseViewModel
{
    private readonly BookShareDB _firebaseAuthService;
    private string _email;

    public ResetPasswordViewModel(BookShareDB firebaseAuthService)
    {
        _firebaseAuthService = firebaseAuthService;
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
            // Show success message to the user
        }
        catch (Exception ex)
        {
            // Handle error and show message to the user
        }
    }
}
