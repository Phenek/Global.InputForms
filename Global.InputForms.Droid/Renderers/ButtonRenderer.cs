using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BlankButton), typeof(CustomButtonRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null && e.NewElement != null)
            {
                if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
                {
                    this.Control.StateListAnimator = null;
                }
                else
                {
                    this.Control.Elevation = 0;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(VisualElement.BackgroundColorProperty.PropertyName) ||
                e.PropertyName.Equals(Button.CornerRadiusProperty.PropertyName) ||
                e.PropertyName.Equals(Button.BorderWidthProperty.PropertyName))
            {
                this.UpdateBackground();
            }
        }

        private void UpdateBackground()
        {
            if (this.Element != null)
            {
                using (var background = new GradientDrawable())
                {
                    background.SetColor(Element.BackgroundColor.ToAndroid());
                    background.SetStroke((int)Context.ToPixels(Element.BorderWidth), Element.BorderColor.ToAndroid());
                    background.SetCornerRadius(Context.ToPixels(Element.CornerRadius));

                    // customize the button states as necessary
                    using (var backgroundStates = new StateListDrawable())
                    {
                        backgroundStates.AddState(new int[] { }, background);

                        this.Control.SetBackground(backgroundStates);
                    }
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