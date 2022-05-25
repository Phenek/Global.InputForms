using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;

namespace Sample.Views
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
                var fromSwitchColor = e.IsToggled ? Color.FromArgb("#EA5A50") : Color.FromArgb("#32B9D9");
                var toSwitchColor = e.IsToggled ? Color.FromArgb("#32B9D9") : Color.FromArgb("#EA5A50");

                //Color Animation
                var fromColor = e.IsToggled ? Color.FromArgb("#32B9D9") : Color.FromArgb("#EA5A50");
                var toColor = e.IsToggled ? Color.FromArgb("#EA5A50") : Color.FromArgb("#32B9D9");

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
                var fromColor = e.IsToggled ? Color.FromArgb("#4ACC64") : Color.FromArgb("#EBECEC");
                var toColor = e.IsToggled ? Color.FromArgb("#EBECEC") : Color.FromArgb("#4ACC64");

                var t = e.Percentage * 0.01;

                _switchIos.BackgroundColor = ColorAnimation(fromColor, toColor, t);
            };

            _switchAndroid.SwitchPanUpdate += (sender, e) =>
            {
                //Switch Color Animation
                var fromSwitchColor = e.IsToggled ? Color.FromArgb("#239385") : Colors.White;
                var toSwitchColor = e.IsToggled ? Colors.White : Color.FromArgb("#239385");

                //BackGroundColor Animation
                var fromColor = e.IsToggled ? Color.FromArgb("#A6D3CF") : Color.FromArgb("#A6A6A6");
                var toColor = e.IsToggled ? Color.FromArgb("#A6A6A6") : Color.FromArgb("#A6D3CF");

                var t = e.Percentage * 0.01;

                _switchAndroid.SwitchColor = ColorAnimation(fromSwitchColor, toSwitchColor, t);
                _switchAndroid.BackgroundColor = ColorAnimation(fromColor, toColor, t);
            };
        }


        Color ColorAnimation(Color fromColor, Color toColor, double t)
        {
            return Color.FromRgba(fromColor.Red + t * (toColor.Red - fromColor.Red),
                fromColor.Green + t * (toColor.Green - fromColor.Green),
                fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                fromColor.Red + t * (toColor.Red - fromColor.Red));
        }
    }
}