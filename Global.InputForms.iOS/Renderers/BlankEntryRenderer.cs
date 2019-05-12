using System.ComponentModel;
using Foundation;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using ObjCRuntime;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankEntry), typeof(BlankEntryRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class BlankEntryRenderer : EntryRenderer
    {
        new BlankEntry Element => (BlankEntry)base.Element;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            SetAttributes();

            if(Control != null)
            {
                //Control.
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.IsEnabled)) SetAttributes();
        }

        private void SetAttributes()
        {
            if (Control != null) Control.BorderStyle = UITextBorderStyle.None;
        }

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            NSOperationQueue.MainQueue.AddOperation(() => {
                UIMenuController.SharedMenuController.SetMenuVisible(Element.IsClipBoardMenuVisible, false);
            });
            return base.CanPerform(action, withSender);
        }
    }
}