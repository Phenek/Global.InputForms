using UIKit;
using Microsoft.Maui.Handlers;

namespace Global.InputForms.Handlers
{
    public partial class BlankButtonHandler : ButtonHandler
    {
        //public BlankButtonHandler(IPropertyMapper mapper): base(mapper)
        //{
        //}

        protected override UIButton CreatePlatformView()
        {
            var platformView = base.CreatePlatformView();
            return platformView;
        }

        protected override void ConnectHandler(UIButton platformView)
        {
            base.ConnectHandler(platformView);

            platformView.TouchCancel += SendReleased;
            platformView.TouchDragExit += SendReleased;
            platformView.TouchDragEnter += SendPressed;
        }

        protected override void DisconnectHandler(UIButton platformView)
        {
            base.DisconnectHandler(platformView);

            platformView.TouchCancel -= SendReleased;
            platformView.TouchDragExit -= SendReleased;
            platformView.TouchDragEnter -= SendPressed;
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }

        void SendReleased(object sender, EventArgs e)
        {
            VirtualView?.Released();
        }

        void SendPressed(object sender, EventArgs e)
        {
            VirtualView?.Pressed();
        }
    }
}