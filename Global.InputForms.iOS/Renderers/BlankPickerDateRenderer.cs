using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using Foundation;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankDatePickerRenderer))]
namespace Global.InputForms.iOS.Renderers
{
    public class BlankDatePickerRenderer : EntryRenderer
    {
        private BlankDatePicker blankPicker;
        private string _oldText;

        UIDatePicker _picker;
        UIColor _defaultTextColor;
        bool _disposed;
        bool _useLegacyColorManagement;

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

                Control.EditingDidBegin += OnStarted;
                Control.EditingDidEnd += OnEnded;

                _picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new NSTimeZone("UTC") };

                _picker.ValueChanged += HandleValueChanged;

                SetInputAccessoryView();

                Control.InputView = _picker;

                Control.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                Control.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

                Control.InputAssistantItem.LeadingBarButtonGroups = null;
                Control.InputAssistantItem.TrailingBarButtonGroups = null;

                _defaultTextColor = Control.TextColor;

                Control.AccessibilityTraits = UIAccessibilityTrait.Button;
            }

            UpdateDateFromModel(false);
            UpdateMaximumDate();
            UpdateMinimumDate();
            SetAttributes();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(BlankDatePicker.Date)
                || e.PropertyName == nameof(BlankDatePicker.Format))
                UpdateDateFromModel(true);
            else if (e.PropertyName == nameof(BlankDatePicker.MinimumDate)) UpdateMinimumDate();

            else if (e.PropertyName == nameof(BlankDatePicker.MaximumDate)) UpdateMaximumDate();
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
                    (s, ev) =>
                    {
                        Control.ResignFirstResponder();
                    });
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
                        blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                        Control.ResignFirstResponder();
                    });
                doneButton.Clicked += (sender, e) => blankPicker.SendDoneClicked();
                items.Add(doneButton);
            }

            toolbar.SetItems(items.ToArray(), true);
            Control.InputAccessoryView = toolbar;
        }

        void HandleValueChanged(object sender, EventArgs e)
        {
            blankPicker?.SetValueFromRenderer(BlankDatePicker.DateProperty, _picker.Date.ToDateTime().Date);
        }

        void OnStarted(object sender, EventArgs eventArgs)
        {
            blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        void OnEnded(object sender, EventArgs eventArgs)
        {
            blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        void UpdateDateFromModel(bool animate)
        {
            if (_picker.Date.ToDateTime().Date != blankPicker.Date.Date)
                _picker.SetDate(blankPicker.Date.ToNSDate(), animate);
        }
        
        void UpdateMaximumDate()
        {
            _picker.MaximumDate = blankPicker.MaximumDate.ToNSDate();
        }

        void UpdateMinimumDate()
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
                _defaultTextColor = null;

                if (_picker != null)
                {
                    _picker.RemoveFromSuperview();
                    _picker.ValueChanged -= HandleValueChanged;
                    _picker.Dispose();
                    _picker = null;
                }

                if (Control != null)
                {
                    Control.EditingDidBegin -= OnStarted;
                }
            }

            base.Dispose(disposing);
        }

        public override bool CanPerform(ObjCRuntime.Selector action, Foundation.NSObject withSender)
        {
            return false;
        }
    }
}