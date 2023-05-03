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
    public partial class EditDonatePage : ContentPage
    {
        public EditDonatePage(Book selectedBook)
        {
            InitializeComponent();
            BindingContext = new EditDonateViewModel(selectedBook);
            MessagingCenter.Subscribe<EditDonateViewModel>(this, "NavigateToNewPage", async (sender) =>
            {
                await Navigation.PushAsync(new ProfilePage());
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
        }
        public EditDonatePage() { InitializeComponent(); }
    }
}