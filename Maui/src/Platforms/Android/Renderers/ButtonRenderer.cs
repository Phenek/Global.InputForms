using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Compatibility.Platform.Android.AppCompat;
using Microsoft.Maui.Controls.Platform;
using Global.InputForms.Droid.Extensions;

[assembly: ExportRenderer(typeof(BlankButton), typeof(CustomButtonRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Microsoft.Maui.Controls.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
                    Control.StateListAnimator = null;
                else
                    Control.Elevation = 0;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(VisualElement.BackgroundColorProperty.PropertyName) ||
                e.PropertyName.Equals(Microsoft.Maui.Controls.Button.CornerRadiusProperty.PropertyName) ||
                e.PropertyName.Equals(Microsoft.Maui.Controls.Button.BorderWidthProperty.PropertyName))
                UpdateBackgroundButton();
        }

        private void UpdateBackgroundButton()
        {
            if (Element != null)
                using (var background = new GradientDrawable())
                {
                    background.SetColor(Element.BackgroundColor.ToAndroid());
                    background.SetStroke((int) Context.ToGlobalPixels(Element.BorderWidth), Element.BorderColor.ToAndroid());
                    background.SetCornerRadius(Context.ToGlobalPixels(Element.CornerRadius));

                    // customize the button states as necessary
                    using (var backgroundStates = new StateListDrawable())
                    {
                        backgroundStates.AddState(new int[] { }, background);

                        Control.SetBackground(backgroundStates);
                    }
                }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Cancel) SendReleased();
            return base.OnTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (ev.ActionMasked == MotionEventActions.Cancel) SendReleased();
            return false;
        }

        private void SendReleased()
        {
            ((IButtonController) Element)?.SendReleased();
        }

        private void SendPressed()
        {
            ((IButtonController) Element)?.SendPressed();
        }
    }
}