using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.App.DatePickerDialog;
using Color = Android.Graphics.Color;
using TextAlignment = Android.Views.TextAlignment;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(BlankDatePicker), typeof(BlankPickerDateRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerDateRenderer : DatePickerRenderer, View.IOnClickListener, IOnDateSetListener
    {
        private DatePickerDialog _dialog;
        private BlankDatePicker blankPicker;
        private string _oldText;

        public BlankPickerDateRenderer(Context context) : base(context)
        {
        }

        private IElementController EController => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankDatePicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
                if (Control != null)
                {
                    if (!string.IsNullOrEmpty(Control.Text))
                        Control.Text = string.Empty;

                    Control.SetOnClickListener(this);
                    Clickable = true;
                    Control.KeyListener = null;

                    Control.TextChanged += (sender, arg) =>
                    {
                        if (bPicker.Text != arg.Text.ToString())
                            bPicker.Text = arg.Text.ToString();
                    };
                    SetPlaceholder();
                    SetAlignment();
                    Control.SetPadding(0, 7, 0, 3);
                    Control.Gravity = GravityFlags.Fill;
                }
            if (e.OldElement != null)
            {
                Control.SetOnClickListener(null);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetPlaceholder();

            if (e.PropertyName == nameof(BlankPicker.HorizontalTextAlignment)) SetAlignment();
            
            if (e.PropertyName == nameof(BlankPicker.Text)) UpdateText();
        }

        private void SetPlaceholder()
        {
            Control.SetBackgroundColor(Color.Transparent);

            if (!string.IsNullOrWhiteSpace(blankPicker.Placeholder))
                Control.Text = blankPicker.Placeholder;
        }

        private void SetAlignment()
        {
            switch (blankPicker.HorizontalTextAlignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.TextAlignment = TextAlignment.Center;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.TextAlignment = TextAlignment.ViewEnd;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.TextAlignment = TextAlignment.ViewStart;
                    break;
            }
        }

        void UpdateText()
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (Control.Text != blankPicker.Text)
                Control.Text = blankPicker.Text;
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            var unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public void OnClick(View v)
        {
            EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
            _dialog = new DatePickerDialog(Context, this, blankPicker.Date.Year, blankPicker.Date.Month - 1, blankPicker.Date.Day);
            _dialog.DatePicker.MaxDate = UnixTimestampFromDateTime(blankPicker.MaximumDate);
            _dialog.DatePicker.MinDate = UnixTimestampFromDateTime(blankPicker.MinimumDate);

            _dialog.SetButton(blankPicker.DoneButtonText, (k, p) =>
            {
                blankPicker.Text = _dialog.DatePicker.DateTime.ToString(blankPicker.Format);
                blankPicker.Date = _dialog.DatePicker.DateTime;
                blankPicker.SendDoneClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            });
            _dialog.SetButton2(blankPicker.CancelButtonText, (s, el) => {
                blankPicker.SendCancelClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
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
    }
}