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
            typeof(TimeSpan), typeof(TimePickerView), new TimeSpan());

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string),
            typeof(TimePickerView), string.Empty);

        private readonly BlankTimePicker _timePicker;

        public TimePickerView()
        {
            _timePicker = new BlankTimePicker
            {
                HeightRequest = 40,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _timePicker.SetBinding(TimePicker.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(TimePicker.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(TimePicker.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(BlankTimePicker.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(BlankTimePicker.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(TimePicker.TextColorProperty,
                new Binding(nameof(EntryTextColor)) { Source = this, Mode = BindingMode.OneWay });

            _timePicker.SetBinding(TimePicker.FormatProperty,
                new Binding(nameof(Format)) { Source = this, Mode = BindingMode.OneWay });
            _timePicker.SetBinding(TimePicker.TimeProperty,
                new Binding(nameof(Time)) { Source = this, Mode = BindingMode.TwoWay });

            var fEntry = new Frame
            {
                Padding = 0,
                HeightRequest = 40,
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                Content = _timePicker
            };
            fEntry.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter() });
            fEntry.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay});

            _timePicker.Focused += FocusEntry;
            _timePicker.Unfocused += UnfocusEntry;

            Children.Add(fEntry, 2, 3, 1, 2);
        }

        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public string Format
        {
            get => (string)GetValue(TimePicker.FormatProperty);
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
    }
}
