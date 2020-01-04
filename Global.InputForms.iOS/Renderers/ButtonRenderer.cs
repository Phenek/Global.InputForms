using System;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankButton), typeof(CustomButtonRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement is Button view)
            {
                
                //Control.TouchDragOutside += (sender, ea) => Console.WriteLine("Touch Drag Outside");
                //Control.TouchDragInside += (sender, ea) => Console.WriteLine("Touch Drag Inside");
                //Control.TouchDragExit += (sender, ea) => Console.WriteLine("Touch Drag Exit");
                //Control.TouchDragEnter += (sender, ea) => Console.WriteLine("Touch Drag Enter");
                //Control.TouchUpOutside += (sender, ea) => Console.WriteLine("Touch up outside");
                //Control.TouchUpInside += (sender, ea) => Console.WriteLine("Touch up inside");
                //Control.TouchCancel += (sender, ea) => Console.WriteLine("Touch Cancel");
                
                Control.TouchCancel += SendReleased;
                Control.TouchDragExit += SendReleased;
                Control.TouchDragEnter += SendPressed;


                //UpdatePadding();
            }
        }

        /*
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Padding")
                UpdatePadding();
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.TouchDragExit -= SendReleased;
                Control.TouchCancel -= SendReleased;
                Control.TouchDragEnter -= SendPressed;
            }

            base.Dispose(disposing);
        }

        private void UpdatePadding()
        {
            if (Element is BlankButton btn)
                Control.ContentEdgeInsets = new UIEdgeInsets(
                    (float) btn.Padding.Top,
                    (float) btn.Padding.Left,
                    (float) btn.Padding.Bottom,
                    (float) btn.Padding.Right);
        }

        private void SendReleased(object sender, EventArgs e)
        {
            ((IButtonController) Element)?.SendReleased();
        }

        private void SendPressed(object sender, EventArgs e)
        {
            ((IButtonController) Element)?.SendPressed();
        }
    }
}