using System.ComponentModel;
using Android.Content;
using Android.Views;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(BlankEntry), typeof(BlankEntryRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankEntryRenderer : EntryRenderer, ActionMode.ICallback
    {
        new BlankEntry Element => (BlankEntry)base.Element;

        public BlankEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.CustomSelectionActionModeCallback = this;
            }

            SetAttributes();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.IsEnabled)) SetAttributes();

            if (e.PropertyName == nameof(BlankEntry.IsClipBoardMenuVisible))
            {
                Control.LongClickable = Element.IsClipBoardMenuVisible;
            }
        }

        private void SetAttributes()
        {
            Control.SetBackgroundColor(Color.Transparent);

            /*
            if (Control.Enabled == false)
            {
                Control.SetHintTextColor(((BlankEntry)Element).PlaceholderColor.ToAndroid());
                Control.SetTextColor(((BlankEntry)Element).TextColor.ToAndroid());
            }
            */
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            return Element.IsClipBoardMenuVisible;
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            return Element.IsClipBoardMenuVisible;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return Element.IsClipBoardMenuVisible;
        }
    }
}