using BookShare.Models;
using BookShare.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BookShare.ViewModels
{
    internal class EditProfileViewModel : BaseViewModel
    {
        private readonly BookShareDB firebaseDataService;
        private ProfileViewModel _profileViewModel = new ProfileViewModel { };

        public EditProfileViewModel(User selecteduser)
        {
            SelectedUser = selecteduser;
            firebaseDataService = new BookShareDB("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
            SaveCommand = new Xamarin.Forms.Command(async () => await UpdateUser(selecteduser));

        }
        public EditProfileViewModel()
        {

        }
        public ICommand SaveCommand { get; }
        private User selecteduser;
        public User SelectedUser
        {
            get { return selecteduser; }
            set
            {
                if (selecteduser != value)
                {
                    selecteduser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        public async Task UpdateUser(User user)
        {
            if (selecteduser.Equals(user))
            {
                try
                {
                    await firebaseDataService.UpdateUser(selecteduser);
                    await App.Current.MainPage.DisplayAlert("Success", "User Edited Successfully!", "OK");
                    MessagingCenter.Send(this, "NavigateToNewPage");
                    OnPropertyChanged(nameof(User));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Editing Failed: {ex.Message}");
                    await App.Current.MainPage.DisplayAlert("Failed", "Cant Edit user info try again!", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failed", "User info didnt modified!", "OK");
            }
        }

    }
}