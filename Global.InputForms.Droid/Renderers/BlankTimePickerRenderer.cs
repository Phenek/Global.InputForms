using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using static Android.App.TimePickerDialog;
using TextAlignment = Android.Views.TextAlignment;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(BlankTimePicker), typeof(BlankTimePickerRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankTimePickerRenderer : EntryRenderer, IOnTimeSetListener
    {
        private TimePickerDialog _dialog;
        private BlankTimePicker blankPicker;
        private string _oldText;

        public BlankTimePickerRenderer(Context context) : base(context)
        {
        }

        private IElementController EController => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (!(e.NewElement is BlankTimePicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
                if (Control != null)
                {
                    if (!string.IsNullOrEmpty(Control.Text))
                        bPicker.Text = Control.Text;

                    Control.Focusable = true;
                    Control.Clickable = false;
                    Control.InputType = InputTypes.Null;
                    blankPicker.Focused += OnClick;


                    //Control.TextChanged += (sender, arg) =>
                    //{
                    //    var selectedDate = arg.Text.ToString();
                    //    if (selectedDate == bPicker.Placeholder) Control.Text = DateTime.Now.ToString(bPicker.Format);

                    //    bPicker.Text = arg.Text.ToString();
                    //};

                    SetAttributes();
                    UpdateTime();
                }
            if (e.OldElement != null)
            {
                Control.SetOnClickListener(null);
            }
        }

        private void SetAttributes()
        {
            Control.SetBackgroundColor(Color.Transparent);
            Control.SetPadding(0, 7, 0, 3);
            Control.Gravity = GravityFlags.Fill;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(BlankTimePicker.Time) || e.PropertyName == nameof(BlankTimePicker.Format))
                UpdateTime();
        }

        void UpdateTime()
        {
            if (blankPicker.TimeSet)
            {
                Control.Text = blankPicker.Time.ToString(blankPicker.Format);
            }
            else
                Control.Text = string.Empty;
        }

        public void OnClick(object sender, EventArgs e)
        {
            _dialog = new TimePickerDialog(Context, this, blankPicker.Time.Hours, blankPicker.Time.Minutes, true);

            _dialog.SetButton(blankPicker.CancelButtonText, (s, el) => {
                blankPicker.SendCancelClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();
            });
            _dialog.SetButton2(blankPicker.DoneButtonText, (k, p) =>
            {
                Control.Text = blankPicker.Time.ToString(blankPicker.Format);
                blankPicker.SendDoneClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();
            });
            _dialog.Show();
        }

        
        public void OnTimeSet(Android.Widget.TimePicker view, int hoursOfDay, int minute)
        {
            blankPicker.Time = new TimeSpan(hoursOfDay, minute, 0);
            EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();
            _dialog = null;
        }
        
    }
}