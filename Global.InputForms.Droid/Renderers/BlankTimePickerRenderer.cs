using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Views;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using TextAlignment = Android.Views.TextAlignment;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(BlankTimePicker), typeof(BlankPickerRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankTimePickerRenderer : TimePickerRenderer, View.IOnClickListener
    {
        private TimePickerDialog _dialog;
        private BlankTimePicker blankPicker;

        public BlankTimePickerRenderer(Context context) : base(context)
        {
        }

        private IElementController EController => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankTimePicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
                if (Control != null)
                    Control.SetOnClickListener(this);
                    Clickable = true;

                    Control.TextChanged += (sender, arg) =>
                    {
                        var selectedDate = arg.Text.ToString();
                        if (selectedDate == bPicker.Placeholder) Control.Text = DateTime.Now.ToString(bPicker.Format);
                    };
            SetPlaceholder();
            SetAlignment();
            Control.SetPadding(0, 7, 0, 3);
            Control.Gravity = GravityFlags.Fill;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Entry.Placeholder)) SetPlaceholder();

            if (e.PropertyName == nameof(BlankPicker.HorizontalTextAlignment)) SetAlignment();
        }


        private void SetPlaceholder()
        {
            Control.SetBackgroundColor(Color.Transparent);

            if (Element is BlankTimePicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                Control.Text = picker.Placeholder;
        }

        private void SetAlignment()
        {
            switch (((BlankTimePicker) Element).HorizontalTextAlignment)
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

        public void OnClick(View v)
        {
            _dialog = new TimePickerDialog(Context, (s, d) =>
            {
                EController.SetValueFromRenderer(TimePicker.TimeProperty, new TimeSpan(d.HourOfDay, d.Minute, 0));
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();

                _dialog = null;
            }, Element.Time.Hours, Element.Time.Minutes, true);

            _dialog.SetButton(blankPicker.CancelButtonText, (s, el) => { blankPicker.SendCancelClicked(); });
            _dialog.SetButton2(blankPicker.DoneButtonText, (k, p) =>
            {
                Control.Text = Element.Time.ToString(Element.Format);
                blankPicker.SendDoneClicked();
            });
            _dialog.Show();
        }
    }
}