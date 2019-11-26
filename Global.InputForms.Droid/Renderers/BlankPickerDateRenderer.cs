using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.App.DatePickerDialog;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankPickerDateRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerDateRenderer : EntryRenderer, IOnDateSetListener
    {
        readonly static HashSet<Keycode> AvailableKeys = new HashSet<Keycode>(new[] {
            Keycode.Tab, Keycode.Forward, Keycode.DpadDown, Keycode.DpadLeft, Keycode.DpadRight, Keycode.DpadUp
        });
        private DatePickerDialog _dialog;
        private BlankDatePicker blankPicker;
        private string _oldText;

        public BlankPickerDateRenderer(Context context) : base(context)
        {
        }

        private IElementController EController => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
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

                    Control.TextChanged += (sender, arg) =>
                    {
                        if (bPicker.Text != arg.Text.ToString())
                            bPicker.Text = arg.Text.ToString();
                    };
                    SetAttributes();
                }
            if (e.OldElement != null)
            {
                Control.SetOnClickListener(null);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private void SetAttributes()
        {
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control.SetPadding(0, 7, 0, 3);
            Control.Gravity = GravityFlags.Fill;
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            var unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public void OnClick(object sender, EventArgs e)
        {
            _dialog = new DatePickerDialog(Context, this, blankPicker.Date.Year, blankPicker.Date.Month - 1, blankPicker.Date.Day);
            _dialog.DatePicker.MaxDate = UnixTimestampFromDateTime(blankPicker.MaximumDate);
            _dialog.DatePicker.MinDate = UnixTimestampFromDateTime(blankPicker.MinimumDate);

            _dialog.SetButton(blankPicker.DoneButtonText, (k, p) =>
            {
                blankPicker.Text = _dialog.DatePicker.DateTime.ToString(blankPicker.Format);
                blankPicker.Date = _dialog.DatePicker.DateTime;
                blankPicker.SendDoneClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();
            });
            _dialog.SetButton2(blankPicker.CancelButtonText, (s, el) => {
                blankPicker.SendCancelClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();
            });
            _dialog.Show();
        }

        public void OnDateSet(Android.Widget.DatePicker view, int year, int month, int dayOfMonth)
        {
            blankPicker.Text = _dialog.DatePicker.DateTime.ToString(blankPicker.Format);
            blankPicker.Date = new DateTime(year, month, dayOfMonth);
            EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();

            _dialog = null;
        }

        public static void Dispose(EditText editText)
        {
            editText.SetOnClickListener(null);
        }
    }
}