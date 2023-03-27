using BookShare.Models;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookShare.Services
{
    public class BookShareDB
    {
        private HttpClient httpClient;
        private FirebaseClient firebaseClient;

        public BookShareDB(string url)
        {
            httpClient = new HttpClient();
            firebaseClient = new FirebaseClient(url);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var firebaseUrl = "https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/books.json";
            var response = await httpClient.GetAsync(firebaseUrl);
            var content = await response.Content.ReadAsStringAsync();

            var booksDict = JsonConvert.DeserializeObject<Dictionary<string, Book>>(content);
            var books = new List<Book>(booksDict.Values);

            return books;
        }

        internal object GetHttpClient()
        {
            throw new NotImplementedException();
        }
    }
}