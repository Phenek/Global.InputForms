using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Global.InputForms
{
    public class SliderTick : Grid
    {
        /// <summary>
        ///     The Items Source property.
        /// </summary>
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IDictionary<double, string>), typeof(SliderTick), null, propertyChanged: OnItemsSourceChanged);

        /// <summary>
        ///     The Thumb color property.
        /// </summary>
        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor),
            typeof(Color), typeof(SliderTick), Colors.Blue);

        /// <summary>
        ///     The MinimumTrack color property.
        /// </summary>
        public static readonly BindableProperty MinimumTrackColorProperty = BindableProperty.Create(
            nameof(MinimumTrackColor),
            typeof(Color), typeof(SliderTick), Colors.Blue);

        /// <summary>
        ///     The MaximumTrack color property.
        /// </summary>
        public static readonly BindableProperty MaximumTrackColorProperty = BindableProperty.Create(
            nameof(MaximumTrackColor),
            typeof(Color), typeof(SliderTick), Colors.Gray);

        /// <summary>
        ///     The Tick color property.
        /// </summary>
        public static readonly BindableProperty TickColorProperty = BindableProperty.Create(nameof(TickColor),
            typeof(Color), typeof(SliderTick), Colors.Gray);

        /// <summary>
        ///     The Label color property.
        /// </summary>
        public static readonly BindableProperty LabelColorProperty = BindableProperty.Create(nameof(LabelColor),
            typeof(Color), typeof(SliderTick), Colors.Black);

        /// <summary>
        ///     The Label Font Size property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize),
            typeof(double), typeof(SliderTick), Device.GetNamedSize(NamedSize.Micro, typeof(Label)));

        /// <summary>
        ///     The Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes),
            typeof(FontAttributes), typeof(SliderTick), FontAttributes.Bold);

        /// <summary>
        ///     The Label Font Family property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily),
            typeof(string), typeof(SliderTick), string.Empty);

        /// <summary>
        ///     The Image Unchecked property.
        /// </summary>
        public static readonly BindableProperty ThumbImageSourceProperty = BindableProperty.Create(nameof(ThumbImageSource),
            typeof(ImageSource), typeof(SliderTick), default(ImageSource));

        /// <summary>
        ///     The Image Unchecked property.
        /// </summary>
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value),
            typeof(double), typeof(SliderTick), null, propertyChanged: ValueHasChanged);

        private readonly Slider _slider;

        public SliderTick()
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
            };

            _slider = new Slider
            {
                Margin = new Thickness(-20, 0, -20, 0)
            };

            _slider.SetBinding(Slider.ThumbColorProperty,
                new Binding(nameof(ThumbColor)) {Source = this, Mode = BindingMode.OneWay});
            _slider.SetBinding(Slider.MinimumTrackColorProperty,
                new Binding(nameof(MinimumTrackColor)) {Source = this, Mode = BindingMode.OneWay});
            _slider.SetBinding(Slider.MaximumTrackColorProperty,
                new Binding(nameof(MaximumTrackColor)) {Source = this, Mode = BindingMode.OneWay});
            _slider.SetBinding(Slider.ThumbImageSourceProperty,
                new Binding(nameof(ThumbImageSource)) {Source = this, Mode = BindingMode.OneWay});

            _slider.ValueChanged += _slider_ValueChanged;

            Children.Add(_slider);
        }

        /// <summary>
        ///     Gets or sets the Items Source.
        /// </summary>
        /// <value>The Check group Items Source.</value>
        public IDictionary<double, string> ItemsSource
        {
            get => (IDictionary<double, string>) GetValue(ItemsSourceProperty);
            set
            {
                if (value != null) SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Thumb Color.
        /// </summary>
        /// <value>The Background Color.</value>
        public Color ThumbColor
        {
            get => (Color) GetValue(ThumbColorProperty);
            set => SetValue(ThumbColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the MinimumTrack Color.
        /// </summary>
        /// <value>The Background Color.</value>
        public Color MinimumTrackColor
        {
            get => (Color) GetValue(MinimumTrackColorProperty);
            set => SetValue(MinimumTrackColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the MaximumTrack Color.
        /// </summary>
        /// <value>The Background Color.</value>
        public Color MaximumTrackColor
        {
            get => (Color) GetValue(MaximumTrackColorProperty);
            set => SetValue(MaximumTrackColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Tick Color.
        /// </summary>
        /// <value>The Background Color.</value>
        public Color TickColor
        {
            get => (Color) GetValue(TickColorProperty);
            set => SetValue(TickColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Label Color.
        /// </summary>
        /// <value>The Background Color.</value>
        public Color LabelColor
        {
            get => (Color) GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font attributes.
        /// </summary>
        /// <value>The label font attributes.</value>
        public FontAttributes FontAttributes
        {
            get => (FontAttributes) GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font family.
        /// </summary>
        /// <value>The label font family.</value>
        public string FontFamily
        {
            get => (string) GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font size.
        /// </summary>
        /// <value>The label font size.</value>
        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Thumb Image Source.
        /// </summary>
        /// <value>The unchecked image.</value>
        [System.ComponentModel.TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource ThumbImageSource
        {
            get => (ImageSource) GetValue(ThumbImageSourceProperty);
            set => SetValue(ThumbImageSourceProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Value.
        /// </summary>
        /// <value>The label font size.</value>
        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        private void _slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double nbTicks = ItemsSource.Count - 1;

            var aTick = 1 / nbTicks;
            var val = Math.Round(e.NewValue / aTick);

            _slider.Value = val * aTick;

            var index = Convert.ToInt32(val);
            var array = ItemsSource.ToArray();
            Value = array[index].Key;
        }

        /// <summary>
        ///     The Items Source changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is SliderTick sliderTick) || sliderTick.ItemsSource == null) return;

            sliderTick.GenerateTickList();
        }

        private void GenerateTickList()
        {
            ColumnDefinitions.Clear();

            var index = 0;
            foreach (var tick in ItemsSource)
            {
                ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
                ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
                var dash = new BoxView
                {
                    HeightRequest = 4,
                    WidthRequest = 2,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start
                };
                var label = new Label
                {
                    Text = tick.Value,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End
                };

                dash.SetBinding(BackgroundColorProperty,
                    new Binding(nameof(TickColor)) {Source = this, Mode = BindingMode.OneWay});

                label.SetBinding(Label.TextColorProperty,
                    new Binding(nameof(LabelColor)) {Source = this, Mode = BindingMode.OneWay});
                label.SetBinding(Label.FontSizeProperty,
                    new Binding(nameof(FontSize)) {Source = this, Mode = BindingMode.OneWay});
                label.SetBinding(Label.FontFamilyProperty,
                    new Binding(nameof(FontFamily)) {Source = this, Mode = BindingMode.OneWay});
                label.SetBinding(Label.FontAttributesProperty,
                    new Binding(nameof(FontAttributes)) {Source = this, Mode = BindingMode.OneWay});

                SetRow((BindableObject)dash, 1);
                SetRow((BindableObject)label, 2);

                SetColumn((BindableObject)dash, index);
                SetColumn((BindableObject)label, index);

                SetColumnSpan((BindableObject)dash, 2);
                SetColumnSpan((BindableObject)label, 2);

                Children.Add(dash);
                Children.Add(label);

                index += 2;
            }

            SetColumn((BindableObject)_slider, 1);
            SetColumnSpan((BindableObject)_slider, ColumnDefinitions.Count - 2);

            if (ItemsSource.ContainsKey(Value))
            {
                var array = ItemsSource.Keys.ToArray();
                var indexVal = Array.IndexOf(array, Value);

                double nbTicks = ItemsSource.Count - 1;
                var aTick = 1 / nbTicks;

                _slider.Value = indexVal * aTick;
            }
        }

        private static void ValueHasChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SliderTick sliderTick)
                if (oldValue != newValue)
                    sliderTick.ValueChanged?.Invoke(sliderTick,
                        new ValueChangedEventArgs((double) oldValue, (double) newValue));
        }
    }
}