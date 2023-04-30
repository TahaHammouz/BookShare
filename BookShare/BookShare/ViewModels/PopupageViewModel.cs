using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BookShare.Views;
using Rg.Plugins.Popup.Services;

namespace BookShare.ViewModels
{
    public class PopupageViewModel : INotifyPropertyChanged
    {
        public PopupageViewModel()
        {
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
            IsLoading = true;
            IsBusy = true; 
            await PopupNavigation.Instance.PushAsync(new Popupage());
        }

        public async Task HideLoadingPageAsync()
        {
            IsLoading = false;
            IsBusy = false;
            await PopupNavigation.Instance.PopAsync(true);
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
