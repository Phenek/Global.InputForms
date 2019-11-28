using System;
using Global.InputForms.Converters;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class TimePickerView : EntryLayout
    {
        /// <summary>
        ///     The Minimum Date property.
        /// </summary>
        public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time),
            typeof(TimeSpan), typeof(TimePickerView), new TimeSpan(0), propertyChanged: TimeChanged, coerceValue: CoerceDate);

        static object CoerceDate(BindableObject bindable, object value)
        {
            var val = (TimeSpan)value;
            TimeSpan timeValue = new TimeSpan(val.Hours, val.Minutes, 0);
            return timeValue;
        }

        private static void TimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TimePickerView picker)
                picker._timePicker.Time = (TimeSpan)newValue;
        }

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string),
            typeof(TimePickerView), @"H\:mm", propertyChanged: FormatChanged);
        
        private static void FormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TimePickerView picker)
                picker._timePicker.Format = (string)newValue;
        }

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(DatePickerView), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(DatePickerView), "Cancel");

        private readonly Frame _pFrame;
        private readonly BlankTimePicker _timePicker;

        public event EventHandler<TimeChangedEventArgs> TimeSelected;

        public TimePickerView()
        {
            _timePicker = new BlankTimePicker
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Input = _timePicker;
            _timePicker.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryText)) { Source = this, Mode = BindingMode.TwoWay });
            _timePicker.SetBinding(Entry.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(Entry.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(Entry.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(Entry.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(Entry.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(Entry.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(Entry.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});
            _timePicker.SetBinding(MarginProperty,
                new Binding(nameof(EntryMargin)) { Source = this, Mode = BindingMode.OneWay });

            _timePicker.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly))
                { Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter() });
            _timePicker.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) { Source = this, Mode = BindingMode.OneWay });

            //_timePicker.SetBinding(BlankTimePicker.FormatProperty,
            //    new Binding(nameof(Format)) { Source = this, Mode = BindingMode.OneWay });
            //_timePicker.SetBinding(BlankTimePicker.TimeProperty,
            //    new Binding(nameof(Time)) { Source = this, Mode = BindingMode.TwoWay });


            _timePicker.SetBinding(BlankTimePicker.DoneButtonTextProperty,
                new Binding(nameof(DoneButtonText)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(BlankTimePicker.CancelButtonTextProperty,
                new Binding(nameof(CancelButtonText)) { Source = this, Mode = BindingMode.OneWay });

            _timePicker.Focused += FocusEntry;
            _timePicker.Unfocused += UnfocusEntry;
            _timePicker.TextChanged += SendEntryTextChanged;
            _timePicker.TimeSelected += Time_Selected;

            Children.Add(_timePicker, 2, 3, 1, 2);
        }

        public TimeSpan Time
        {
            get => (TimeSpan) GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public string Format
        {
            get => (string) GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }

        public string DoneButtonText
        {
            get => (string)GetValue(DoneButtonTextProperty);
            set => SetValue(DoneButtonTextProperty, value);
        }

        public string CancelButtonText
        {
            get => (string)GetValue(CancelButtonTextProperty);
            set => SetValue(CancelButtonTextProperty, value);
        }

        public override void Focus()
        {
            _timePicker.Focus();
        }

        public override void Unfocus()
        {
            _timePicker.Unfocus();
        }

        private void Time_Selected(object sender, TimeChangedEventArgs e)
        {
            if (e.NewTime != Time)
                Time = e.NewTime;
            TimeSelected?.Invoke(this, e);
        }
    }
}