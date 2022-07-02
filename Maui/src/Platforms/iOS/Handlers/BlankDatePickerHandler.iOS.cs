using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Global.InputForms.iOS.Extensions;
using Foundation;

namespace Global.InputForms.Handlers
{
    public partial class BlankDatePickerHandler : EntryHandler
    {
        BlankDatePicker _virtualView;
        MauiTextField _platformView;
        UIDatePicker _picker;

        private bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

        //public BlankDatePickerHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override MauiTextField CreatePlatformView()
        {
            _platformView = base.CreatePlatformView();
            _virtualView = (BlankDatePicker)VirtualView;

            _platformView.BorderStyle = UITextBorderStyle.None;
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

            _picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new NSTimeZone("UTC") };
            _picker.ValueChanged += HandleValueChanged;

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 2))
            {
                _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            }

            _platformView.InputView = _picker;
            _platformView.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

            SetInputAccessoryView();
            UpdateDate();
            UpdateMaximumDate();
            UpdateMinimumDate();

            return _platformView;
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
                _picker.ValueChanged -= HandleValueChanged;
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

        private void UpdateDate()
        {
            if (_virtualView.DateSet)
            {
                _virtualView.Text = _platformView.Text = _virtualView.Date.Date.ToString(_virtualView.Format);
                if (_picker.Date.ToGlobalDateTime().Date != _virtualView.Date.Date)
                    _picker.SetDate(_virtualView.Date.Date.ToGlobalNSDate(), false);
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
                cancelButton.Clicked += (sender, e) => _virtualView.SendCancelClicked();
                items.Add(cancelButton);
            }

            var flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            items.Add(flexible);

            if (!string.IsNullOrEmpty(_virtualView.DoneButtonText))
            {
                var doneButton = new UIBarButtonItem(_virtualView.DoneButtonText, UIBarButtonItemStyle.Done,
                    (s, ev) =>
                    {
                        _virtualView.Text = _platformView.Text = _picker.Date.ToGlobalDateTime().Date.ToString(_virtualView.Format);
                        _virtualView.Date = _picker.Date.ToGlobalDateTime().Date;
                        if (_virtualView != null)
                            _virtualView.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                        _platformView.ResignFirstResponder();
                    });
                doneButton.Clicked += (sender, e) => _virtualView.SendDoneClicked();
                items.Add(doneButton);
            }
            toolbar.SetItems(items.ToArray(), true);
            _platformView.InputAccessoryView = toolbar;
            _platformView.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            if (_virtualView.UpdateMode == UpdateMode.Immediately)
            {
                _virtualView.Text = _platformView.Text = _picker.Date.ToGlobalDateTime().Date.ToString(_virtualView.Format);
                _virtualView.Date = _picker.Date.ToGlobalDateTime().Date;
            }
        }

        private void OnStarted(object sender, EventArgs eventArgs)
        {
            if (_virtualView != null)
                _virtualView.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        private void OnEnded(object sender, EventArgs eventArgs)
        {
            if (_virtualView != null)
                _virtualView.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        private void UpdateMaximumDate()
        {
            _picker.MaximumDate = _virtualView.MaximumDate.ToGlobalNSDate();
        }

        private void UpdateMinimumDate()
        {
            _picker.MinimumDate = _virtualView.MinimumDate.ToGlobalNSDate();
        }
    }
}