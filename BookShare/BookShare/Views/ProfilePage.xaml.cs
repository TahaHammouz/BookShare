using BookShare.Models;
using BookShare.Services;
using BookShare.ViewModels;
using BookShare.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        BookShareDB firebaseDataService = new BookShareDB("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
        private ProfileViewModel _viewModel;
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel();
            _viewModel = new ProfileViewModel();
            BindingContext = _viewModel;
            MessagingCenter.Subscribe<ProfileViewModel>(this, "NavigateToSearchPage", async (sender) =>
            {
                await Navigation.PushAsync(new SearchPage());
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!ConnectivityHelper.IsConnected())
            {
                await Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            await _viewModel.LoadPostsAsync();


        }


        private async void EditProfile(object sender, EventArgs e)
        {


            User selectedItem = _viewModel.GetUserInfo();
            if (selectedItem != null)
            {

                await Navigation.PushAsync(new EditProfilePage(selectedItem));


            }
            else { Console.WriteLine("bad sender"); }
        }

        private async void OnItemTapped(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Menu", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    Book selectedItem = (sender as Frame)?.BindingContext as Book;
                    if (selectedItem != null)
                    {
                        await Navigation.PushAsync(new EditDonatePage(selectedItem));
                    }
                    break;
                case "Delete":
                    Book selectedItem1 = (sender as Frame)?.BindingContext as Book;
                    if (selectedItem1 != null)
                    {
                        await _viewModel.DeleteAndRefresh(selectedItem1);
                    }
                    break;

            }
        }



    }
}