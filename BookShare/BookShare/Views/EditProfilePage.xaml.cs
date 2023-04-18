using BookShare.ViewModels;
using BookShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        public EditProfilePage(Models.User user)
        {
            InitializeComponent();
            BindingContext = new EditProfileViewModel(user);
            MessagingCenter.Subscribe<EditProfileViewModel>(this, "NavigateToNewPage", async (sender) =>
            {
                await Navigation.PushAsync(new ProfilePage());
            });

        }
        public EditProfilePage()
        {

        }

    }
}