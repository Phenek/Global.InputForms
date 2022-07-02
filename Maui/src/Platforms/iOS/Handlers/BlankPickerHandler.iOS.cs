using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System.Collections.Specialized;

namespace Global.InputForms.Handlers
{
    public partial class BlankPickerHandler : EntryHandler
    {
        BlankPicker _virtualView;
        MauiTextField _platformView;
        UIPickerView _picker;

        private bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

        //public BlankPickerHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override MauiTextField CreatePlatformView()
        {
            _platformView = base.CreatePlatformView();
            _virtualView = (BlankPicker)VirtualView;

            _platformView.BorderStyle = UITextBorderStyle.None;
            _platformView.SpellCheckingType = UITextSpellCheckingType.No;
            _platformView.AutocorrectionType = UITextAutocorrectionType.No;
            _platformView.AutocapitalizationType = UITextAutocapitalizationType.None;
            _platformView.BorderStyle = UITextBorderStyle.RoundedRect;
            _platformView.AccessibilityTraits = UIAccessibilityTrait.Button;
            UIMenuController.SharedMenuController.MenuVisible = false;

            _platformView.EditingDidBegin += OnStarted;
            _platformView.Ended += OnEnded;
            _platformView.EditingChanged += OnEditing;

            if (IsiOS9OrNewer)
            {
                _platformView.InputAssistantItem.LeadingBarButtonGroups = null;
                _platformView.InputAssistantItem.TrailingBarButtonGroups = null;
            }

            _picker = new UIPickerView();
            _platformView.InputView = _picker;
            _picker.Model = new PickerSource(this);
            _platformView.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

            SetInputAccessoryView();
            UpdatePicker();

            if (_virtualView.Items is INotifyCollectionChanged Collection)
                Collection.CollectionChanged += RowsCollectionChanged;

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
                if (_picker.Model != null)
                {
                    _picker.Model.Dispose();
                    _picker.Model = null;
                }

                _picker.RemoveFromSuperview();
                _picker.Dispose();
                _picker = null;
            }

            if (_platformView != null)
            {
                _platformView.EditingDidBegin -= OnStarted;
                _platformView.EditingDidEnd -= OnEnded;
                _platformView.EditingChanged -= OnEditing;
            }

            if (_virtualView != null && _virtualView.Items is INotifyCollectionChanged colletion)
                colletion.CollectionChanged -= RowsCollectionChanged;
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
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
                    (sd, ev) =>
                    {
                        var s = (PickerSource)_picker.Model;
                        if (s.SelectedIndex == -1 && _virtualView.Items != null && _virtualView.Items.Count > 0)
                            UpdatePickerSelectedIndex(0);
                        UpdatePickerFromModel(s);
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

        private void OnEditing(object sender, EventArgs eventArgs)
        {
            // Reset the TextField's Text so it appears as if typing with a keyboard does not work.
            var selectedIndex = _virtualView.SelectedIndex;
            var items = _virtualView.Items;
            _virtualView.Text = _platformView.Text = selectedIndex == -1 || items == null ? "" : items[selectedIndex];
            // Also clears the undo stack (undo/redo possible on iPads)
            _platformView.UndoManager.RemoveAllActions();
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

        private void RowsCollectionChanged(object sender, EventArgs e)
        {
            UpdatePicker();
        }

        private void UpdatePicker()
        {
            var selectedIndex = _virtualView.SelectedIndex;
            var items = _virtualView.Items;

            var oldText = _platformView.Text;
            _virtualView.Text = _platformView.Text = selectedIndex == -1 || items == null || selectedIndex >= items.Count
                ? ""
                : items[selectedIndex];
            UpdatePickerNativeSize(oldText);
            _picker.ReloadAllComponents();
            if (items == null || items.Count == 0)
                return;

            UpdatePickerSelectedIndex(selectedIndex);
        }

        private void UpdatePickerFromModel(PickerSource s)
        {
            if (_virtualView != null)
            {
                var oldText = _platformView.Text;
                _virtualView.Text = _platformView.Text = s.SelectedItem;
                UpdatePickerNativeSize(oldText);
                if (_virtualView != null)
                    _virtualView.SetValue(BlankPicker.SelectedIndexProperty, s.SelectedIndex);
            }
        }

        private void UpdatePickerNativeSize(string oldText)
        {
            if (oldText != _platformView.Text)
                _virtualView.PlatformSizeChanged();
        }

        private void UpdatePickerSelectedIndex(int formsIndex)
        {
            var source = (PickerSource)_picker.Model;
            source.SelectedIndex = formsIndex;
            source.SelectedItem = formsIndex >= 0 ? _virtualView.Items[formsIndex] : null;
            _picker.Select(Math.Max(formsIndex, 0), 0, true);
        }

        //public override bool CanPerform(Selector action, NSObject withSender)
        //{
        //    NSOperationQueue.MainQueue.AddOperation(() =>
        //    {
        //        UIMenuController.SharedMenuController.SetMenuVisible(false, false);
        //    });
        //    return base.CanPerform(action, withSender);
        //}

        private class PickerSource : UIPickerViewModel
        {
            private bool _disposed;
            private BlankPickerHandler _handler;

            public PickerSource(BlankPickerHandler handler)
            {
                _handler = handler;
            }

            public int SelectedIndex { get; internal set; }

            public string SelectedItem { get; internal set; }

            public override nint GetComponentCount(UIPickerView picker)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return _handler._virtualView.Items != null ? _handler._virtualView.Items.Count : 0;
            }

            public override string GetTitle(UIPickerView picker, nint row, nint component)
            {
                return _handler._virtualView.Items[(int)row];
            }

            public override void Selected(UIPickerView picker, nint row, nint component)
            {
                if (_handler._virtualView.Items.Count == 0)
                {
                    SelectedItem = null;
                    SelectedIndex = -1;
                }
                else
                {
                    SelectedItem = _handler._virtualView.Items[(int)row];
                    SelectedIndex = (int)row;
                }

                if (_handler._virtualView.UpdateMode == UpdateMode.Immediately)
                    _handler.UpdatePickerFromModel(this);
            }

            protected override void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                _disposed = true;

                if (disposing)
                    _handler = null;

                base.Dispose(disposing);
            }
        }
    }
}