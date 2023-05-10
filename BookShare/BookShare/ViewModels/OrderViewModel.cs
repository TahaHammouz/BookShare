using System;
using System.Collections.Generic;
using System.Text;
using BookShare.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using BookShare.Views;
using BookShare.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Mail;
using System.Net;

namespace BookShare.ViewModels
{
    internal class OrderViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private PopupageViewModel _popupageViewModel;

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

        private string _bookName;

        public string BookName
        {
            get { return _bookName; }
            set
            {
                if (_bookName != value)
                {
                    _bookName = value;
                    OnPropertyChanged(nameof(BookName));


                }
            }
        }
        private string _bookDescription;

        public string BookDescription
        {
            get { return _bookDescription; }
            set
            {
                if (_bookDescription != value)
                {
                    _bookDescription = value;
                    OnPropertyChanged(nameof(BookDescription));


                }
            }
        }
        private string _studentId;

        public string StudentId
        {
            get { return _studentId; }
            set
            {
                if (_studentId != value)
                {
                    _studentId = value;
                    OnPropertyChanged(nameof(StudentId));


                }
            }
        }
        private string _facultyPicker;

        public string FacultyPicker
        {
            get { return _facultyPicker; }
            set
            {
                if (_facultyPicker != value)
                {
                    _facultyPicker = value;
                    OnPropertyChanged(nameof(FacultyPicker));


                }
            }
        }
        private string _phoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));


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

        public async void SetEmailEntry()
        {
            try
            {
                EmailEntery = await BookShareDB.GetUserEmail();
            }
            catch (Exception)
            {
                EmailEntery = "email user not found";
            }
        }
        private async Task OnOrderClick()
        {
            if (string.IsNullOrEmpty(BookName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a book name", "OK");
                return;
            }
            if (string.IsNullOrEmpty(FacultyPicker))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Please select a faculty", "OK");
                return;
            }
            string studentid = StudentId;
            if (string.IsNullOrEmpty(studentid) || studentid.Length < 8)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your student ID", "OK");
                return;
            }


            if (string.IsNullOrEmpty(BookDescription))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a book description", "OK");
                return;
            }



            string phonenumber = PhoneNumber;
            if (string.IsNullOrEmpty(phonenumber) || phonenumber.Length < 10)
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
                var subject = "New Order";
                var body = "Order a book\n\n" +
                            "Name: " + BookName + "\n" +
                            "Faculty: " + FacultyPicker + "\n" +
                            "Student ID: " + StudentId + "\n" +
                            "Description: " + BookDescription + "\n" +
                            "Phone: " + PhoneNumber + "\n";

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
            BookName = string.Empty;
            FacultyPicker = string.Empty;
            StudentId = string.Empty;
            BookDescription = string.Empty;
            PhoneNumber = string.Empty;
            IsChecked = false;
        }

        public ICommand SendOrderCommand => new Command(async () => await OnOrderClick());

        public OrderViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
            SetEmailEntry();
            _popupageViewModel = new PopupageViewModel();
        }
    }

}
