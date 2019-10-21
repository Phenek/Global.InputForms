using System;
using Global.InputForms.Converters;
using Xamarin.Forms;

namespace Global.InputForms
{
    [ContentProperty(nameof(Children))]
    public class DatePickerView : EntryLayout
    {
        /// <summary>
        ///     The Date property.
        /// </summary>
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime),
            typeof(DatePickerView), DateTime.Today);

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string),
            typeof(DatePickerView), string.Empty);

        /// <summary>
        ///     The Minimum Date property.
        /// </summary>
        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate),
            typeof(DateTime), typeof(DatePickerView), new DateTime(1900, 1, 1));

        /// <summary>
        ///     The Maximum Date property.
        /// </summary>
        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate),
            typeof(DateTime), typeof(DatePickerView), new DateTime(2100, 12, 31));

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(DatePickerView), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(DatePickerView), "Cancel");

        private readonly BlankDatePicker _datePicker;
        private readonly Frame _pFrame;

        public EventHandler<DateChangedEventArgs> DateSelected;

        public DatePickerView()
        {
            _datePicker = new BlankDatePicker
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _datePicker.SetBinding(DatePicker.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});

            _datePicker.SetBinding(DatePicker.FormatProperty,
                new Binding(nameof(Format)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.MinimumDateProperty,
                new Binding(nameof(MinimumDate)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.MaximumDateProperty,
                new Binding(nameof(MaximumDate)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.DateProperty,
                new Binding(nameof(Date)) {Source = this, Mode = BindingMode.TwoWay});

            _datePicker.SetBinding(BlankDatePicker.DoneButtonTextProperty,
                new Binding(nameof(DoneButtonText)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.CancelButtonTextProperty,
                new Binding(nameof(CancelButtonText)) {Source = this, Mode = BindingMode.OneWay});

            _pFrame = new Frame
            {
                Padding = 0,
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                Content = _datePicker
            };
            _pFrame.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly))
                    {Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter()});
            _pFrame.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) {Source = this, Mode = BindingMode.OneWay});
            _pFrame.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});

            TextAlignmentCommand = new Command(() => TextAlignmentChanged());

            _datePicker.Focused += FocusEntry;
            _datePicker.Unfocused += UnfocusEntry;
            _datePicker.DateSelected += Date_Selected;

            _datePicker.DoneClicked += (sender, e) => DoneClicked?.Invoke(this, e);
            _datePicker.CancelClicked += (sender, e) => CancelClicked?.Invoke(this, e);

            Children.Add(_pFrame, 2, 3, 1, 2);
        }

        public DateTime Date
        {
            get => (DateTime) GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public string Format
        {
            get => (string) GetValue(DatePicker.FormatProperty);
            set => SetValue(DatePicker.FormatProperty, value);
        }

        public DateTime MaximumDate
        {
            get => (DateTime) GetValue(MaximumDateProperty);
            set => SetValue(MaximumDateProperty, value);
        }

        public DateTime MinimumDate
        {
            get => (DateTime) GetValue(MinimumDateProperty);
            set => SetValue(MinimumDateProperty, value);
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

        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;

        private void TextAlignmentChanged()
        {
            switch (EntryHorizontalTextAlignment)
            {
                case TextAlignment.Center:
                    _pFrame.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    break;
                case TextAlignment.End:
                    _pFrame.HorizontalOptions = LayoutOptions.EndAndExpand;
                    break;
                case TextAlignment.Start:
                    _pFrame.HorizontalOptions = LayoutOptions.StartAndExpand;
                    break;
            }
        }

        public override void Focus()
        {
            _datePicker.Focus();
        }

        public override void Unfocus()
        {
            _datePicker.Unfocus();
        }

        private void Date_Selected(object sender, DateChangedEventArgs e)
        {
            DateSelected?.Invoke(this, e);
        }

        protected override void SetCornerPaddingLayout()
        {
            base.SetCornerPaddingLayout();

            if (EntryCornerRadius >= 1f)
            {
                var thick = 0.0;
                if (BorderRelative) thick = Convert.ToDouble(EntryCornerRadius);
                _datePicker.Margin = new Thickness(thick, 0, thick, 0);
            }
            else
            {
                _datePicker.Margin = 0;
            }
        }
    }
}