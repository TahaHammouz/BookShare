using BookShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShare.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuyPage : ContentPage
    {
        public BuyPage()
        {
            InitializeComponent();

            BindingContext = new BuyViewModel();

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var selectedItem = (sender as Frame)?.BindingContext as Order;
            if (selectedItem != null)
            {
                await Navigation.PushAsync(new ItemToSellPage(selectedItem));
            }

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
    }
}