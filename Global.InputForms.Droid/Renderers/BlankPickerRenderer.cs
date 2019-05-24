using System;
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

[assembly: ExportRenderer(typeof(BlankPicker), typeof(BlankPickerRenderer))]
namespace Global.InputForms.Droid.Renderers
{
    public class BlankPickerRenderer : PickerRenderer, Android.Views.View.IOnClickListener
    {
        AlertDialog _dialog;
        IElementController EController => Element as IElementController;
        BlankPicker blankPicker;

        public BlankPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            if (!(e.NewElement is BlankPicker bPicker)) return;
            blankPicker = bPicker;
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var textField = CreateNativeControl();
                    textField.SetOnClickListener(this);
                    textField.InputType = InputTypes.Null;
                    SetNativeControl(textField);
                }
            }
            base.OnElementChanged(e);

            SetPlaceholder();
            SetAlignment();
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

            if (Element is BlankPicker picker && !string.IsNullOrWhiteSpace(picker.Placeholder))
                Control.Text = picker.Placeholder;
        }

        private void SetAlignment()
        {
            switch (((BlankPicker)Element).HorizontalTextAlignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.TextAlignment = Android.Views.TextAlignment.Center;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.TextAlignment = Android.Views.TextAlignment.ViewEnd;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.TextAlignment = Android.Views.TextAlignment.ViewStart;
                    break;
            }
        }

        public void OnClick(Android.Views.View v)
        {
            Picker model = Element;

            var picker = new NumberPicker(Context);
            if (model.Items != null && model.Items.Any())
            {
                picker.MaxValue = model.Items.Count - 1;
                picker.MinValue = 0;
                picker.SetDisplayedValues(model.Items.ToArray());
                picker.WrapSelectorWheel = false;
                picker.DescendantFocusability = Android.Views.DescendantFocusability.BlockDescendants;
                picker.Value = model.SelectedIndex;
            }

            var layout = new LinearLayout(Context) { Orientation = Orientation.Vertical };
            layout.AddView(picker);

            EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            var builder = new AlertDialog.Builder(Context);
            builder.SetView(layout);
            builder.SetTitle(model.Title ?? "");
            builder.SetNegativeButton(blankPicker.CancelButtonText ?? "Cancel", (s, a) =>
            {
                blankPicker.SendCancelClicked();
                EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                _dialog = null;
            });
            builder.SetPositiveButton(blankPicker.DoneButtonText ?? "OK", (s, a) =>
            {
                EController.SetValueFromRenderer(Picker.SelectedIndexProperty, picker.Value);
                //blankPicker.SelectedItem = picker.Value;
                blankPicker.SendDoneClicked();
                // It is possible for the Content of the Page to be changed on SelectedIndexChanged. 
                // In this case, the Element & Control will no longer exist.
                if (Element != null)
                {
                    if (model.Items.Count > 0 && Element.SelectedIndex >= 0)
                        Control.Text = model.Items[Element.SelectedIndex];
                    EController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                }
                _dialog = null;
            });

            _dialog = builder.Create();
            _dialog.DismissEvent += (sender, args) =>
            {
                EController?.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            };
            _dialog.Show();
        }
    }
}