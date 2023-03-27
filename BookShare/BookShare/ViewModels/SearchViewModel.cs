using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BookShare.Models;
using BookShare.Services;

namespace BookShare.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private BookShareDB firebaseDataService;

        public   SearchViewModel()
        {
            try { 
            firebaseDataService = new BookShareDB("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
                _ = LoadBooksAsync();
            }catch(Exception e) { e.Source = "SearchViewModel"; }
        }

        private ObservableCollection<Book> books;
        public ObservableCollection<Book> Books
        {
       
                get { return books; }
                set { books = value; OnPropertyChanged(); }
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                var books = await firebaseDataService.GetBooksAsync();
                Books = new ObservableCollection<Book>(books);
            }
            catch(Exception e)
            {
                e.Source = "LoadBooksAsync";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}