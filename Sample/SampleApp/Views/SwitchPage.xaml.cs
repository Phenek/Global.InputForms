using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwitchPage : ContentPage
    {
        int _ToggleChangedCatched;
        public SwitchPage()
        {
            _ToggleChangedCatched = 0;

            Label _label = new Label
            {
                Text = "Harrison",
                BackgroundColor = Color.YellowGreen,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            Label _labelLeft = new Label
            {
                Text = "Sportes",
                BackgroundColor = Color.YellowGreen,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            Label _label2 = new Label
            {
                Text = "Harrison",
                BackgroundColor = Color.Green,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            Label _labelLeft2 = new Label
            {
                Text = "Sportes",
                BackgroundColor = Color.YellowGreen,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            Label _label3 = new Label
            {
                Text = "Harrison",
                BackgroundColor = Color.Green,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            Label _labelLeft3 = new Label
            {
                Text = "Sportes",
                BackgroundColor = Color.YellowGreen,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            Label _label4 = new Label
            {
                Text = "Harrison",
                BackgroundColor = Color.Green,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            Label _labelLeft4 = new Label
            {
                Text = "Sportes",
                BackgroundColor = Color.YellowGreen,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };



            InitializeComponent();

            switchTest.RightView = _label;
            switchTest.RightView.HorizontalOptions = LayoutOptions.Center;
            switchTest.LeftView = _labelLeft;
            switchTest.LeftView.HorizontalOptions = LayoutOptions.Center;
            switchTest.Content = _label;
            switchTest.BackgroundContent = new Label() { Text = " BackgroundContent !", VerticalOptions = LayoutOptions.Center };
            switchTest.CurrentPanGesture += ProgressRateEvent;
            switchTest.CompletedPanGesture += ReleaseSwitch;

            Image _rightImage = new Image() { Source = "BoxChecked", Margin = 5 };
            Image _leftImage = new Image() { Source = "RadioButtonChecked", Margin = 5 };
            switchTest2.RightView = _rightImage;
            switchTest2.LeftView = _leftImage;
            switchTest2.SwitchColor = Color.FromHex("#FFFFFF");
            switchTest2.BackgroundColor = Color.FromHex("#236AA6");
            switchTest2.IconSwitch.BorderColor = Color.Gray;
            switchTest2.Content = _rightImage;
            switchTest2.CurrentPanGesture += ProgressRateEvent2;
            switchTest2.CompletedPanGesture += ReleaseSwitch2;

            switchTest3.RightView = _label3;
            switchTest3.LeftView = _labelLeft3;
            switchTest3.Content = _label3;

            switchTest4.RightView = _label4;
            switchTest4.LeftView = _labelLeft4;
            switchTest4.Content = _label4;
        }

        private void ToggledEvent(object sender, ToggledEventArgs e)
        {
            //switchTest2.BackgroundContent = new Label() { Text = "Toggle changed : " + _ToggleChangedCatched, VerticalOptions = LayoutOptions.Center };
            //_ToggleChangedCatched++;
        }

        private void ToggledEvent3(object sender, ToggledEventArgs e)
        {
            switchTest3.BackgroundContent = new Label()
            {
                Text = _ToggleChangedCatched + " Toggle changed : " + _ToggleChangedCatched,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 10
            };
            _ToggleChangedCatched++;
        }

        private void ProgressRateEvent(object sender, PanUpdatedEventArgs e)
        {
            Console.WriteLine(switchTest.ProgressRate);
            if (switchTest.IsToggled)
            {  
                if (switchTest.ProgressRate > 0.5)
                {
                    switchTest.LeftView.Opacity = 2 * switchTest.ProgressRate - 1;
                    switchTest.Content = switchTest.LeftView;
                }
                else
                {
                    switchTest.Content = switchTest.RightView;
                    switchTest.RightView.Opacity = (1 - 2 * switchTest.ProgressRate);
                }
            }
            else
            {
                if (switchTest.ProgressRate < 0.5)
                {
                    switchTest.Content = switchTest.RightView;
                    switchTest.RightView.Opacity = 1 - 2 * switchTest.ProgressRate;
                }
                else
                {
                    switchTest.Content = switchTest.LeftView;
                    switchTest.LeftView.Opacity = 2 * switchTest.ProgressRate - 1;
                }
            }
        }

        private void ProgressRateEvent2(object sender, PanUpdatedEventArgs e)
        {
            Console.WriteLine(switchTest2.ProgressRate);
            if (switchTest2.IsToggled)
            {
                if (switchTest2.ProgressRate > 0.5)
                {
                    switchTest2.LeftView.Opacity = 2 * switchTest2.ProgressRate - 1;
                    switchTest2.Content = switchTest2.LeftView;
                }
                else
                {
                    switchTest2.Content = switchTest2.RightView;
                    switchTest2.RightView.Opacity = (1 - 2 * switchTest2.ProgressRate);
                }
            }
            else
            {
                if (switchTest2.ProgressRate < 0.5)
                {
                    switchTest2.Content = switchTest2.RightView;
                    switchTest2.RightView.Opacity = 1 - 2 * switchTest2.ProgressRate;
                }
                else
                {
                    switchTest2.Content = switchTest2.LeftView;
                    switchTest2.LeftView.Opacity = 2 * switchTest2.ProgressRate - 1;
                }
            }
        }

        private void ReleaseSwitch(object sender, PanUpdatedEventArgs e)
        {
            switchTest.LeftView.Opacity = 1;
            switchTest.RightView.Opacity = 1;
            switchTest.Content.Opacity = 1;
        }

        private void ReleaseSwitch2(object sender, PanUpdatedEventArgs e)
        {
            switchTest2.LeftView.Opacity = 1;
            switchTest2.RightView.Opacity = 1;
            switchTest2.Content.Opacity = 1;
        }
    }
}
