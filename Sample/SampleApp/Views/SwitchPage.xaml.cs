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
                //BackgroundColor = Color.Green,
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
            switchTest.LeftView = _labelLeft;
            switchTest.Content = _label;
            switchTest.BackgroundContent = new Label() { Text = " BackgroundContent !", VerticalOptions = LayoutOptions.Center };

            switchTest2.RightView = _label2;
            switchTest2.LeftView = _labelLeft2;
            switchTest2.Content = _label2;

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
            Console.WriteLine(((Global.InputForms.Switch)sender).IsToggled);
        }

        private void ToggledEvent2(object sender, ToggledEventArgs e)
        {
            switchTest2.BackgroundContent = new Label()
            {
                Text = _ToggleChangedCatched + " Toggle changed : " + _ToggleChangedCatched,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 10
            };
            _ToggleChangedCatched++;
        }
    }
}
