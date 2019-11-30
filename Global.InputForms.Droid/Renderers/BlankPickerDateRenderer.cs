using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.App.DatePickerDialog;
using Color = Android.Graphics.Color;
using DatePicker = Android.Widget.DatePicker;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankPickerDateRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerDateRenderer : EntryRenderer, IOnDateSetListener
    {
        private static readonly HashSet<Keycode> AvailableKeys = new HashSet<Keycode>(new[]
        {
            Keycode.Tab, Keycode.Forward, Keycode.DpadDown, Keycode.DpadLeft, Keycode.DpadRight, Keycode.DpadUp
        });

        private DatePickerDialog _dialog;
        private string _oldText;
        private BlankDatePicker blankPicker;

        public BlankPickerDateRenderer(Context context) : base(context)
        {
        }

        private IElementController EController => Element;

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            blankPicker.Text = _dialog.DatePicker.DateTime.ToString(blankPicker.Format);
            blankPicker.Date = new DateTime(year, month, dayOfMonth);
            EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();
            HideKeyboard();

            _dialog = null;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
                if (Control != null)
                {
                    if (!string.IsNullOrEmpty(Control.Text))
                        Control.Text = string.Empty;

                    Control.Focusable = true;
                    Control.Clickable = false;
                    Control.InputType = InputTypes.Null;
                    blankPicker.Focused += OnClick;

                    //Control.TextChanged += (sender, arg) =>
                    //{
                    //    if (bPicker.Text != arg.Text.ToString())
                    //        bPicker.Text = arg.Text.ToString();
                    //};
                    SetAttributes();
                    UpdateDate();
                }

            if (e.OldElement != null) Control.SetOnClickListener(null);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(BlankDatePicker.Date) || e.PropertyName == nameof(BlankDatePicker.Format))
                UpdateDate();
        }

        private void SetAttributes()
        {
            // Disable the Keyboard on Focus
            Control.ShowSoftInputOnFocus = false;

            Control.SetBackgroundColor(Color.Transparent);
            Control.SetPadding(0, 7, 0, 3);
            Control.Gravity = GravityFlags.Fill;
        }

        private void HideKeyboard()
        {
            var imm = (InputMethodManager) Context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(Control.WindowToken, 0);
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            var unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public void OnClick(object sender, EventArgs e)
        {
            HideKeyboard();
            _dialog = new DatePickerDialog(Context, this, blankPicker.Date.Year, blankPicker.Date.Month - 1,
                blankPicker.Date.Day);
            _dialog.DatePicker.MaxDate = UnixTimestampFromDateTime(blankPicker.MaximumDate);
            _dialog.DatePicker.MinDate = UnixTimestampFromDateTime(blankPicker.MinimumDate);

            _dialog.SetButton(blankPicker.DoneButtonText, (k, p) =>
            {
                blankPicker.Text = _dialog.DatePicker.DateTime.ToString(blankPicker.Format);
                blankPicker.Date = _dialog.DatePicker.DateTime;
                blankPicker.SendDoneClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();
                HideKeyboard();
            });
            _dialog.SetButton2(blankPicker.CancelButtonText, (s, el) =>
            {
                blankPicker.SendCancelClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();
                HideKeyboard();
            });

            _dialog.CancelEvent += _dialog_DismissEvent;


            _dialog.Show();
        }

        private void _dialog_DismissEvent(object sender, EventArgs e)
        {
            blankPicker.Unfocus();
            EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();
            HideKeyboard();
        }

        private void UpdateDate()
        {
            if (blankPicker.DateSet)
                Control.Text = blankPicker.Date.Date.ToString(blankPicker.Format);
            else
                Control.Text = string.Empty;
        }

        public static void Dispose(EditText editText)
        {
            editText.SetOnClickListener(null);
        }
    }
}