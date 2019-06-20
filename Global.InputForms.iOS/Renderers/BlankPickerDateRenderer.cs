using System;
using System.Collections.Generic;
using System.ComponentModel;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankPickerDateRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class BlankPickerDateRenderer : DatePickerRenderer
    {
        BlankDatePicker blankPicker;
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;

            Control.ShouldEndEditing += textField =>
            {
                var seletedDate = textField;
                var text = seletedDate.Text;
                if (text == bPicker.Placeholder) Control.Text = DateTime.Now.ToString(bPicker.Format);
                return true;
            };

            if (Control == null)
            {
                SetNativeControl(new UITextField
                {
                    RightViewMode = UITextFieldViewMode.Always,
                    ClearButtonMode = UITextFieldViewMode.WhileEditing,
                });
            }
            SetPlaceholder();
            SetUIButtons();
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
                if (Element is BlankDatePicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                    Control.Text = picker.Placeholder;
            }
        }

        private void SetAlignment()
        {
            switch (((BlankDatePicker)Element).HorizontalTextAlignment)
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
            UIToolbar toolbar = new UIToolbar
            {
                BarStyle = UIBarStyle.Default,
                Translucent = true
            };
            toolbar.SizeToFit();

            var items = new List<UIBarButtonItem>();

            if (!string.IsNullOrEmpty(blankPicker.CancelButtonText))
            {
                UIBarButtonItem cancelButton = new UIBarButtonItem(blankPicker.CancelButtonText, UIBarButtonItemStyle.Done, (s, ev) =>
                {
                    Control.ResignFirstResponder();
                });
                cancelButton.Clicked += (sender, e) => { blankPicker.SendCancelClicked(); };
                items.Add(cancelButton);
            }

            UIBarButtonItem flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            items.Add(flexible);

            if (!string.IsNullOrEmpty(blankPicker.DoneButtonText))
            {
                UIBarButtonItem doneButton = new UIBarButtonItem(blankPicker.DoneButtonText, UIBarButtonItemStyle.Done, (s, ev) =>
                {
                    Control.ResignFirstResponder();
                });
                doneButton.Clicked += (sender, e) => { blankPicker.SendDoneClicked(); };
                items.Add(doneButton);
            }

            toolbar.SetItems(items.ToArray(), true);
            Control.InputAccessoryView = toolbar;
        }
    }
}