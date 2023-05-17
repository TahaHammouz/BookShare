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
using BookShare.ViewModels;

namespace BookShare.Services
{
    public class BookShareDB
    {
        private string userAccessToken { get; set; }
        private static readonly FirebaseAuthConfig _config = new FirebaseAuthConfig
        {
            ApiKey = Constants.ApiKey,
            AuthDomain = Constants.AuthDomain,
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            },
        };
        private HttpClient httpClient;
        private FirebaseClient firebaseClient;

        private static readonly FirebaseAuthClient userAuthentication = new FirebaseAuthClient(_config);
        private static readonly FirebaseClient fireBaseBase = new FirebaseClient(Constants.BaseUrl);
        public BookShareDB(string url)
        {
            httpClient = new HttpClient();
            firebaseClient = new FirebaseClient(url);
        }
        public async Task ResetPassword(string email)
        {
            try
            {
                await userAuthentication.ResetEmailPasswordAsync(email);
                await App.Current.MainPage.DisplayAlert("Success", "Reset password email sent successfully.", "OK");
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.InvalidEmailAddress)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Invalid email address.", "OK");
                }
                else if (ex.Reason == AuthErrorReason.UserNotFound)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User not found.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "An error occurred" + ex.Message, "OK");
                }
            }
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
                    await App.Current.MainPage.DisplayAlert("Error", "Email already exists", "OK");
                }
                else if (ex.Reason == AuthErrorReason.WeakPassword)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Incorrect password", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "An error occurred" + ex.Message, "OK");
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

        public async Task<List<Models.User>> GetUsersAsync()
        {
            var firebaseUrl = "https://bookshare-33c3f-default-rtdb.europe-west1.firebasedatabase.app/Users.json";
            var response = await httpClient.GetAsync(firebaseUrl);
            var content = await response.Content.ReadAsStringAsync();
            var UsersDict = JsonConvert.DeserializeObject<Dictionary<string, Models.User>>(content);
            var Users = new List<Models.User>(UsersDict.Values);
            return Users;
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

                var loginViewModel = new LoginViewModel();
                var loginPage = new LoginPage()
                {
                    BindingContext = loginViewModel
                };

                // Use the MainPage's Navigation to push the LoginPage
                await Application.Current.MainPage.Navigation.PushModalAsync(loginPage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Failed", "An error occurred while signing in", "ok");
            }
        }



        public static async Task<string> GetUserEmail()
        {
            string _accessToken = await SecureStorage.GetAsync("auth_token");
            Console.WriteLine("The token is" + _accessToken);
            var informationUser = (await fireBaseBase.Child("Users").OnceAsync<Models.User>())
                .FirstOrDefault(item => item.Object.Uid == _accessToken);
            Console.WriteLine("User information = " + informationUser);

            if (informationUser != null)
            {
                return informationUser.Object.Email.ToString();
            }
            else
            {
                Console.WriteLine("User not found");
                return null;
            }
        }
        public async Task UpdateUser(Models.User u)
        {
            var users = await firebaseClient
                        .Child("Users")
                        .OnceAsync<Models.User>();
            var books = await firebaseClient
                        .Child("books")
                        .OnceAsync<Models.Book>();

            foreach (var user in users)
            {
                if (user.Object.Uid == u.Uid)
                {
                    await firebaseClient.Child("Users").Child(user.Key).PutAsync(u);
                }
            }

            foreach (var book in books)
            {
                // Check if the book belongs to the updated user
                if (book.Object.UserId == u.Uid)
                {
                    // Update the book with the new user name and ID
                    book.Object.Username = u.Name;
                    book.Object.UserId = u.Uid;
                    await firebaseClient.Child("books").Child(book.Key).PutAsync(book.Object);
                }
            }
        }
        
        public async Task UpdateBook(Book b)
        {
            var books = await firebaseClient
                        .Child("books")
                        .OnceAsync<Book>();
            foreach (var book in books)
            {
                if (book.Object.BookId == b.BookId)
                {
                    await firebaseClient.Child("books").Child(book.Key).PutAsync(b);
                }
            }
        }
        public async Task DeletePost(Book book)
        {
            bool response = await App.Current.MainPage.DisplayAlert("Alert", "Do you want to delete this post ?", "Yes", "No");

            if (response)
            {
                var books = await firebaseClient
                            .Child("books")
                            .OnceAsync<Book>();

                foreach (var x in books)
                {
                    if (x.Object.BookId == book.BookId)
                    {
                        await firebaseClient
                            .Child("books")
                            .Child(x.Key)
                            .DeleteAsync();
                    }
                }
                await App.Current.MainPage.DisplayAlert("Success", "Book Deleted Successfly", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failed", "Book Deleting Failed", "OK");
            }
        }

        private async Task AccessToken()
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



        public async Task AddBook(Models.Book bindingBook)
        {
            string _acessToken = await SecureStorage.GetAsync("auth_token");
            Console.WriteLine("The token is" + _acessToken);
            var informationUser = (await fireBaseBase.Child("Users").OnceAsync<Models.User>())
                .FirstOrDefault(item => item.Object.Uid == _acessToken);
            Console.WriteLine("User information = " + informationUser);
            var books = await fireBaseBase.Child("books").OnceAsync<Book>();
            if (informationUser != null)
            {
                var book = await firebaseClient.Child("books").PostAsync(new Book
                {
                    BookId = Guid.NewGuid().ToString(),
                    UserId = _acessToken,
                    Bookname = bindingBook.Bookname,
                    Status = bindingBook.Status,
                    Details = bindingBook.Details,
                    Contactlink = bindingBook.Contactlink,
                    Username = informationUser.Object.Name.ToString(),
                    PublisherGender = informationUser.Object.Gender,
                    ContactMethod = bindingBook.ContactMethod
                });
                await App.Current.MainPage.DisplayAlert("Success", "Book Donated successfully!", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failed", "Book Donation Failed!", "OK");
            }
        }
        internal object GetHttpClient()
        {
            throw new NotImplementedException();
        }
    }
}