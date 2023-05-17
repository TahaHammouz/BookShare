using BookShare.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            if (!CheckNetworkConnectivityAsync())
            {
                MainPage = new ContentPage
                {
                    Content = new Label
                    {
                        Text = "Please connect to the internet before using this app.",
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                       
                    }
                    
                };
              
                return;
            }
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVk1mQ1BCaV1CX2BZe1l2RWlcdk4BCV5EYF5SRHNeRV1mSX9TdkRhUH0=;Mgo+DSMBPh8sVXJ1S0R+X1pCaV5EQmFJfFBmQGlZf1R1fUU3HVdTRHRcQlhiTn5WdkRjWX5adXY=;ORg4AjUWIQA/Gnt2VFhiQlJPcEBDW3xLflF1VWZTf156cVxWESFaRnZdQV1mSHlTc0ViWX9fdnRX;MjA4NjgzNUAzMjMxMmUzMjJlMzNXTlBRQWpMbUpVUGYzcWZjWHVuMXZpbi9qbmJXdjFpRnRIZDVvYmRLK3FFPQ==;MjA4NjgzNkAzMjMxMmUzMjJlMzNMZGR0ZTJGbWFJck9lSlZoaFRWZlM5MWRTS093QTB6MDE4Vmc5N2FEY3VjPQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfVldXGVWfFN0RnNcdV5wflBPcC0sT3RfQF5jTH9UdkBjWH9ecnZQRA==;MjA4NjgzOEAzMjMxMmUzMjJlMzNkRkk1SUJTdzhXNkM3azNmWWtvUkwwZXZxL0dIVW9QeVU0dE5MbCt3MFVFPQ==;MjA4NjgzOUAzMjMxMmUzMjJlMzNuRzJOYUtrQ29icE1PT3oySzJIVnI1T2lCUXhrcDlmdk9DZFMzR25sWmh3PQ==;Mgo+DSMBMAY9C3t2VFhiQlJPcEBDW3xLflF1VWZTf156cVxWESFaRnZdQV1mSHlTc0ViWX9ceXBX;MjA4Njg0MUAzMjMxMmUzMjJlMzNYd0xKbk9QaVVpUGRpZzkwRmUrYzFoMGRNN01yRVEzMVc0aWV0UUVhSFQ0PQ==;MjA4Njg0MkAzMjMxMmUzMjJlMzNRcUFJUDBBaW9iTzlQU1ZHTFhVemVhUHF4NTJtK3QrQyt1U3Rkd095dDlrPQ==;MjA4Njg0M0AzMjMxMmUzMjJlMzNkRkk1SUJTdzhXNkM3azNmWWtvUkwwZXZxL0dIVW9QeVU0dE5MbCt3MFVFPQ==");

            MainPage = new NavigationPage(new LoginPage());

        }
        private  bool CheckNetworkConnectivityAsync()
        {
            var networkAccess = Connectivity.NetworkAccess;

            if (networkAccess != NetworkAccess.Internet)
            {
             return false;
            }

            
            return true;
        }
        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
