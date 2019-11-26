using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Global.InputForms;
using Global.InputForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using TextAlignment = Android.Views.TextAlignment;
using AView = Android.Views.View;
using System;

[assembly: ExportRenderer(typeof(BlankPicker), typeof(BlankPickerRenderer))]

namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerRenderer : EntryRenderer
    {
        readonly static HashSet<Keycode> AvailableKeys = new HashSet<Keycode>(new[] {
            Keycode.Tab, Keycode.Forward, Keycode.DpadDown, Keycode.DpadLeft, Keycode.DpadRight, Keycode.DpadUp
        });

        private AlertDialog _dialog;
        private BlankPicker blankPicker;

        public BlankPickerRenderer(Context context) : base(context)
        {
        }

        private IElementController EController => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (!(e.NewElement is BlankPicker bPicker)) return;
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

                    Control.Text = blankPicker.SelectedItem?.ToString();
                    Control.KeyListener = null;

                    Control.TextChanged += (sender, arg)
                        => bPicker.Text = arg.Text.ToString();

                    SetAttributes();
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

        public void OnClick(object sender, EventArgs e)
        {
            var model = blankPicker;
            var picker = new NumberPicker(Context);
            if (model.Items != null && model.Items.Any())
            {
                picker.MaxValue = model.Items.Count - 1;
                picker.MinValue = 0;
                picker.SetDisplayedValues(model.Items.ToArray());
                picker.WrapSelectorWheel = false;
                picker.DescendantFocusability = DescendantFocusability.BlockDescendants;
                picker.Value = model.SelectedIndex;
            }

            var layout = new LinearLayout(Context) {Orientation = Orientation.Vertical};
            layout.AddView(picker);

            var builder = new AlertDialog.Builder(Context);
            builder.SetView(layout);
            builder.SetTitle(model.Title ?? "");
            builder.SetNegativeButton(blankPicker.CancelButtonText ?? "Cancel", (s, a) =>
            {
                blankPicker.SendCancelClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                _dialog = null;
                Control.ClearFocus();
            });
            builder.SetPositiveButton(blankPicker.DoneButtonText ?? "OK", (s, a) =>
            {
                EController.SetValueFromRenderer(BlankPicker.SelectedIndexProperty, picker.Value);
                //blankPicker.SelectedItem = picker.Value;
                blankPicker.SendDoneClicked();
                // It is possible for the Content of the Page to be changed on SelectedIndexChanged. 
                // In this case, the Element & Control will no longer exist.
                if (blankPicker != null)
                {
                    if (model.Items.Count > 0 && blankPicker.SelectedIndex >= 0)
                        Control.Text = model.Items[blankPicker.SelectedIndex];
                    EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                    Control.ClearFocus();
                }

                _dialog = null;
            });

            _dialog = builder.Create();
            _dialog.DismissEvent += (s, args) =>
            {
                EController?.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                Control.ClearFocus();
            };
            _dialog.Show();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}