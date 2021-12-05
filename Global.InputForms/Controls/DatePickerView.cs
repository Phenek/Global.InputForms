using System;
using System.Windows.Input;
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
            typeof(DatePickerView), default, BindingMode.TwoWay, coerceValue: CoerceDate, propertyChanged: DateChanged);

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string),
            typeof(DatePickerView), string.Empty, propertyChanged: FormatChanged);

        /// <summary>
        ///     The Minimum Date property.
        /// </summary>
        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate),
            typeof(DateTime), typeof(DatePickerView), new DateTime(1900, 1, 1),
            validateValue: ValidateMinimumDate, coerceValue: CoerceMinimumDate, propertyChanged: MinimumDateChanged);

        /// <summary>
        ///     The Maximum Date property.
        /// </summary>
        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate),
            typeof(DateTime), typeof(DatePickerView), new DateTime(2100, 12, 31),
            validateValue: ValidateMaximumDate, coerceValue: CoerceMaximumDate, propertyChanged: MaximumDateChanged);

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(DatePickerView), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(DatePickerView), "Cancel");

        public static readonly BindableProperty UpdateModeProperty =
            BindableProperty.Create(nameof(UpdateMode), typeof(UpdateMode), typeof(DatePickerView),
                UpdateMode.Immediately);

        private readonly BlankDatePicker _datePicker;
        private readonly Frame _pFrame;

        public EventHandler<DateChangedEventArgs> DateSelected;

        public DatePickerView()
        {
            _datePicker = new BlankDatePicker
            {
                BackgroundColor = Color.Transparent
            };
            Input = _datePicker;
            _datePicker.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryText)) {Source = this, Mode = BindingMode.TwoWay});
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
                new Binding(nameof(EntryMargin)) {Source = this, Mode = BindingMode.OneWay});

            //_datePicker.SetBinding(BlankDatePicker.FormatProperty,
            //    new Binding(nameof(Format)) { Source = this, Mode = BindingMode.OneWay });
            //_datePicker.SetBinding(BlankDatePicker.MinimumDateProperty,
            //    new Binding(nameof(MinimumDate)) { Source = this, Mode = BindingMode.OneWay });
            //_datePicker.SetBinding(BlankDatePicker.MaximumDateProperty,
            //    new Binding(nameof(MaximumDate)) { Source = this, Mode = BindingMode.OneWay });
            //_datePicker.SetBinding(BlankDatePicker.DateProperty,
            //    new Binding(nameof(Date)) { Source = this, Mode = BindingMode.TwoWay });

            _datePicker.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly))
                    {Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter()});
            _datePicker.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});

            _datePicker.SetBinding(BlankDatePicker.DoneButtonTextProperty,
                new Binding(nameof(DoneButtonText)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.CancelButtonTextProperty,
                new Binding(nameof(CancelButtonText)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.UpdateModeProperty,
                new Binding(nameof(UpdateMode)) {Source = this, Mode = BindingMode.OneWay});


            _datePicker.Focused += FocusEntry;
            _datePicker.Unfocused += UnfocusEntry;
            _datePicker.DateSelected += Date_Selected;
            _datePicker.TextChanged += SendEntryTextChanged;

            FloatingLabelWithoutAnimation();

            Children.Add(_datePicker, 2, 3, 1, 2);
        }

        public UpdateMode UpdateMode
        {
            get => (UpdateMode) GetValue(UpdateModeProperty);
            set => SetValue(UpdateModeProperty, value);
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

        private static void DateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView picker)
                picker._datePicker.Date = (DateTime) newValue;
        }

        private static void FormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView picker)
                picker._datePicker.Format = (string) newValue;
        }

        private static void MinimumDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView picker)
                picker._datePicker.MinimumDate = (DateTime) newValue;
        }

        private static void MaximumDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView picker)
                picker._datePicker.MaximumDate = (DateTime) newValue;
        }

        public event EventHandler DoneClicked
        {
            add => _datePicker.DoneClicked += value;
            remove => _datePicker.DoneClicked -= value;
        }

        public event EventHandler CancelClicked
        {
            add => _datePicker.CancelClicked += value;
            remove => _datePicker.CancelClicked -= value;
        }

        private static object CoerceDate(BindableObject bindable, object value)
        {
            var picker = (DatePickerView) bindable;
            var dateValue = ((DateTime) value).Date;

            if (dateValue > picker.MaximumDate && picker.Date != default)
                dateValue = picker.MaximumDate;

            if (dateValue < picker.MinimumDate && picker.Date != default)
                dateValue = picker.MinimumDate;

            return dateValue;
        }

        private static object CoerceMaximumDate(BindableObject bindable, object value)
        {
            var dateValue = ((DateTime) value).Date;
            var picker = (DatePickerView) bindable;

            if (picker.Date > dateValue && picker.Date != default)
                picker.Date = dateValue;

            return dateValue;
        }

        private static object CoerceMinimumDate(BindableObject bindable, object value)
        {
            var dateValue = ((DateTime) value).Date;
            var picker = (DatePickerView) bindable;

            if (picker.Date < dateValue && picker.Date != default)
                picker.Date = dateValue;

            return dateValue;
        }

        private static bool ValidateMaximumDate(BindableObject bindable, object value)
        {
            if (((DatePickerView) bindable)._datePicker.Date == new DateTime(42, 1, 1))
                return true;
            return ((DateTime) value).Date >= ((DatePickerView) bindable).MinimumDate.Date;
        }

        private static bool ValidateMinimumDate(BindableObject bindable, object value)
        {
            if (((DatePickerView) bindable)._datePicker.Date == new DateTime(42, 1, 1))
                return true;
            return ((DateTime) value).Date <= ((DatePickerView) bindable).MaximumDate.Date;
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
            if (e.NewDate != Date)
                Date = e.NewDate;
            DateSelected?.Invoke(this, e);
        }
    }
}