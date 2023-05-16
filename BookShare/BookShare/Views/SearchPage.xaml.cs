using BookShare.Models;
using BookShare.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();

        }
        private async void OnItemTapped(object sender, EventArgs e)
        {

            var selectedItem = (sender as Frame)?.BindingContext as Book;
            if (selectedItem != null)
            {
                await Navigation.PushAsync(new ContactPage(selectedItem));
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