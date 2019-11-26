using System;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class BlankDatePicker : DatePicker
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(BlankDatePicker), defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) => ((BlankDatePicker)bindable).OnTextChanged((string)oldValue, (string)newValue));

        public new static readonly BindableProperty DateProperty =
            BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(BlankDatePicker), default(DateTime));

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(BlankDatePicker), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(BlankDatePicker), "Cancel");

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(BlankDatePicker), string.Empty);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(BlankDatePicker), Color.Black);

        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(DatePickerView),
                TextAlignment.Start);

        public event EventHandler<TextChangedEventArgs> TextChanged;
        private bool _dateBinded;

        protected void OnTextChanged(string oldValue, string newValue)
        {
            TextChanged?.Invoke(this, new TextChangedEventArgs(oldValue, newValue));
        }

        public new DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set
            {
                if (!_dateBinded)
                {
                    base.SetBinding(DatePicker.DateProperty,
                    new Binding(nameof(Date)) { Source = this, Mode = BindingMode.TwoWay });
                    _dateBinded = true;
                }
                SetValue(DateProperty, value);
            }
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

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