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

            await _viewModel.LoadPostsAsync();
        }
        private void EditProfile(object sender, EventArgs e)
        {
            // Handle the tap event here
        }


        private async void EllipesesTapped(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Menu", "Cancel", null, "edit", "delete");

            switch (action)
            {
                case "edit":
                    Book selectedItem = (sender as Image)?.BindingContext as Book;
                    if (selectedItem != null)
                    {
                        await Navigation.PushAsync(new EditDonatePage(selectedItem));
                    }
                    break;
                case "delete":
                    Book selectedItem1 = (sender as Image)?.BindingContext as Book;
                    if (selectedItem1 != null)
                    {
                        await _viewModel.DeleteAndRefresh(selectedItem1);
                    }
                    break;
                default:
                    // Handle cancel or unknown option
                    break;
            }
        }

    }
}
