using BookShare.Models;
using BookShare.ViewModels;
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
    public partial class ItemToSellPage : ContentPage
    {
        public ItemToSellPage(Order order)
        {
            InitializeComponent();
            BindingContext = new ItemToSellViewModel(order);

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

            ItemToSellViewModel sellViewModel = (ItemToSellViewModel)BindingContext;
            await Navigation.PushAsync(new BuyOrderPage(sellViewModel.SelectedItem));


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