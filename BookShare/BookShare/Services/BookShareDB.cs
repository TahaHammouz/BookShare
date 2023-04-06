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
using BookShare.Views;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookShare.Services
{
    public class BookShareDB
    {
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
        private HttpClient httpClient;
        private FirebaseClient firebaseClient;


        private static readonly FirebaseAuthClient userAuthentication = new FirebaseAuthClient(_config);
        private static readonly FirebaseClient fireBaseBase = new FirebaseClient(_baseUrl);
        public BookShareDB(string url)
        {
            httpClient = new HttpClient();
            firebaseClient = new FirebaseClient(url);
        }
        public static async Task CreateUser(Models.User bindingUser)
        {

            try
            {
                var user = await userAuthentication.CreateUserWithEmailAndPasswordAsync(bindingUser.Email, bindingUser.Password);
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
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    // Display an error message to the user indicating that the email already exists
                    await App.Current.MainPage.DisplayAlert("Error", "Email already exists", "OK");
                }
                else if (ex.Reason == AuthErrorReason.WrongPassword)
                {
                    // Display an error message to the user indicating that the password is incorrect
                    await App.Current.MainPage.DisplayAlert("Error", "Incorrect password", "OK");
                }
                else
                {
                    // Display a generic error message to the user for any other exceptions
                    await App.Current.MainPage.DisplayAlert("Error", "An error occurred", "OK");
                }
            }
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
        public async Task Login(string email, string password)
        {
            try
            {
                var user = await userAuthentication.SignInWithEmailAndPasswordAsync(email, password);
                string token = user?.User?.Uid;
                Console.WriteLine("The token is " + token);
                if (!string.IsNullOrEmpty(token))
                {
                    await SecureStorage.SetAsync("auth_token", token);
                    await SecureStorage.SetAsync("issignin", "true");

                    var shell = new AppShell();
                    Application.Current.MainPage = shell;

                    await shell.GoToAsync($"//{nameof(SearchPage)}");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Email or Password is Empty", "ok");
                }
            }
            catch (FirebaseAuthException ex) when (ex.Reason == AuthErrorReason.InvalidEmailAddress || ex.Reason == AuthErrorReason.WrongPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Email or Password is wrong", "ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Failed", "An error occurred while signing in", "ok");
            }
        }


        private async Task accessToken()
        {
            try
            {
                var oauthToken = await SecureStorage.GetAsync("oauth_token");
                var oname = await SecureStorage.GetAsync("NameUser");
                userAccessToken = oauthToken;
                Console.WriteLine("The token is" + oauthToken);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        internal object GetHttpClient()
        {
            throw new NotImplementedException();
        }
    }
}