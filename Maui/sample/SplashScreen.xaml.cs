using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Sample
{
    public partial class SplashScreen : ContentPage
    {
        private readonly bool _animated;
        private readonly uint _duration;
        private readonly DisplayInfo info;

        public SplashScreen(bool animated = true, uint duration = 500)
        {
            _animated = animated;
            _duration = duration;
            InitializeComponent();

            info = DeviceDisplay.MainDisplayInfo;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (_animated)
            {
                await _logo.TranslateTo(0.0, -info.Height / info.Density / 3, _duration, Easing.Linear);
            }
            Microsoft.Maui.Controls.Application.Current.MainPage = new NavigationPage(new MenuPage())
            {
                BarBackgroundColor = (Color)Microsoft.Maui.Controls.Application.Current.Resources["PrimaryColor"]
            };
        }
    }
}
