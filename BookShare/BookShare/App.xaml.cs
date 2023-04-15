using BookShare.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHFqVkFrWE5GfkBAXWFKblR8QWdTflxgBShNYlxTR3ZbQ1hiT3xQdkJkWnxa;Mgo+DSMBPh8sVXJ1S0d+X1ZPckBAVXxLflF1VWRTe1p6dVdWESFaRnZdQV1nSHtScEdgWXhZdHNR;ORg4AjUWIQA/Gnt2VFhhQlJDfVtdX2tWfFN0RnNedVp0flREcC0sT3RfQF5jTX9Wd0NhWn9Zd3JVQg==;MTczNjI2OUAzMjMxMmUzMTJlMzMzN1dQZURsVk9hRGFlUkMvUWdVaUFPdC9LbmFwc1NVdDRMcmdBNW1oZEpRdVE9;MTczNjI3MEAzMjMxMmUzMTJlMzMzN0w4c0tSdm1LUGxxcXJqZW1yVkM5S3V5OUxPZlV1Q2NRU2ZEVXM4M3paeEE9;NRAiBiAaIQQuGjN/V0d+XU9HcVRGQmFBYVF2R2BJeFR0cF9HYkwgOX1dQl9gSXpSc0RlW3xed3xUT2Q=;MTczNjI3MkAzMjMxMmUzMTJlMzMzN0RNTUFlQ25Bc0pvWFVTWE1tb2Z3M1ZYei9xVkZZVDc4a2R5Q1hNNzlNMms9;MTczNjI3M0AzMjMxMmUzMTJlMzMzN01zd0diUFZzY0FCY2pWcURET1J4RmlmS2JaMkYrbWl2cEZESytDTEdhaTQ9;Mgo+DSMBMAY9C3t2VFhhQlJDfVtdX2tWfFN0RnNedVp0flREcC0sT3RfQF5jTX9Wd0NhWn9ZeHxUQg==;MTczNjI3NUAzMjMxMmUzMTJlMzMzN2ozN2Q5YmxtREFhRjNPNmVBSzRsWGxqQmtDYWZGQTdLeWhxNjY0K3E3cXM9;MTczNjI3NkAzMjMxMmUzMTJlMzMzN1pTendMK3NhOHBENVFnd1dic1pvY0MvL1hkWUc4U1hMUGZTZEl2R242Szg9;MTczNjI3N0AzMjMxMmUzMTJlMzMzN0RNTUFlQ25Bc0pvWFVTWE1tb2Z3M1ZYei9xVkZZVDc4a2R5Q1hNNzlNMms9");

            MainPage = new NavigationPage(new LoginPage());

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
