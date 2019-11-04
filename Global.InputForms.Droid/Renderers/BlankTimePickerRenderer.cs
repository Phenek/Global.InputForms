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

[assembly: ExportRenderer(typeof(BlankTimePicker), typeof(BlankTimePickerRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankTimePickerRenderer : TimePickerRenderer, View.IOnClickListener//, TimePickerDialog.IOnTimeSetListener
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
                {
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
            _dialog = new TimePickerDialog(Context, this, Element.Time.Hours, Element.Time.Minutes, true);

            _dialog.SetButton(blankPicker.CancelButtonText, (s, el) => {
                blankPicker.SendCancelClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            });
            _dialog.SetButton2(blankPicker.DoneButtonText, (k, p) =>
            {
                Control.Text = Element.Time.ToString(Element.Format);
                blankPicker.SendDoneClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            });
            _dialog.Show();
        }

        /*
        public void OnTimeSet(Android.Widget.TimePicker view, int hoursOfDay, int minute)
        {
            Element.Time = new TimeSpan(hoursOfDay, minute, 0);
            EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();

            _dialog = null;
        }
        */
    }
}