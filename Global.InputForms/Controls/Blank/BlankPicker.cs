using System;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class BlankPicker : Picker
    {
        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(BlankPicker), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(BlankPicker), "Cancel");

        /// <summary>
        ///     The Entry Placeholder property.
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(BlankPicker), string.Empty);

        /// <summary>
        ///     The Entry Placeholder Color property.
        /// </summary>
        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(BlankPicker), Color.Black);

        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(DatePickerView),
                TextAlignment.Start);

        public string DoneButtonText
        {
            get => (string) GetValue(DoneButtonTextProperty);
            set => SetValue(DoneButtonTextProperty, value);
        }

        public string CancelButtonText
        {
            get => (string) GetValue(CancelButtonTextProperty);
            set => SetValue(CancelButtonTextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry placeholder.
        /// </summary>
        /// <value>The entry placeholdeer.</value>
        public string Placeholder
        {
            get => (string) GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry placeholder color.
        /// </summary>
        /// <value>The entry placeholder color.</value>
        public Color PlaceholderColor
        {
            get => (Color) GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment) GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }

        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;

        public void SendDoneClicked()
        {
            DoneClicked?.Invoke(this, new EventArgs());
        }

        public void SendCancelClicked()
        {
            CancelClicked?.Invoke(this, new EventArgs());
        }
    }
}