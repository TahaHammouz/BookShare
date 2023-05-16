using BookShare.Models;
using BookShare.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuyOrderPage : ContentPage
    {

        internal BuyOrderPage(Order order)
        {
            InitializeComponent();
            BindingContext = new ItemToSellViewModel(order);

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