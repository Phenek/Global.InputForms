using System;
using System.Collections.Generic;
using System.ComponentModel;
using Foundation;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankDatePickerRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class BlankDatePickerRenderer : EntryRenderer
    {
        private bool _disposed;

        private UIDatePicker _picker;
        private BlankDatePicker blankPicker;
        private bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;

            if (Control != null)
            {
                Control.SpellCheckingType = UITextSpellCheckingType.No;
                Control.AutocorrectionType = UITextAutocorrectionType.No;
                Control.AutocapitalizationType = UITextAutocapitalizationType.None;
                Control.BorderStyle = UITextBorderStyle.RoundedRect;
                Control.AccessibilityTraits = UIAccessibilityTrait.Button;
                UIMenuController.SharedMenuController.MenuVisible = false;

                Control.EditingDidBegin += OnStarted;
                Control.EditingDidEnd += OnEnded;

                if (IsiOS9OrNewer)
                {
                    Control.InputAssistantItem.LeadingBarButtonGroups = null;
                    Control.InputAssistantItem.TrailingBarButtonGroups = null;
                }

                _picker = new UIDatePicker {Mode = UIDatePickerMode.Date, TimeZone = new NSTimeZone("UTC")};
                _picker.ValueChanged += HandleValueChanged;

                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 2))
                {
                    _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
                }

                Control.InputView = _picker;
                Control.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

                SetInputAccessoryView();
                UpdateDate();
                UpdateMaximumDate();
                UpdateMinimumDate();
                SetAttributes();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(BlankDatePicker.Date)
                || e.PropertyName == nameof(BlankDatePicker.Format))
                UpdateDate();
            else if (e.PropertyName == nameof(BlankDatePicker.MinimumDate)) UpdateMinimumDate();

            else if (e.PropertyName == nameof(BlankDatePicker.MaximumDate)) UpdateMaximumDate();
        }


        private void UpdateDate()
        {
            if (blankPicker.DateSet)
            {
                blankPicker.Text = Control.Text = blankPicker.Date.Date.ToString(blankPicker.Format);
                if (_picker.Date.ToDateTime().Date != blankPicker.Date.Date)
                    _picker.SetDate(blankPicker.Date.Date.ToNSDate(), false);
            }
            else
            {
                blankPicker.Text = Control.Text = string.Empty;
            }
        }

        private void SetAttributes()
        {
            if (Control != null) Control.BorderStyle = UITextBorderStyle.None;
        }

        public void SetInputAccessoryView()
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
                cancelButton.Clicked += (sender, e) => blankPicker.SendCancelClicked();
                items.Add(cancelButton);
            }

            var flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            items.Add(flexible);

            if (!string.IsNullOrEmpty(blankPicker.DoneButtonText))
            {
                var doneButton = new UIBarButtonItem(blankPicker.DoneButtonText, UIBarButtonItemStyle.Done,
                    (s, ev) =>
                    {
                        blankPicker.Text = Control.Text = _picker.Date.ToDateTime().Date.ToString(blankPicker.Format);
                        blankPicker.Date = _picker.Date.ToDateTime().Date;
                        if (blankPicker != null)
                            blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                        Control.ResignFirstResponder();
                    });
                doneButton.Clicked += (sender, e) => blankPicker.SendDoneClicked();
                items.Add(doneButton);
            }
            toolbar.SetItems(items.ToArray(), true);
            Control.InputAccessoryView = toolbar;
            Control.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            if (blankPicker.UpdateMode == UpdateMode.Immediately)
            {
                blankPicker.Text = Control.Text = _picker.Date.ToDateTime().Date.ToString(blankPicker.Format);
                blankPicker.Date = _picker.Date.ToDateTime().Date;
            }
        }

        private void OnStarted(object sender, EventArgs eventArgs)
        {
            if (blankPicker != null)
                blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        private void OnEnded(object sender, EventArgs eventArgs)
        {
            if (blankPicker != null)
                blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        private void UpdateMaximumDate()
        {
            _picker.MaximumDate = blankPicker.MaximumDate.ToNSDate();
        }

        private void UpdateMinimumDate()
        {
            _picker.MinimumDate = blankPicker.MinimumDate.ToNSDate();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                if (_picker != null)
                {
                    _picker.RemoveFromSuperview();
                    _picker.ValueChanged -= HandleValueChanged;
                    _picker.Dispose();
                    _picker = null;
                }

                if (Control != null) Control.EditingDidBegin -= OnStarted;
            }

            base.Dispose(disposing);
        }

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            NSOperationQueue.MainQueue.AddOperation(() =>
            {
                UIMenuController.SharedMenuController.SetMenuVisible(false, false);
            });
            return base.CanPerform(action, withSender);
        }
    }
}