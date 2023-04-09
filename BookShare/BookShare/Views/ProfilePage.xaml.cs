using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();


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
                    await Navigation.PushAsync(new SearchPage());
                    break;
                case "delete":
                    // Handle option 2
                    break;
                default:
                    // Handle cancel or unknown option
                    break;
            }
        }

    }
}
