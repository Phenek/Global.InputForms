using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        UIDatePicker _picker;
        UIColor _defaultTextColor;
        bool _disposed;
        bool _useLegacyColorManagement;
        bool _doneClicked;

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;

            if (Control == null)
            {
                var entry = new MyGlobalTextField();

                SetNativeControl(entry);

                entry.EditingDidBegin += OnStarted;
                entry.EditingDidEnd += OnEnded;

                _picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new NSTimeZone("UTC") };

                _picker.ValueChanged += HandleValueChanged;

                SetInputAccessoryView();

                entry.InputView = _picker;

                entry.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                entry.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

                entry.InputAssistantItem.LeadingBarButtonGroups = null;
                entry.InputAssistantItem.TrailingBarButtonGroups = null;

                _defaultTextColor = entry.TextColor;

                entry.AccessibilityTraits = UIAccessibilityTrait.Button;
            }

            UpdateDateFromModel(false);
            UpdateFont();
            UpdateMaximumDate();
            UpdateMinimumDate();
            UpdateTextColor();
            SetPlaceholder();
        }

        private void RemoveEvents(EventHandler eventHandler)
        {
            foreach (Delegate d in eventHandler.GetInvocationList())
            {
                eventHandler -= (EventHandler)d;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName)
            {
                UpdateDateFromModel(true);
            }
            else if (e.PropertyName == nameof(DatePicker.MinimumDate)) UpdateMinimumDate();

            else if (e.PropertyName == nameof(DatePicker.MaximumDate)) UpdateMaximumDate();

            else if (e.PropertyName == nameof(DatePicker.TextColor)
                || e.PropertyName == nameof(VisualElement.IsEnabled)) UpdateTextColor();

            else if (e.PropertyName == nameof(DatePicker.FontAttributes)
                || e.PropertyName == nameof(DatePicker.FontFamily)
                || e.PropertyName == nameof(DatePicker.FontSize)) UpdateFont();

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
                        _doneClicked = true;
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
            blankPicker?.SetValueFromRenderer(DatePicker.DateProperty, _picker.Date.ToDateTime().Date);
        }


        void OnStarted(object sender, EventArgs eventArgs)
        {
            blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        void OnEnded(object sender, EventArgs eventArgs)
        {
            if (_doneClicked)
            {
                blankPicker.Text = Control.Text = _picker.Date.ToDateTime().Date.ToString(blankPicker.Format);
                blankPicker.Date = _picker.Date.ToDateTime().Date;
            }
            blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        void UpdateDateFromModel(bool animate)
        {
            if (_picker.Date.ToDateTime().Date != Element.Date.Date)
                _picker.SetDate(Element.Date.ToNSDate(), animate);
        }

        protected internal virtual void UpdateFont()
        {
            Control.Font = UIFont.FromName(Element.FontFamily, (float)Element.FontSize);
        }

        void UpdateMaximumDate()
        {
            _picker.MaximumDate = Element.MaximumDate.ToNSDate();
        }

        void UpdateMinimumDate()
        {
            _picker.MinimumDate = Element.MinimumDate.ToNSDate();
        }

        protected internal virtual void UpdateTextColor()
        {
            var textColor = Element.TextColor;

            if (textColor.IsDefault || (!Element.IsEnabled && _useLegacyColorManagement))
                Control.TextColor = _defaultTextColor;
            else
                Control.TextColor = textColor.ToUIColor();

            // HACK This forces the color to update; there's probably a more elegant way to make this happen
            Control.Text = Control.Text;
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
    }

    public class MyGlobalTextField : UITextField
    {
        public MyGlobalTextField() : base(new RectangleF())
        {
            SpellCheckingType = UITextSpellCheckingType.No;
            AutocorrectionType = UITextAutocorrectionType.No;
            AutocapitalizationType = UITextAutocapitalizationType.None;
            BorderStyle = UITextBorderStyle.RoundedRect;
        }

        public override bool CanPerform(ObjCRuntime.Selector action, Foundation.NSObject withSender)
        {
           return false;
        }
    }
}