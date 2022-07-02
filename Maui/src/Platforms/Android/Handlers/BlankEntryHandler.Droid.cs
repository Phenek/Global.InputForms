using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.Button;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace Global.InputForms.Handlers
{
    public partial class BlankEntryHandler : EntryHandler
    {
        //public BlankEntryHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override AppCompatEditText CreatePlatformView()
        {
            var platformView = base.CreatePlatformView();
            platformView.SetBackgroundColor(Colors.Transparent.ToAndroid());
            platformView.SetPadding(0, 7, 0, 3);
            return platformView;
        }

        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(AppCompatEditText platformView)
        {
            base.DisconnectHandler(platformView);
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }
    }
}