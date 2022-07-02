using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Global.InputForms.Handlers
{
    public partial class BlankEntryHandler : EntryHandler
    {
        //public BlankEntryHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //    Console.WriteLine("test");
        //}

        protected override MauiTextField CreatePlatformView()
        {
            var platformView = base.CreatePlatformView();

            platformView.BorderStyle = UITextBorderStyle.None;

            return platformView;
        }

        protected override void ConnectHandler(MauiTextField platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(MauiTextField platformView)
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