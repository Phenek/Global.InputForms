using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Global.InputForms.iOS.Extensions;
using Foundation;

namespace Global.InputForms.Handlers
{
    public class BlankTimePickerHandler : EntryHandler
    {
        BlankTimePicker _virtualView;
        MauiTextField _platformView;
        UIDatePicker _picker;

        private bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

        //public BlankTimePickerHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override MauiTextField CreatePlatformView()
        {
            var platformView = base.CreatePlatformView();

            platformView.BorderStyle = UITextBorderStyle.None;
            _platformView.SpellCheckingType = UITextSpellCheckingType.No;
            _platformView.AutocorrectionType = UITextAutocorrectionType.No;
            _platformView.AutocapitalizationType = UITextAutocapitalizationType.None;
            _platformView.BorderStyle = UITextBorderStyle.RoundedRect;
            _platformView.AccessibilityTraits = UIAccessibilityTrait.Button;
            UIMenuController.SharedMenuController.MenuVisible = false;

            _platformView.EditingDidBegin += OnStarted;
            _platformView.EditingDidEnd += OnEnded;

            if (IsiOS9OrNewer)
            {
                _platformView.InputAssistantItem.LeadingBarButtonGroups = null;
                _platformView.InputAssistantItem.TrailingBarButtonGroups = null;
            }

            _picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };
            _picker.ValueChanged += OnValueChanged;
            _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            _platformView.InputView = _picker;
            _platformView.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

            SetInputAccessoryView();
            UpdateTime();


            return platformView;
        }

        protected override void ConnectHandler(MauiTextField platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(MauiTextField platformView)
        {
            base.DisconnectHandler(platformView);

            if (_picker != null)
            {
                _picker.RemoveFromSuperview();
                _picker.ValueChanged -= OnValueChanged;
                _picker.Dispose();
                _picker = null;
            }

            if (_platformView != null)
            {
                _platformView.EditingDidBegin -= OnStarted;
                _platformView.EditingDidEnd -= OnEnded;
            }
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }

        private void UpdateTime()
        {
            if (_virtualView.TimeSet)
            {
                _picker.Date = new DateTime(_virtualView.Time.Ticks).ToGlobalNSDate();
                _virtualView.Text = _platformView.Text = new DateTime(_virtualView.Time.Ticks).ToString(_virtualView.Format);
            }
            else
            {
                _virtualView.Text = _platformView.Text = string.Empty;
            }
        }

        public void SetInputAccessoryView()
        {
            if (string.IsNullOrEmpty(_virtualView.DoneButtonText) && string.IsNullOrEmpty(_virtualView.CancelButtonText))
            {
                _platformView.InputAccessoryView = null;
                return;
            }

            var toolbar = new UIToolbar
            {
                BarStyle = UIBarStyle.Default,
                Translucent = true
            };
            toolbar.SizeToFit();

            var items = new List<UIBarButtonItem>();

            if (!string.IsNullOrEmpty(_virtualView.CancelButtonText))
            {
                var cancelButton = new UIBarButtonItem(_virtualView.CancelButtonText, UIBarButtonItemStyle.Done,
                    (s, ev) => { _platformView.ResignFirstResponder(); });
                cancelButton.Clicked += (sender, e) => { _virtualView.SendCancelClicked(); };
                items.Add(cancelButton);
            }

            var flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            items.Add(flexible);

            if (!string.IsNullOrEmpty(_virtualView.DoneButtonText))
            {
                var doneButton = new UIBarButtonItem(_virtualView.DoneButtonText, UIBarButtonItemStyle.Done,
                    (s, ev) =>
                    {
                        var timeOfDay = _picker.Date.ToGlobalDateTime().TimeOfDay;
                        var time = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, 0);
                        _virtualView.Text = _platformView.Text = new DateTime(time.Ticks).ToString(_virtualView.Format);
                        _virtualView.Time = time;
                        if (_virtualView != null)
                            _virtualView.SetValue(VisualElement.IsFocusedPropertyKey, false);
                        _platformView.ResignFirstResponder();
                    });
                doneButton.Clicked += (sender, e) => { _virtualView.SendDoneClicked(); };
                items.Add(doneButton);
            }
            toolbar.SetItems(items.ToArray(), true);
            _platformView.InputAccessoryView = toolbar;
            _platformView.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
        }

        private void OnEnded(object sender, EventArgs eventArgs)
        {
            if (_virtualView != null)
                _virtualView.SetValue(VisualElement.IsFocusedPropertyKey, false);
        }

        private void OnStarted(object sender, EventArgs eventArgs)
        {
            if (_virtualView != null)
                _virtualView.SetValue(VisualElement.IsFocusedPropertyKey, true);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_virtualView.UpdateMode == UpdateMode.Immediately)
            {
                var timeOfDay = _picker.Date.ToGlobalDateTime().TimeOfDay;
                var time = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, 0);
                _virtualView.Text = _platformView.Text = new DateTime(time.Ticks).ToString(_virtualView.Format);
                _virtualView.Time = time;
            }
        }
    }
}