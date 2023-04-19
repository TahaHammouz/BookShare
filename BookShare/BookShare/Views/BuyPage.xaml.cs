﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShare.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookShare.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuyPage : ContentPage
    {
        public BuyPage()
        {
            InitializeComponent();

            BindingContext = new BuyViewModel();
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
        }
    }
}