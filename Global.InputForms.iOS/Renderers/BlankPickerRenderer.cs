using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using Foundation;
using Global.InputForms;
using Global.InputForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

[assembly: ExportRenderer(typeof(BlankPicker), typeof(BlankPickerRenderer))]

namespace Global.InputForms.iOS.Renderers
{
    public class BlankPickerRenderer : EntryRenderer
    {
        bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        public BlankPicker blankPicker;

        UIPickerView _picker;
        UIColor _defaultTextColor;
        bool _disposed;
        bool _useLegacyColorManagement;

        IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
                ((INotifyCollectionChanged)blankPicker.Items).CollectionChanged -= RowsCollectionChanged;

            if (!(e.NewElement is BlankPicker bPicker)) return;
            blankPicker = bPicker;

            if (Control != null)
            {
                Control.SpellCheckingType = UITextSpellCheckingType.No;
                Control.AutocorrectionType = UITextAutocorrectionType.No;
                Control.AutocapitalizationType = UITextAutocapitalizationType.None;
                Control.BorderStyle = UITextBorderStyle.RoundedRect;

                Control.EditingDidBegin += OnStarted;
                Control.Ended += OnEnded;
                Control.EditingChanged += OnEditing;

                _picker = new UIPickerView();

                SetInputAccessoryView();

                Control.InputView = _picker;

                Control.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                Control.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

                if (IsiOS9OrNewer)
                {
                    Control.InputAssistantItem.LeadingBarButtonGroups = null;
                    Control.InputAssistantItem.TrailingBarButtonGroups = null;
                }

                Control.AccessibilityTraits = UIAccessibilityTrait.Button;

                _picker.Model = new PickerSource(this);

                UpdatePicker();
                SetAttributes();

                ((INotifyCollectionChanged)blankPicker.Items).CollectionChanged += RowsCollectionChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(BlankPicker.SelectedIndex)) UpdatePicker();
        }

        private void SetAttributes()
        {
            if (Control != null)
                Control.BorderStyle = UITextBorderStyle.None;
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
                cancelButton.Clicked += (sender, e) => { blankPicker.SendCancelClicked(); };
                items.Add(cancelButton);
            }

            var flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            items.Add(flexible);

            if (!string.IsNullOrEmpty(blankPicker.DoneButtonText))
            {
                var doneButton = new UIBarButtonItem(blankPicker.DoneButtonText, UIBarButtonItemStyle.Done,
                    (sd, ev) =>
                    {
                        var s = (PickerSource)_picker.Model;
                        if (s.SelectedIndex == -1 && blankPicker.Items != null && blankPicker.Items.Count > 0)
                            UpdatePickerSelectedIndex(0);
                        UpdatePickerFromModel(s);

                        ElementController.SetValueFromRenderer(Xamarin.Forms.VisualElement.IsFocusedPropertyKey, false);
                        Control.ResignFirstResponder();
                    });
                doneButton.Clicked += (sender, e) => { blankPicker.SendDoneClicked(); };
                items.Add(doneButton);
            }

            toolbar.SetItems(items.ToArray(), true);
            Control.InputAccessoryView = toolbar;
        }

        void OnEditing(object sender, EventArgs eventArgs)
        {
            // Reset the TextField's Text so it appears as if typing with a keyboard does not work.
            var selectedIndex = blankPicker.SelectedIndex;
            var items = blankPicker.Items;
            blankPicker.Text = Control.Text = selectedIndex == -1 || items == null ? "" : items[selectedIndex];
            // Also clears the undo stack (undo/redo possible on iPads)
            Control.UndoManager.RemoveAllActions();
        }

        void OnEnded(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(Xamarin.Forms.VisualElement.IsFocusedPropertyKey, false);
        }

        void OnStarted(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(Xamarin.Forms.VisualElement.IsFocusedPropertyKey, true);
        }

        void RowsCollectionChanged(object sender, EventArgs e)
        {
            UpdatePicker();
        }

        void UpdatePicker()
        {
            var selectedIndex = blankPicker.SelectedIndex;
            var items = blankPicker.Items;

            var oldText = Control.Text;
            blankPicker.Text = Control.Text = selectedIndex == -1 || items == null || selectedIndex >= items.Count ? "" : items[selectedIndex];
            UpdatePickerNativeSize(oldText);
            _picker.ReloadAllComponents();
            if (items == null || items.Count == 0)
                return;

            UpdatePickerSelectedIndex(selectedIndex);
        }

        void UpdatePickerFromModel(PickerSource s)
        {
            if (Element != null)
            {
                var oldText = Control.Text;
                blankPicker.Text = Control.Text = s.SelectedItem;
                UpdatePickerNativeSize(oldText);
                ElementController.SetValueFromRenderer(BlankPicker.SelectedIndexProperty, s.SelectedIndex);
            }
        }

        void UpdatePickerNativeSize(string oldText)
        {
            if (oldText != Control.Text)
                ((IVisualElementController)Element).NativeSizeChanged();
        }

        void UpdatePickerSelectedIndex(int formsIndex)
        {
            var source = (PickerSource)_picker.Model;
            source.SelectedIndex = formsIndex;
            source.SelectedItem = formsIndex >= 0 ? blankPicker.Items[formsIndex] : null;
            _picker.Select(Math.Max(formsIndex, 0), 0, true);
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
                    if (_picker.Model != null)
                    {
                        _picker.Model.Dispose();
                        _picker.Model = null;
                    }

                    _picker.RemoveFromSuperview();
                    _picker.Dispose();
                    _picker = null;
                }

                if (Control != null)
                {
                    Control.EditingDidBegin -= OnStarted;
                    Control.EditingDidEnd -= OnEnded;
                    Control.EditingChanged -= OnEditing;
                }

                if (Element != null)
                    ((INotifyCollectionChanged)blankPicker.Items).CollectionChanged -= RowsCollectionChanged;
            }

            base.Dispose(disposing);
        }

        public override bool CanPerform(ObjCRuntime.Selector action, Foundation.NSObject withSender)
        {
            return false;
        }
    }

    class PickerSource : UIPickerViewModel
    {
        BlankPickerRenderer _renderer;
        bool _disposed;

        public PickerSource(BlankPickerRenderer renderer)
        {
            _renderer = renderer;
        }

        public int SelectedIndex { get; internal set; }

        public string SelectedItem { get; internal set; }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _renderer.blankPicker.Items != null ? _renderer.blankPicker.Items.Count : 0;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return _renderer.blankPicker.Items[(int)row];
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            if (_renderer.blankPicker.Items.Count == 0)
            {
                SelectedItem = null;
                SelectedIndex = -1;
            }
            else
            {
                SelectedItem = _renderer.blankPicker.Items[(int)row];
                SelectedIndex = (int)row;
            }

            //if (_renderer.Element.On<PlatformConfiguration.iOS>().UpdateMode() == UpdateMode.Immediately)
            //    _renderer.UpdatePickerFromModel(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
                _renderer = null;

            base.Dispose(disposing);
        }
    }
}