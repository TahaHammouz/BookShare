using BookShare.Models;
using Firebase.Auth.Providers;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database.Query;

namespace BookShare.Services
{
    public class BookShareDB
    {
        private HttpClient httpClient;
        private FirebaseClient firebaseClient;
        private static readonly string _apiKey = "AIzaSyD-Qf4rSCuiatxgta-6e93RR_rBJ5hWjR0";

        private static readonly string _authDomain = "bookshare-33c3f.firebaseapp.com";

        private static readonly string _baseUrl = "https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/";
        private string userAccessToken { get; set; }
        private static readonly FirebaseAuthConfig _config = new FirebaseAuthConfig
        {
            ApiKey = _apiKey,
            AuthDomain = _authDomain,
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            },
        };

        private static readonly FirebaseAuthClient userAuth = new FirebaseAuthClient(_config);
        private static readonly FirebaseClient fireBaseBase = new FirebaseClient(_baseUrl);
        public BookShareDB(string url)
        {
            httpClient = new HttpClient();
            firebaseClient = new FirebaseClient(url);
        }
        public static async Task CreateUser(Models.User bindingUser)
        {

            var users = await fireBaseBase.Child("Users").OnceAsync<UserDatabase>();
            bool emailExists = users.Any(u => u.Object.Email == bindingUser.Email);

            if (emailExists)
            {

                await App.Current.MainPage.DisplayAlert("Error", "This email is already registered.", "OK");
                return;
            }


            var user = await userAuth.CreateUserWithEmailAndPasswordAsync(bindingUser.Email, bindingUser.Password);
            await fireBaseBase.Child("Users").PostAsync(new UserDatabase
            {
                Uid = user.User.Uid,
                Email = user.User.Info.Email,
                Faculty = bindingUser.Faculty,
                Name = bindingUser.Name,
                Gender = bindingUser.Gender
            });


            await App.Current.MainPage.DisplayAlert("Success", "User registered successfully.", "OK");
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