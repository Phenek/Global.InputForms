using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwitchPage : ContentPage
    {
        public SwitchPage()
        {
            InitializeComponent();

            _switchNitendo.SwitchPanUpdate += (sender, e) =>
            {
                _nitendo.TranslationX = -(e.TranslateX + e.xRef);

                //Switch Color Animation
                var fromSwitchColor = e.IsToggled ? Color.FromHex("#EA5A50") : Color.FromHex("#32B9D9");
                var toSwitchColor = e.IsToggled ? Color.FromHex("#32B9D9") : Color.FromHex("#EA5A50");

                //Color Animation
                var fromColor = e.IsToggled ? Color.FromHex("#32B9D9") : Color.FromHex("#EA5A50");
                var toColor = e.IsToggled ? Color.FromHex("#EA5A50") : Color.FromHex("#32B9D9");

                var t = e.Percentage * 0.01;

                _switchNitendo.SwitchColor = ColorAnimation(fromSwitchColor, toSwitchColor, t);
                _switchNitendo.BackgroundColor = ColorAnimation(fromColor, toColor, t);
            };

            _switchGlaseEye.SwitchPanUpdate += (sender, e) =>
            {
                _flex.TranslationX = -(e.TranslateX + e.xRef);
            };

            _switchIos.SwitchPanUpdate += (sender, e) =>
            {
                //Color Animation
                var fromColor = e.IsToggled ? Color.FromHex("#4ACC64") : Color.FromHex("#EBECEC");
                var toColor = e.IsToggled ? Color.FromHex("#EBECEC") : Color.FromHex("#4ACC64");

                var t = e.Percentage * 0.01;

                _switchIos.BackgroundColor = ColorAnimation(fromColor, toColor, t);
            };

            _switchAndroid.SwitchPanUpdate += (sender, e) =>
            {
                //Switch Color Animation
                var fromSwitchColor = e.IsToggled ? Color.FromHex("#239385") : Color.White;
                var toSwitchColor = e.IsToggled ? Color.White : Color.FromHex("#239385");

                //BackGroundColor Animation
                var fromColor = e.IsToggled ? Color.FromHex("#A6D3CF") : Color.FromHex("#A6A6A6");
                var toColor = e.IsToggled ? Color.FromHex("#A6A6A6") : Color.FromHex("#A6D3CF");

                var t = e.Percentage * 0.01;

                _switchAndroid.SwitchColor = ColorAnimation(fromSwitchColor, toSwitchColor, t);
                _switchAndroid.BackgroundColor = ColorAnimation(fromColor, toColor, t);
            };
        }


        Color ColorAnimation(Color fromColor, Color toColor, double t)
        {
            return Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
                fromColor.G + t * (toColor.G - fromColor.G),
                fromColor.B + t * (toColor.B - fromColor.B),
                fromColor.A + t * (toColor.A - fromColor.A));
        }
    }
}