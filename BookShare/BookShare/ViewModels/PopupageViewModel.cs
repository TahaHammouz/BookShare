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

        public async Task ShowLoadingPageAsync()
        {
            IsLoading = true;
            await PopupNavigation.Instance.PushAsync(new Popupage());
        }

        public async Task HideLoadingPageAsync()
        {
            IsLoading = false;
            await PopupNavigation.Instance.PopAsync();
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

