using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
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
    public class BlankPickerRenderer : PickerRenderer
    {
        bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        private BlankPicker blankPicker;
        bool _doneClicked;

        UIPickerView _picker;
        UIColor _defaultTextColor;
        bool _disposed;
        bool _useLegacyColorManagement;

        IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        {
            if (e.OldElement != null)
                ((INotifyCollectionChanged)e.OldElement.Items).CollectionChanged -= RowsCollectionChanged;

            if (!(e.NewElement is BlankPicker bPicker)) return;
            blankPicker = bPicker;

            if (Control == null)
            {
                // disabled cut, delete, and toggle actions because they can throw an unhandled native exception
                var entry = new MyGlobalTextField();
                SetNativeControl(entry);

                entry.EditingDidBegin += OnStarted;
                entry.EditingDidEnd += OnEnded;
                entry.EditingChanged += OnEditing;

                _picker = new UIPickerView();

                SetInputAccessoryView();

                entry.InputView = _picker;

                entry.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                entry.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

                if (IsiOS9OrNewer)
                {
                    entry.InputAssistantItem.LeadingBarButtonGroups = null;
                    entry.InputAssistantItem.TrailingBarButtonGroups = null;
                }

                _defaultTextColor = entry.TextColor;


                entry.AccessibilityTraits = UIAccessibilityTrait.Button;

            }
            _picker.Model = new PickerSource(this);

            UpdateFont();
            UpdatePicker();
            UpdateTextColor();

            ((INotifyCollectionChanged)e.NewElement.Items).CollectionChanged += RowsCollectionChanged;

            if (Control is UITextField textField)
            {
                if (!string.IsNullOrEmpty(Control.Text))
                    bPicker.Text = Control.Text;

                textField.EditingChanged += (sender, arg)
                    => bPicker.Text = Control.Text;

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
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(BlankPicker.Title))UpdatePicker();

            else if (e.PropertyName == nameof(BlankPicker.SelectedIndex))UpdatePicker();

            else if (e.PropertyName == nameof(BlankPicker.TextColor)
                || e.PropertyName == nameof(BlankPicker.IsEnabled))
                UpdateTextColor();
            else if (e.PropertyName == nameof(BlankPicker.FontAttributes)
                || e.PropertyName == nameof(BlankPicker.FontFamily)
                || e.PropertyName == nameof(BlankPicker.FontSize))
                UpdateFont();

            if (e.PropertyName == nameof(BlankPicker.Placeholder)) SetPlaceholder();

            if (e.PropertyName == nameof(BlankPicker.HorizontalTextAlignment)) SetAlignment();

            if (e.PropertyName == nameof(BlankPicker.Text)) UpdateText();
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
                        _doneClicked = true;
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
            var selectedIndex = Element.SelectedIndex;
            var items = Element.Items;
            Control.Text = selectedIndex == -1 || items == null ? "" : items[selectedIndex];
            // Also clears the undo stack (undo/redo possible on iPads)
            Control.UndoManager.RemoveAllActions();
        }

        void OnEnded(object sender, EventArgs eventArgs)
        {
            var s = (PickerSource)_picker.Model;
            //if (s.SelectedIndex != -1 && s.SelectedIndex != _picker.SelectedRowInComponent(0))
            //{
            //    _picker.Select(s.SelectedIndex, 0, false);
            //}

            if (_doneClicked)
            {
                if (s.SelectedIndex == -1 && Element.Items != null && Element.Items.Count > 0)
                    UpdatePickerSelectedIndex(0);
                UpdatePickerFromModel(s);
            }

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

        protected internal virtual void UpdateFont()
        {
            Control.Font = UIFont.FromName(Element.FontFamily, (float)Element.FontSize);
        }

        protected virtual void UpdateAttributedPlaceholder(NSAttributedString nsAttributedString) =>
            Control.AttributedPlaceholder = nsAttributedString;

        void UpdatePicker()
        {
            var selectedIndex = Element.SelectedIndex;
            var items = Element.Items;

            var oldText = Control.Text;
            Control.Text = selectedIndex == -1 || items == null || selectedIndex >= items.Count ? "" : items[selectedIndex];
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
                ElementController.SetValueFromRenderer(Xamarin.Forms.Picker.SelectedIndexProperty, s.SelectedIndex);
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
            source.SelectedItem = formsIndex >= 0 ? Element.Items[formsIndex] : null;
            _picker.Select(Math.Max(formsIndex, 0), 0, true);
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
                    ((INotifyCollectionChanged)Element.Items).CollectionChanged -= RowsCollectionChanged;
            }

            base.Dispose(disposing);
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
            return _renderer.Element.Items != null ? _renderer.Element.Items.Count : 0;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return _renderer.Element.Items[(int)row];
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            if (_renderer.Element.Items.Count == 0)
            {
                SelectedItem = null;
                SelectedIndex = -1;
            }
            else
            {
                SelectedItem = _renderer.Element.Items[(int)row];
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