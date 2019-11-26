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
            };
            _datePicker.GestureRecognizers.Add(TapGesture);
            _datePicker.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryText)) { Source = this, Mode = BindingMode.TwoWay });
            _datePicker.SetBinding(Entry.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(Entry.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(Entry.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(Entry.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(Entry.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(Entry.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(Entry.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(MarginProperty,
                new Binding(nameof(EntryMargin)) { Source = this, Mode = BindingMode.OneWay });

            _datePicker.SetBinding(BlankDatePicker.FormatProperty,
                new Binding(nameof(Format)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.MinimumDateProperty,
                new Binding(nameof(MinimumDate)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.MaximumDateProperty,
                new Binding(nameof(MaximumDate)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.DateProperty,
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
            Input = _pFrame;
            _pFrame.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly))
                { Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter() });
            _pFrame.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) {Source = this, Mode = BindingMode.OneWay});
            _pFrame.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});

            TextAlignmentCommand = new Command(() => TextAlignmentChanged());

            _datePicker.Focused += FocusEntry;
            _datePicker.Unfocused += UnfocusEntry;
            _datePicker.DateSelected += Date_Selected;
            _datePicker.TextChanged += SendEntryTextChanged;

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
    }
}