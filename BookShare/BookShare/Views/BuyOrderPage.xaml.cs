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
    }
}