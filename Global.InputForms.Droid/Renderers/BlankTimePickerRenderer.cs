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
        private bool _doneClicked;

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
                var time = new TimeSpan(blankPicker.Time.Hours, blankPicker.Time.Minutes, 0);
                blankPicker.Text = Control.Text = new DateTime(time.Ticks).ToString(blankPicker.Format);
            }
            else
                Control.Text = string.Empty;
        }

        public void OnClick(object sender, EventArgs e)
        {
            _dialog = new TimePickerDialog(Context, this, blankPicker.Time.Hours, blankPicker.Time.Minutes, true);

            _dialog.SetButton(blankPicker.DoneButtonText, (k,p) => { });
            _dialog.SetButton2(blankPicker.CancelButtonText, (k, p) =>
            {
                EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();
                blankPicker.SendCancelClicked();
            });
            
            _dialog.CancelEvent += _dialog_DismissEvent;

            _dialog.Show();
        }

        private void _dialog_DismissEvent(object sender, EventArgs e)
        {
            blankPicker.Unfocus();
            EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();
        }

        public void OnTimeSet(Android.Widget.TimePicker view, int hoursOfDay, int minute)
        {

            var time = blankPicker.Time = new TimeSpan(hoursOfDay, minute, 0);
            Control.Text = new DateTime(time.Ticks).ToString(blankPicker.Format);
            EController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            Control.ClearFocus();
            blankPicker.SendDoneClicked();
            _dialog = null;
        }
        
    }
}