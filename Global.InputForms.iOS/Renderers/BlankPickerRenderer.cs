using System.Collections.Generic;
using System.ComponentModel;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankPicker), typeof(BlankPickerRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class BlankPickerRenderer : PickerRenderer
    {
        private BlankPicker blankPicker;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankPicker bPicker)) return;
            blankPicker = bPicker;
            if (Control == null)
                SetNativeControl(new UITextField
                {
                    RightViewMode = UITextFieldViewMode.Always,
                    ClearButtonMode = UITextFieldViewMode.WhileEditing
                });
            SetUIButtons();
            SetPlaceholder();
            SetAlignment();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetPlaceholder();

            if (e.PropertyName == nameof(BlankPicker.HorizontalTextAlignment)) SetAlignment();
        }

        private void SetPlaceholder()
        {
            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                if (Element is BlankPicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                    Control.Text = picker.Placeholder;
            }
        }

        private void SetAlignment()
        {
            switch (((BlankPicker) Element).HorizontalTextAlignment)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        public void SetUIButtons()
        {
            if (string.IsNullOrEmpty(blankPicker.DoneButtonText) && string.IsNullOrEmpty(blankPicker.CancelButtonText))
            {
                Control.InputAccessoryView = null;
                return;
            }

            var toolbar = new UIToolbar
            {
                BarStyle = UIBarStyle.Default,
                Translucent = true
            };
            toolbar.SizeToFit();

            var items = new List<UIBarButtonItem>();

            if (!string.IsNullOrEmpty(blankPicker.CancelButtonText))
            {
                var cancelButton = new UIBarButtonItem(blankPicker.CancelButtonText, UIBarButtonItemStyle.Done,
                    (s, ev) => { Control.ResignFirstResponder(); });
                cancelButton.Clicked += (sender, e) => { blankPicker.SendCancelClicked(); };
                items.Add(cancelButton);
            }

            var flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            items.Add(flexible);

            if (!string.IsNullOrEmpty(blankPicker.DoneButtonText))
            {
                var doneButton = new UIBarButtonItem(blankPicker.DoneButtonText, UIBarButtonItemStyle.Done,
                    (s, ev) => { Control.ResignFirstResponder(); });
                doneButton.Clicked += (sender, e) => { blankPicker.SendDoneClicked(); };
                items.Add(doneButton);
            }

            toolbar.SetItems(items.ToArray(), true);
            Control.InputAccessoryView = toolbar;
        }
    }
}