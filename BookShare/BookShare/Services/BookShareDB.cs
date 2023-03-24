using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BookShare.Models;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace BookShare.Services
{
    internal class BookShareDB
    {
        FirebaseClient client;

        public BookShareDB()
        {
            client = new FirebaseClient("https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/");
        }
        public async Task AddBook(Book book)
        {
            await client.Child("books").PostAsync(book);
        }


    }

}
