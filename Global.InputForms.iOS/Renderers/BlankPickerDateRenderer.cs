using System;
using System.Collections.Generic;
using System.ComponentModel;
using Foundation;
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
        private BlankDatePicker blankPicker;
        private string _oldText;

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;

            //Control.ShouldEndEditing += textField =>
            //{
            //    var seletedDate = textField;
            //    var text = seletedDate.Text;
            //    if (text == bPicker.Placeholder) Control.Text = DateTime.Now.ToString(bPicker.Format);
            //    return true;
            //};

            if (Control is UITextField textField)
            {
                if (!string.IsNullOrEmpty(Control.Text))
                    Control.Text = string.Empty;
                //bPicker.Text = Control.Text;
                textField.EditingChanged += (sender, arg)
                =>
                {
                    bPicker.Text = Control.Text;
                };
                textField.EditingDidEnd += (sender, arg) =>
                {
                    var controlText = Control.Text ?? string.Empty;
                    var entryText = bPicker.Text ?? string.Empty;
                    if (controlText != entryText)
                    {
                        bPicker.Text = Control.Text;
                    }
                };

                SetPlaceholder();
                SetAlignment();
                SetUIButtons();
            }

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetPlaceholder();

            if (e.PropertyName == nameof(BlankPicker.HorizontalTextAlignment)) SetAlignment();

            if (e.PropertyName == nameof(BlankPicker.Text)) UpdateText();
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

        void UpdateText()
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (Control.Text != blankPicker.Text)
                Control.Text = blankPicker.Text;
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
                    (s, ev) =>
                    {
                        blankPicker.Text = string.Empty;
                        Control.ResignFirstResponder();
                    });
                cancelButton.Clicked += (sender, e) =>
                {
                    blankPicker.SendCancelClicked();
                };
                items.Add(cancelButton);
            }

            var flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            items.Add(flexible);

            if (!string.IsNullOrEmpty(blankPicker.DoneButtonText))
            {
                var doneButton = new UIBarButtonItem(blankPicker.DoneButtonText, UIBarButtonItemStyle.Done,
                    (s, ev) =>
                    {
                        blankPicker.Text = Control.Text;
                        Control.ResignFirstResponder();
                    });
                doneButton.Clicked += (sender, e) =>
                {
                    blankPicker.SendDoneClicked();
                };
                items.Add(doneButton);
            }

            toolbar.SetItems(items.ToArray(), true);
            Control.InputAccessoryView = toolbar;
        }
    }
}