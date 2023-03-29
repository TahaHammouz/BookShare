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
    public partial class ContactPage : ContentPage

    {
        private readonly Book _selectedBook;

        public ContactPage(Book selectedItem)
        {
            InitializeComponent();
            _selectedBook = selectedItem;

            // Set the BindingContext to the selected book
            BindingContext = _selectedBook;
        }


        public ContactPage(SearchViewModel book)
        {
            InitializeComponent();
        }
    }
}

