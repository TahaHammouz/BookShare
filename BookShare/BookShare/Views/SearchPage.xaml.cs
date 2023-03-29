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
                // Navigate to the ContactPage and pass the selected book as parameter
                await Navigation.PushAsync(new ContactPage(selectedItem));
            }
        }
        


    }
}