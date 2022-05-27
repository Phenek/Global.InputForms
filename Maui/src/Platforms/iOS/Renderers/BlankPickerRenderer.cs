//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using Foundation;
//using Global.InputForms;
//using Global.InputForms.iOS.Renderers;
//using ObjCRuntime;
//using UIKit;
//using Microsoft.Maui.Controls;
//using Microsoft.Maui.Controls.Compatibility;
//using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
//using Microsoft.Maui.Controls.Platform;

////[assembly: ExportRenderer(typeof(BlankPicker), typeof(BlankPickerRenderer))]

//namespace Global.InputForms.iOS.Renderers
//{
//    public class BlankPickerRenderer : EntryRenderer
//    {
//        private bool _disposed;

//        private UIPickerView _picker;
//        public BlankPicker blankPicker;
//        private bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

//        private IElementController ElementController => Element;

//        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
//        {
//            base.OnElementChanged(e);

//            if (e.OldElement != null)
//                ((INotifyCollectionChanged) blankPicker.Items).CollectionChanged -= RowsCollectionChanged;

//            if (!(e.NewElement is BlankPicker bPicker)) return;
//            blankPicker = bPicker;

//            if (Control != null)
//            {
//                Control.SpellCheckingType = UITextSpellCheckingType.No;
//                Control.AutocorrectionType = UITextAutocorrectionType.No;
//                Control.AutocapitalizationType = UITextAutocapitalizationType.None;
//                Control.BorderStyle = UITextBorderStyle.RoundedRect;
//                Control.AccessibilityTraits = UIAccessibilityTrait.Button;
//                UIMenuController.SharedMenuController.MenuVisible = false;

//                Control.EditingDidBegin += OnStarted;
//                Control.Ended += OnEnded;
//                Control.EditingChanged += OnEditing;

//                if (IsiOS9OrNewer)
//                {
//                    Control.InputAssistantItem.LeadingBarButtonGroups = null;
//                    Control.InputAssistantItem.TrailingBarButtonGroups = null;
//                }

//                _picker = new UIPickerView();
//                Control.InputView = _picker;
//                _picker.Model = new PickerSource(this);
//                Control.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

//                SetInputAccessoryView();
//                UpdatePicker();
//                SetAttributes();

//                if (blankPicker.Items is INotifyCollectionChanged Collection)
//                    Collection.CollectionChanged += RowsCollectionChanged;
//            }
//        }

//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);

//            if (e.PropertyName == nameof(BlankPicker.SelectedIndex)) UpdatePicker();
//        }

//        private void SetAttributes()
//        {
//            if (Control != null)
//                Control.BorderStyle = UITextBorderStyle.None;
//        }

//        public void SetInputAccessoryView()
//        {
//            if (string.IsNullOrEmpty(blankPicker.DoneButtonText) && string.IsNullOrEmpty(blankPicker.CancelButtonText))
//            {
//                Control.InputAccessoryView = null;
//                return;
//            }

//            var toolbar = new UIToolbar
//            {
//                BarStyle = UIBarStyle.Default,
//                Translucent = true
//            };
//            toolbar.SizeToFit();

//            var items = new List<UIBarButtonItem>();

//            if (!string.IsNullOrEmpty(blankPicker.CancelButtonText))
//            {
//                var cancelButton = new UIBarButtonItem(blankPicker.CancelButtonText, UIBarButtonItemStyle.Done,
//                    (s, ev) => { Control.ResignFirstResponder(); });
//                cancelButton.Clicked += (sender, e) => { blankPicker.SendCancelClicked(); };
//                items.Add(cancelButton);
//            }

//            var flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
//            items.Add(flexible);

//            if (!string.IsNullOrEmpty(blankPicker.DoneButtonText))
//            {
//                var doneButton = new UIBarButtonItem(blankPicker.DoneButtonText, UIBarButtonItemStyle.Done,
//                    (sd, ev) =>
//                    {
//                        var s = (PickerSource) _picker.Model;
//                        if (s.SelectedIndex == -1 && blankPicker.Items != null && blankPicker.Items.Count > 0)
//                            UpdatePickerSelectedIndex(0);
//                        UpdatePickerFromModel(s);
//                        if (ElementController != null)
//                            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
//                        Control.ResignFirstResponder();
//                    });
//                doneButton.Clicked += (sender, e) => { blankPicker.SendDoneClicked(); };
//                items.Add(doneButton);
//            }
//            toolbar.SetItems(items.ToArray(), true);
//            Control.InputAccessoryView = toolbar;
//            Control.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
//        }

//        private void OnEditing(object sender, EventArgs eventArgs)
//        {
//            // Reset the TextField's Text so it appears as if typing with a keyboard does not work.
//            var selectedIndex = blankPicker.SelectedIndex;
//            var items = blankPicker.Items;
//            blankPicker.Text = Control.Text = selectedIndex == -1 || items == null ? "" : items[selectedIndex];
//            // Also clears the undo stack (undo/redo possible on iPads)
//            Control.UndoManager.RemoveAllActions();
//        }

//        private void OnEnded(object sender, EventArgs eventArgs)
//        {
//            if (ElementController != null)
//                ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
//        }

//        private void OnStarted(object sender, EventArgs eventArgs)
//        {
//            if (ElementController != null)
//                ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
//        }

//        private void RowsCollectionChanged(object sender, EventArgs e)
//        {
//            UpdatePicker();
//        }

//        private void UpdatePicker()
//        {
//            var selectedIndex = blankPicker.SelectedIndex;
//            var items = blankPicker.Items;

//            var oldText = Control.Text;
//            blankPicker.Text = Control.Text = selectedIndex == -1 || items == null || selectedIndex >= items.Count
//                ? ""
//                : items[selectedIndex];
//            UpdatePickerNativeSize(oldText);
//            _picker.ReloadAllComponents();
//            if (items == null || items.Count == 0)
//                return;

//            UpdatePickerSelectedIndex(selectedIndex);
//        }

//        private void UpdatePickerFromModel(PickerSource s)
//        {
//            if (Element != null)
//            {
//                var oldText = Control.Text;
//                blankPicker.Text = Control.Text = s.SelectedItem;
//                UpdatePickerNativeSize(oldText);
//                if (ElementController != null)
//                    ElementController.SetValueFromRenderer(BlankPicker.SelectedIndexProperty, s.SelectedIndex);
//            }
//        }

//        private void UpdatePickerNativeSize(string oldText)
//        {
//            if (oldText != Control.Text)
//                ((IVisualElementController) Element).PlatformSizeChanged();
//        }

//        private void UpdatePickerSelectedIndex(int formsIndex)
//        {
//            var source = (PickerSource) _picker.Model;
//            source.SelectedIndex = formsIndex;
//            source.SelectedItem = formsIndex >= 0 ? blankPicker.Items[formsIndex] : null;
//            _picker.Select(Math.Max(formsIndex, 0), 0, true);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (_disposed)
//                return;

//            _disposed = true;

//            if (disposing)
//            {
//                if (_picker != null)
//                {
//                    if (_picker.Model != null)
//                    {
//                        _picker.Model.Dispose();
//                        _picker.Model = null;
//                    }

//                    _picker.RemoveFromSuperview();
//                    _picker.Dispose();
//                    _picker = null;
//                }

//                if (Control != null)
//                {
//                    Control.EditingDidBegin -= OnStarted;
//                    Control.EditingDidEnd -= OnEnded;
//                    Control.EditingChanged -= OnEditing;
//                }

//                if (Element != null && blankPicker.Items is INotifyCollectionChanged colletion)
//                    colletion.CollectionChanged -= RowsCollectionChanged;
//            }

//            base.Dispose(disposing);
//        }

//        public override bool CanPerform(Selector action, NSObject withSender)
//        {
//            NSOperationQueue.MainQueue.AddOperation(() =>
//            {
//                UIMenuController.SharedMenuController.SetMenuVisible(false, false);
//            });
//            return base.CanPerform(action, withSender);
//        }

//        private class PickerSource : UIPickerViewModel
//        {
//            private bool _disposed;
//            private BlankPickerRenderer _renderer;

//            public PickerSource(BlankPickerRenderer renderer)
//            {
//                _renderer = renderer;
//            }

//            public int SelectedIndex { get; internal set; }

//            public string SelectedItem { get; internal set; }

//            public override nint GetComponentCount(UIPickerView picker)
//            {
//                return 1;
//            }

//            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
//            {
//                return _renderer.blankPicker.Items != null ? _renderer.blankPicker.Items.Count : 0;
//            }

//            public override string GetTitle(UIPickerView picker, nint row, nint component)
//            {
//                return _renderer.blankPicker.Items[(int) row];
//            }

//            public override void Selected(UIPickerView picker, nint row, nint component)
//            {
//                if (_renderer.blankPicker.Items.Count == 0)
//                {
//                    SelectedItem = null;
//                    SelectedIndex = -1;
//                }
//                else
//                {
//                    SelectedItem = _renderer.blankPicker.Items[(int) row];
//                    SelectedIndex = (int) row;
//                }

//                if (_renderer.blankPicker.UpdateMode == UpdateMode.Immediately)
//                    _renderer.UpdatePickerFromModel(this);
//            }

//            protected override void Dispose(bool disposing)
//            {
//                if (_disposed)
//                    return;

//                _disposed = true;

//                if (disposing)
//                    _renderer = null;

//                base.Dispose(disposing);
//            }
//        }
//    }
//}