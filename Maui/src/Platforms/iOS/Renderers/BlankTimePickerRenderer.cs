//using System;
//using System.Collections.Generic;
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
//using Global.InputForms.iOS.Extensions;

////[assembly: ExportRenderer(typeof(BlankTimePicker), typeof(BlankTimePickerRenderer))]

//namespace Global.InputForms.iOS.Renderers
//{
//    public class BlankTimePickerRenderer : EntryRenderer
//    {
//        private bool _disposed;

//        private UIDatePicker _picker;
//        private BlankTimePicker blankPicker;

//        private IElementController ElementController => Element;
//        private bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

//        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
//        {
//            base.OnElementChanged(e);

//            if (!(e.NewElement is BlankTimePicker bPicker)) return;
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
//                Control.EditingDidEnd += OnEnded;

//                if (IsiOS9OrNewer)
//                {
//                    Control.InputAssistantItem.LeadingBarButtonGroups = null;
//                    Control.InputAssistantItem.TrailingBarButtonGroups = null;
//                }

//                _picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };
//                _picker.ValueChanged += OnValueChanged;
//                _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
//                Control.InputView = _picker;
//                Control.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

//                SetInputAccessoryView();
//                UpdateTime();
//                SetAttributes();
//            }
//        }

//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);
//            if (e.PropertyName == nameof(BlankTimePicker.Time)
//                || e.PropertyName == nameof(BlankTimePicker.Format))
//                UpdateTime();
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
//                    (s, ev) =>
//                    {
//                        var timeOfDay = _picker.Date.ToGlobalDateTime().TimeOfDay;
//                        var time = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, 0);
//                        blankPicker.Text = Control.Text = new DateTime(time.Ticks).ToString(blankPicker.Format);
//                        blankPicker.Time = time;
//                        if (blankPicker != null)
//                            blankPicker.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
//                        Control.ResignFirstResponder();
//                    });
//                doneButton.Clicked += (sender, e) => { blankPicker.SendDoneClicked(); };
//                items.Add(doneButton);
//            }
//            toolbar.SetItems(items.ToArray(), true);
//            Control.InputAccessoryView = toolbar;
//            Control.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
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

//        private void OnValueChanged(object sender, EventArgs e)
//        {
//            if (blankPicker.UpdateMode == UpdateMode.Immediately)
//            {
//                var timeOfDay = _picker.Date.ToGlobalDateTime().TimeOfDay;
//                var time = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, 0);
//                blankPicker.Text = Control.Text = new DateTime(time.Ticks).ToString(blankPicker.Format);
//                blankPicker.Time = time;
//            }
//        }

//        private void UpdateTime()
//        {
//            if (blankPicker.TimeSet)
//            {
//                _picker.Date = new DateTime(blankPicker.Time.Ticks).ToGlobalNSDate();
//                blankPicker.Text = Control.Text = new DateTime(blankPicker.Time.Ticks).ToString(blankPicker.Format);
//            }
//            else
//            {
//                blankPicker.Text = Control.Text = string.Empty;
//            }
//        }

//        private void SetAttributes()
//        {
//            if (Control != null)
//                Control.BorderStyle = UITextBorderStyle.None;
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
//                    _picker.RemoveFromSuperview();
//                    _picker.ValueChanged -= OnValueChanged;
//                    _picker.Dispose();
//                    _picker = null;
//                }

//                if (Control != null)
//                {
//                    Control.EditingDidBegin -= OnStarted;
//                    Control.EditingDidEnd -= OnEnded;
//                }
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
//    }
//}