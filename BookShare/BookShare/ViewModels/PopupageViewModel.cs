using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BookShare.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BookShare.ViewModels
{
    public class PopupageViewModel : INotifyPropertyChanged
    {
        public PopupageViewModel()
        {
            if (!ConnectivityHelper.IsConnected())
            {
                Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
                return;
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }

        public async Task ShowLoadingPageAsync()
        {
            await PopupNavigation.Instance.PushAsync(new Popupage());
        }

        public async Task HideLoadingPageAsync()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAsync();
            }

        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
