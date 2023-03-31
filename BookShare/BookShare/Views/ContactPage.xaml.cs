using BookShare.Models;
using BookShare.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactPage : ContentPage
    {
        private Book _selectedBook;

        public ContactPage(Book selectedBook)
        {
            InitializeComponent();
            BindingContext = new ContactViewModel(selectedBook);
        }
    }
}
