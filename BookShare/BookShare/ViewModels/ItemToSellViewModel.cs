using BookShare.Models;
using BookShare.Services;
using BookShare.Views;
using Firebase.Auth;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BookShare.ViewModels
{
    internal class ItemToSellViewModel : BaseViewModel
    {
        private readonly BookShareDB firebaseDataService;
        private PopupageViewModel _popupageViewModel;

        public ItemToSellViewModel(Order selectedItem)
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            SelectedItem = selectedItem;
            SetInfo();
            SetEmail();
            firebaseDataService = new BookShareDB(Constants.BaseUrl);
            OrderCommand = new Xamarin.Forms.Command(async () => await OnOrderClick());
            _popupageViewModel = new PopupageViewModel();
        }
        public ItemToSellViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
        }
        public ICommand OrderCommand { get; }
        private Order selectedItem;
        public Order SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }


        private string _emailEntery;
        public string EmailEntery
        {
            get { return _emailEntery; }
            set
            {
                if (_emailEntery != value)
                {
                    _emailEntery = value;
                    OnPropertyChanged(nameof(EmailEntery));


                }
            }
        }
        private string _bookname;
        public string BookName
        {
            get { return _bookname; }
            set
            {
                if (_bookname != value)
                {
                    _bookname = value;
                    OnPropertyChanged(nameof(BookName));


                }
            }
        }
        private string _price;
        public string Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));


                }
            }
        }
        private string _desc;
        public string Desc
        {
            get { return _desc; }
            set
            {
                if (_desc != value)
                {
                    _desc = value;
                    OnPropertyChanged(nameof(Desc));


                }
            }
        }

        private string _studentid;
        public string StudentId
        {
            get { return _studentid; }
            set
            {
                if (_studentid != value)
                {
                    _studentid = value;
                    OnPropertyChanged(nameof(StudentId));


                }
            }
        }
        private string _phonenumber;
        public string PhoneNumber
        {
            get { return _phonenumber; }
            set
            {
                if (_phonenumber != value)
                {
                    _phonenumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));


                }
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));


                }
            }
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }
        public async void SetEmail()
        {
            try
            {

                EmailEntery = await BookShareDB.GetUserEmail();

            }
            catch (Exception)
            {
                EmailEntery = "user Email not found";
            }
        }

        public void SetInfo()
        {
            try
            {
                BookName = selectedItem.Bookname;
                Price = selectedItem.BookPrice;
                Desc = selectedItem.Description;

            }
            catch (Exception)
            {
                EmailEntery = "user info not found";
            }
        }

        private async Task OnOrderClick()
        {

            if (string.IsNullOrEmpty(BookName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a book name", "OK");
                return;
            }

            if (string.IsNullOrEmpty(StudentId) || StudentId.Length <= 8)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your student ID", "OK");
                return;
            }


            if (string.IsNullOrEmpty(Desc))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a book description", "OK");
                return;
            }

            if (string.IsNullOrEmpty(Address))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your address", "OK");
                return;
            }

            if (string.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Length < 10)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid phone number", "OK");
                return;
            }

            if (!IsChecked)
            {
                await Application.Current.MainPage.DisplayAlert("Validation Failed", "Please agree to the terms and conditions", "OK");
                return;
            }

            try
            {
                await _popupageViewModel.ShowLoadingPageAsync();
                var recipient = EmailEntery;
                var subject = "Book order";
                string body = "Your order has been received successfully\n\n";


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("najahbookshare@gmail.com");
                mail.To.Add(new MailAddress(recipient));
                mail.Subject = subject;
                mail.Body = body;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("najahbookshare@gmail.com", "ccaloddbaxrsmpxq");
                await smtp.SendMailAsync(mail);
                await Application.Current.MainPage.DisplayAlert("Success", "Your order has been submitted successfully.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while submitting your order. Please try again later.", "OK");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await _popupageViewModel.HideLoadingPageAsync();

            }

            try
            {

                string recipient = EmailEntery;
                string subject = "Book order";
                var body = "We have a new order\n\n" +
                         "Book name  : " + BookName + "\n" +
                         "Book Price : " + Price + "\n" +
                         "Student  Address : " + Address + "\n" +
                         "Student ID : " + StudentId + "\n" +
                         "Book Description : " + Desc + "\n" +
                         "Student Contact number : " + PhoneNumber + "\n";

                var mail = new MailMessage();


                mail.From = new MailAddress(recipient);
                mail.To.Add(new MailAddress("najahbookshare@gmail.com"));


                mail.Subject = subject;
                mail.Body = body;

                var smtp = new SmtpClient("smtp.gmail.com", 587);


                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("najahbookshare@gmail.com", "ccaloddbaxrsmpxq");


                await smtp.SendMailAsync(mail);


                await Application.Current.MainPage.DisplayAlert("Success", "You will recive an email .", "OK");

                var userMail = new MailMessage();
                userMail.From = new MailAddress(recipient);
                userMail.To.Add(new MailAddress("najahbookshare@gmail.com"));
                userMail.Subject = subject;
                userMail.Body = body;
                await smtp.SendMailAsync(userMail);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred", "OK");
            }
            StudentId = string.Empty;
            Address = string.Empty;
            StudentId = string.Empty;
            PhoneNumber = string.Empty;
            IsChecked = false;
        }
    }
}