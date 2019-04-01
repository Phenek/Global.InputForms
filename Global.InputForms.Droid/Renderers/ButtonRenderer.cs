using Android.Content;
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