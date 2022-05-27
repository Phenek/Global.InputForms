using Android.Views;
using Google.Android.Material.Button;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace Global.InputForms.Handlers
{
    public class BlankButtonHandler : ButtonHandler
    {
        ButtonTouchListener onTouchListener { get; } = new ButtonTouchListener();

        //public BlankButtonHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override MaterialButton CreatePlatformView()
        {
            var platformView = base.CreatePlatformView();
            return platformView;
        }

        protected override void ConnectHandler(MaterialButton platformView)
        {
            base.ConnectHandler(platformView);

            onTouchListener.Handler = this;
            platformView.SetOnTouchListener(onTouchListener);
        }

        protected override void DisconnectHandler(MaterialButton platformView)
        {
            base.DisconnectHandler(platformView);

            onTouchListener.Handler = null;
            platformView.SetOnTouchListener(null);
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }

        protected virtual bool OnTouch(IButton button, AView v, MotionEvent e)
        {
            switch (e?.ActionMasked)
            {
                case MotionEventActions.Down:
                    button?.Pressed();
                    break;
                case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    button?.Released();
                    break;
            }

            return false;
        }

        class ButtonTouchListener : Java.Lang.Object, AView.IOnTouchListener
        {
            public BlankButtonHandler Handler { get; set; }

            public bool OnTouch(AView v, MotionEvent e) => Handler.OnTouch(Handler?.VirtualView, v, e);
        }
    }
}