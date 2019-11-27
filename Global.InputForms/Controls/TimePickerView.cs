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
            typeof(TimeSpan), typeof(TimePickerView), null);
        //    propertyChanged: TimeChanged);

        //private static void TimeChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    if (bindable is TimePickerView picker)
        //        picker._timePicker.Time = (TimeSpan)newValue;
        //}

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string),
            typeof(TimePickerView), string.Empty);
        //, propertyChanged: FormatChanged);

        //private static void FormatChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    if (bindable is TimePickerView picker)
        //        picker._timePicker.Format = (string)newValue;
        //}

        private readonly Frame _pFrame;
        private readonly BlankTimePicker _timePicker;

        public event EventHandler<NullableTimeChangedEventArgs> TimeSelected;

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

            _timePicker.SetBinding(BlankTimePicker.FormatProperty,
                new Binding(nameof(Format)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(BlankTimePicker.TimeProperty,
                new Binding(nameof(Time)) { Source = this, Mode = BindingMode.TwoWay });


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
            get => (string) GetValue(TimePicker.FormatProperty);
            set => SetValue(TimePicker.FormatProperty, value);
        }

        public override void Focus()
        {
            _timePicker.Focus();
        }

        public override void Unfocus()
        {
            _timePicker.Unfocus();
        }

        private void Time_Selected(object sender, NullableTimeChangedEventArgs e)
        {
            //Time = e.NewTime;
            TimeSelected?.Invoke(this, e);
        }
    }
}