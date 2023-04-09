using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup;
using Xamarin.CommunityToolkit.PlatformConfiguration.iOSSpecific;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();


        }
        private void editProfile(object sender, EventArgs e)
        {
            // Handle the tap event here
        }

        
        private async void ellipesesTapped(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Menu", "Cancel", null, "edit", "delete");

            switch (action)
            {
                case "edit":
                    // Handle option 1
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
