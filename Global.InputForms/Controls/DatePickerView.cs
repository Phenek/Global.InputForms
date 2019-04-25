using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Global.InputForms
{
    [ContentProperty(nameof(Children))]
    public class DatePickerView : StackLayout
    {
        /// <summary>
        ///     The Date property.
        /// </summary>
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime),
            typeof(DatePickerView), DateTime.Today,
            BindingMode.Default, null, (bindable, oldValue, newValue) =>
            {
                if (bindable is DatePickerView view)
                    view._datePicker.Date = (DateTime) newValue;
            });

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string),
            typeof(DatePickerView), string.Empty,
            BindingMode.Default, null, (bindable, oldValue, newValue) =>
            {
                if (bindable is DatePickerView view)
                    view._datePicker.Format = (string) newValue;
            });

        /// <summary>
        ///     The Minimum Date property.
        /// </summary>
        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate),
            typeof(DateTime), typeof(DatePickerView), new DateTime(1900, 1, 1),
            BindingMode.Default, null, (bindable, oldValue, newValue) =>
            {
                if (bindable is DatePickerView view)
                    view._datePicker.MinimumDate = (DateTime) newValue;
            });


        /// <summary>
        ///     The Maximum Date property.
        /// </summary>
        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate),
            typeof(DateTime), typeof(DatePickerView), new DateTime(2100, 12, 31),
            BindingMode.Default, null, (bindable, oldValue, newValue) =>
            {
                if (bindable is DatePickerView view)
                    view._datePicker.MaximumDate = (DateTime) newValue;
            });

        /// <summary>
        ///     The Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty LabelFontAttributesProperty =
            BindableProperty.Create(nameof(LabelFontAttributes), typeof(FontAttributes), typeof(DatePickerView),
                FontAttributes.Bold);

        /// <summary>
        ///     The Label Font Family property.
        /// </summary>
        public static readonly BindableProperty LabelFontFamilyProperty =
            BindableProperty.Create(nameof(LabelFontFamily), typeof(string), typeof(DatePickerView), string.Empty);

        /// <summary>
        ///     The Label Font Size property.
        /// </summary>
        public static readonly BindableProperty LabelFontSizeProperty =
            BindableProperty.Create(nameof(LabelFontSize), typeof(double), typeof(DatePickerView),
                Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        /// <summary>
        ///     The Label Horizontal TextAlignment property.
        /// </summary>
        public static readonly BindableProperty LabelHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelHorizontalTextAlignment), typeof(TextAlignment), typeof(DatePickerView),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty LabelVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelVerticalTextAlignment), typeof(TextAlignment), typeof(DatePickerView),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Text property.
        /// </summary>
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(DatePickerView), string.Empty,
                propertyChanged: LabelTextChanged);

        /// <summary>
        ///     The Label Highlighted Color property.
        /// </summary>
        public static readonly BindableProperty LabelHighlightedColorProperty =
            BindableProperty.Create(nameof(LabelHighlightedColor), typeof(Color), typeof(DatePickerView), Color.Gray,
                propertyChanged: LabelHighlightedColorChanged);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty LabelTextColorProperty =
            BindableProperty.Create(nameof(LabelTextColor), typeof(Color), typeof(DatePickerView), Color.Black,
                propertyChanged: LabelTextColorChanged);

        /// <summary>
        ///     The Entry Corner Radius property.
        /// </summary>
        public static readonly BindableProperty EntryCornerRadiusProperty =
            BindableProperty.Create(nameof(EntryCornerRadius), typeof(float), typeof(DatePickerView), 0f,
                propertyChanged: EntryCornerRadiusChanged);

        /// <summary>
        ///     The Entry Border Color property.
        /// </summary>
        public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(nameof(EntryBorderColor), typeof(Color), typeof(DatePickerView), Color.Transparent,
                propertyChanged: EntryBorderColorChanged);


        /// <summary>
        ///     The Entry Background Color property.
        /// </summary>
        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(DatePickerView),
                Color.Transparent, propertyChanged: EntryBackgroundColorChanged);

        /// <summary>
        ///     The Entry Font Attributes property.
        /// </summary>
        public static readonly BindableProperty EntryFontAttributesProperty =
            BindableProperty.Create(nameof(EntryFontAttributes), typeof(FontAttributes), typeof(DatePickerView),
                FontAttributes.None);

        /// <summary>
        ///     The Entry Font Family property.
        /// </summary>
        public static readonly BindableProperty EntryFontFamilyProperty =
            BindableProperty.Create(nameof(EntryFontFamily), typeof(string), typeof(DatePickerView), string.Empty);

        /// <summary>
        ///     The Entry Font Size property.
        /// </summary>
        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(nameof(EntryFontSize), typeof(double), typeof(DatePickerView),
                Device.GetNamedSize(NamedSize.Medium, typeof(Entry)));

        /// <summary>
        ///     The Entry Horizontal Text Alignment property.
        /// </summary>
        public static readonly BindableProperty EntryHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(EntryHorizontalTextAlignment), typeof(TextAlignment), typeof(DatePickerView),
                TextAlignment.Start);

        /// <summary>
        ///     The Entry Placeholder property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderProperty =
            BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(DatePickerView), string.Empty);

        /// <summary>
        ///     The Entry Placeholder Color property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderColorProperty =
            BindableProperty.Create(nameof(EntryPlaceholderColor), typeof(Color), typeof(DatePickerView), Color.Black);

        /// <summary>
        ///     The Entry Text Color property.
        /// </summary>
        public static readonly BindableProperty EntryTextColorProperty =
            BindableProperty.Create(nameof(EntryTextColor), typeof(Color), typeof(DatePickerView), Color.Black);

        /// <summary>
        ///     The IsEnabled property.
        /// </summary>
        public static readonly BindableProperty EntryIsEnabledProperty =
            BindableProperty.Create(nameof(EntryIsEnabled), typeof(bool), typeof(EntryView), true,
                propertyChanged: EntryIsEnabledChanged);

        /// <summary>
        ///     The Info View Type property.
        /// </summary>
        public static readonly BindableProperty InfoViewTypeProperty =
            BindableProperty.Create(nameof(InfoViewType), typeof(InfoViewType), typeof(DatePickerView),
                InfoViewType.Line, propertyChanged: InfoViewTypeChanged);

        /// <summary>
        ///     The Info Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontAttributesProperty =
            BindableProperty.Create(nameof(InfoLabelFontAttributes), typeof(FontAttributes), typeof(DatePickerView),
                FontAttributes.Bold);

        /// <summary>
        ///     The Info Label Font Family property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontFamilyProperty =
            BindableProperty.Create(nameof(InfoLabelFontFamily), typeof(string), typeof(DatePickerView), string.Empty);

        /// <summary>
        ///     The Info Label Font Size property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontSizeProperty =
            BindableProperty.Create(nameof(InfoLabelFontSize), typeof(double), typeof(DatePickerView),
                Device.GetNamedSize(NamedSize.Micro, typeof(Label)));

        /// <summary>
        ///     The Info Label Horizontal TextAlignment property.
        /// </summary>
        public static readonly BindableProperty InfoLabelHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoLabelHorizontalTextAlignment), typeof(TextAlignment),
                typeof(DatePickerView), TextAlignment.Start);

        /// <summary>
        ///     The Info Label Vertical Text Alignment property.
        /// </summary=
        public static readonly BindableProperty InfoLabelVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoLabelVerticalTextAlignment), typeof(TextAlignment),
                typeof(DatePickerView), TextAlignment.Start);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty InfoColorProperty =
            BindableProperty.Create(nameof(InfoColor), typeof(Color), typeof(DatePickerView), Color.Red);

        /// <summary>
        ///     The Info Label Text property.
        /// </summary>
        public static readonly BindableProperty InfoLabelTextProperty =
            BindableProperty.Create(nameof(InfoLabelText), typeof(string), typeof(DatePickerView), string.Empty,
                propertyChanged: InfoLabelTextChanged);

        /// <summary>
        ///     The Info is visible property.
        /// </summary>
        public static readonly BindableProperty InfoIsVisibleProperty =
            BindableProperty.Create(nameof(InfoIsVisible), typeof(bool), typeof(DatePickerView), false,
                propertyChanged: InfoIsVisibleChanged);

        /// <summary>
        ///     The Info is visible property.
        /// </summary>
        public static readonly BindableProperty InfoProperty =
            BindableProperty.Create(nameof(Info), typeof(bool), typeof(DatePickerView), false);

        private readonly BlankDatePicker _datePicker;
        private readonly Frame _frameEntry;
        private readonly Grid _gridEntry;

        private Label _infoLabel;
        private StackLayout _infoLayout;
        private BoxView _infoLine;
        private Label _label;
        public EventHandler<DateChangedEventArgs> DateSelected;
        public EventHandler<EventArgs> Validators;

        public IList<View> StackChildren => base.Children;
        public new IList<View> Children => _gridEntry.Children;

        public DatePickerView()
        {
            Orientation = StackOrientation.Vertical;
            Spacing = 0;
            _datePicker = new BlankDatePicker
            {
                HeightRequest = 40,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _datePicker.SetBinding(DatePicker.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(BlankDatePicker.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.OneWay});
                
            _datePicker.SetBinding(DatePicker.FormatProperty,
                new Binding(nameof(Format)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.MinimumDateProperty,
                new Binding(nameof(MinimumDate)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.MaximumDateProperty,
                new Binding(nameof(MaximumDate)) {Source = this, Mode = BindingMode.OneWay});
            _datePicker.SetBinding(DatePicker.DateProperty,
                new Binding(nameof(Date)) { Source = this, Mode = BindingMode.TwoWay });

            _frameEntry = new Frame
            {
                Padding = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = EntryBackgroundColor,
                CornerRadius = EntryCornerRadius,
                BorderColor = EntryBorderColor,
                HasShadow = false,
            };
            _frameEntry.SetBinding(Frame.CornerRadiusProperty,
                new Binding(nameof(EntryCornerRadius)) {Source = this, Mode = BindingMode.OneWay});

            _gridEntry = new Grid
            {
                ColumnSpacing = 1,
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
                }
            };
            _gridEntry.SetBinding(IsEnabledProperty,
                new Binding(nameof(EntryIsEnabled)) { Source = this, Mode = BindingMode.OneWay });

            _gridEntry.Children.Add(_frameEntry, 1, 4, 0, 1);
            _gridEntry.Children.Add(_datePicker, 2, 0);

            _datePicker.Focused += Date_Focused;
            _datePicker.Unfocused += Date_Unfocused;
            _datePicker.DateSelected += Date_Selected;

            Unfocused += (sender, e) => Validate();

            StackChildren.Add(_gridEntry);
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

        /// <summary>
        ///     Gets or sets the label font attributes.
        /// </summary>
        /// <value>The label font attributes.</value>
        public FontAttributes LabelFontAttributes
        {
            get => (FontAttributes) GetValue(LabelFontAttributesProperty);
            set => SetValue(LabelFontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font family.
        /// </summary>
        /// <value>The label font family.</value>
        public string LabelFontFamily
        {
            get => (string) GetValue(LabelFontFamilyProperty);
            set => SetValue(LabelFontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font size.
        /// </summary>
        /// <value>The label font size.</value>
        public double LabelFontSize
        {
            get => (double) GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label horizontal text alignment.
        /// </summary>
        /// <value>The label horizontal text alignment.</value>
        public TextAlignment LabelHorizontalTextAlignment
        {
            get => (TextAlignment) GetValue(LabelHorizontalTextAlignmentProperty);
            set => SetValue(LabelHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label vertical text alignment.
        /// </summary>
        /// <value>The label vertical text alignment.</value>
        public TextAlignment LabelVerticalTextAlignment
        {
            get => (TextAlignment) GetValue(LabelVerticalTextAlignmentProperty);
            set => SetValue(LabelVerticalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text.
        /// </summary>
        /// <value>The label text.</value>
        public string LabelText
        {
            get => (string) GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color LabelHighlightedColor
        {
            get => (Color) GetValue(LabelHighlightedColorProperty);
            set => SetValue(LabelHighlightedColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color LabelTextColor
        {
            get => (Color) GetValue(LabelTextColorProperty);
            set => SetValue(LabelTextColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry corner radius.
        /// </summary>
        /// <value>The entry background color.</value>
        public float EntryCornerRadius
        {
            get => (float) GetValue(EntryCornerRadiusProperty);
            set => SetValue(EntryCornerRadiusProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry border color.
        /// </summary>
        /// <value>The entry background color.</value>
        public Color EntryBorderColor
        {
            get => (Color) GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry background color.
        /// </summary>
        /// <value>The entry background color.</value>
        public Color EntryBackgroundColor
        {
            get => (Color) GetValue(EntryBackgroundColorProperty);
            set => SetValue(EntryBackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry font attributes.
        /// </summary>
        /// <value>The entry font attributes.</value>
        public FontAttributes EntryFontAttributes
        {
            get => (FontAttributes) GetValue(EntryFontAttributesProperty);
            set => SetValue(EntryFontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry font family.
        /// </summary>
        /// <value>The entry font family.</value>
        public string EntryFontFamily
        {
            get => (string) GetValue(EntryFontFamilyProperty);
            set => SetValue(EntryFontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry font size.
        /// </summary>
        /// <value>The entry font size.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double EntryFontSize
        {
            get => (double) GetValue(EntryFontSizeProperty);
            set => SetValue(EntryFontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry horizontal text alignment.
        /// </summary>
        /// <value>The entry horizontal text alignment.</value>
        public TextAlignment EntryHorizontalTextAlignment
        {
            get => (TextAlignment) GetValue(EntryHorizontalTextAlignmentProperty);
            set => SetValue(EntryHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry placeholder.
        /// </summary>
        /// <value>The entry placeholdeer.</value>
        public string EntryPlaceholder
        {
            get => (string) GetValue(EntryPlaceholderProperty);
            set => SetValue(EntryPlaceholderProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry placeholder color.
        /// </summary>
        /// <value>The entry placeholder color.</value>
        public Color EntryPlaceholderColor
        {
            get => (Color) GetValue(EntryPlaceholderColorProperty);
            set => SetValue(EntryPlaceholderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry text color.
        /// </summary>
        /// <value>The entry text color.</value>
        public Color EntryTextColor
        {
            get => (Color) GetValue(EntryTextColorProperty);
            set => SetValue(EntryTextColorProperty, value);
        }

        public bool EntryIsEnabled
        {
            get => (bool) GetValue(EntryIsEnabledProperty);
            set => SetValue(EntryIsEnabledProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info view type  value.
        /// </summary>
        /// <value>the Keyboard value.</value>
        public InfoViewType InfoViewType
        {
            get => (InfoViewType) GetValue(InfoViewTypeProperty);
            set => SetValue(InfoViewTypeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label font attributes.
        /// </summary>
        /// <value>The info label font attributes.</value>
        public FontAttributes InfoLabelFontAttributes
        {
            get => (FontAttributes) GetValue(InfoLabelFontAttributesProperty);
            set => SetValue(InfoLabelFontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label font family.
        /// </summary>
        /// <value>The info label font family.</value>
        public string InfoLabelFontFamily
        {
            get => (string) GetValue(InfoLabelFontFamilyProperty);
            set => SetValue(InfoLabelFontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label font size.
        /// </summary>
        /// <value>The info label font size.</value>
        public double InfoLabelFontSize
        {
            get => (double) GetValue(InfoLabelFontSizeProperty);
            set => SetValue(InfoLabelFontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label horizontal text alignment.
        /// </summary>
        /// <value>The info label horizontal text alignment.</value>
        public TextAlignment InfoLabelHorizontalTextAlignment
        {
            get => (TextAlignment) GetValue(InfoLabelHorizontalTextAlignmentProperty);
            set => SetValue(InfoLabelHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label vertical text alignment.
        /// </summary>
        /// <value>The info label vertical text alignment.</value>
        public TextAlignment InfoLabelVerticalTextAlignment
        {
            get => (TextAlignment) GetValue(InfoLabelVerticalTextAlignmentProperty);
            set => SetValue(InfoLabelVerticalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label text.
        /// </summary>
        /// <value>The info label text.</value>
        public string InfoLabelText
        {
            get => (string) GetValue(InfoLabelTextProperty);
            set => SetValue(InfoLabelTextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label text color.
        /// </summary>
        /// <value>The info label text color.</value>
        public Color InfoColor
        {
            get => (Color) GetValue(InfoColorProperty);
            set => SetValue(InfoColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info visibility.
        /// </summary>
        /// <value>The info visibility.</value>
        public bool InfoIsVisible
        {
            get => (bool) GetValue(InfoIsVisibleProperty);
            set => SetValue(InfoIsVisibleProperty, value);
        }

        /// <summary>
        ///     Gets or sets the boolean info .
        /// </summary>
        /// <value>The boolean info.</value>
        public bool Info
        {
            get => (bool) GetValue(InfoProperty);
            set => SetValue(InfoProperty, value);
        }

        public new bool IsFocused { get; set; }

        public new event EventHandler<FocusEventArgs> Focused;
        public new event EventHandler<FocusEventArgs> Unfocused;


        /// <summary>
        ///     The Label Text property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DatePickerView datePickerView) || datePickerView._label != null) return;
            
            datePickerView._label = new Label
            {
                TextColor = datePickerView.LabelTextColor
            };

            datePickerView._label.SetBinding(Label.FontAttributesProperty,
                new Binding(nameof(LabelFontAttributes))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

            datePickerView._label.SetBinding(Label.FontFamilyProperty, new Binding(nameof(LabelFontFamily))
                {Source = datePickerView, Mode = BindingMode.OneWay});

            datePickerView._label.SetBinding(Label.FontSizeProperty, new Binding(nameof(LabelFontSize))
                {Source = datePickerView, Mode = BindingMode.OneWay});

            datePickerView._label.SetBinding(Label.HorizontalTextAlignmentProperty,
                new Binding(nameof(LabelHorizontalTextAlignment))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

            datePickerView._label.SetBinding(Label.VerticalTextAlignmentProperty,
                new Binding(nameof(LabelVerticalTextAlignment))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

            datePickerView._label.SetBinding(Label.TextProperty, new Binding(nameof(LabelText))
                {Source = datePickerView, Mode = BindingMode.OneWay});

            datePickerView.SetCornerPaddingLayout();
            datePickerView.StackChildren.Insert(0, datePickerView._label);
        }

        /// <summary>
        ///     The Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelHighlightedColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView datePickerView && datePickerView._label != null && datePickerView.IsFocused)
                datePickerView._label.TextColor = (Color) newValue;
        }

        /// <summary>
        ///     The Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView datePickerView && datePickerView._label != null &&
                !datePickerView.IsFocused)
                datePickerView._label.TextColor = (Color) newValue;
        }

        /// <summary>
        ///     The entry corner radius property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DatePickerView datePickerView)) return;
            
            datePickerView._frameEntry.CornerRadius = (float) newValue;
            datePickerView.SetCornerPaddingLayout();
        }

        /// <summary>
        ///     The entry border color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView datePickerView)
                datePickerView._frameEntry.BorderColor = (Color) newValue;
        }

        /// <summary>
        ///     The Entry Background Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView datePickerView)
                datePickerView._frameEntry.BackgroundColor = (Color) newValue;
        }

        private static void EntryIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //Hack for Android Input Text
            if (bindable is DatePickerView datePickerView) datePickerView.InputTransparent = !(bool) newValue;
        }

        /// <summary>
        ///     The Info Label Font Attributes property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoViewTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DatePickerView datePickerView && datePickerView._infoLine != null)
                datePickerView._infoLine.IsVisible = (InfoViewType) newValue == InfoViewType.Line;
        }

        /// <summary>
        ///     The Info Label Text property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DatePickerView datePickerView)) return;
            
            if (datePickerView._infoLabel is Label label)
            {
                label.Text = (string) newValue;
            }
            else
            {
                datePickerView._infoLabel = new Label();
                datePickerView._infoLabel.SetBinding(Label.FontAttributesProperty,
                    new Binding(nameof(InfoLabelFontAttributes))
                        {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLabel.SetBinding(Label.FontFamilyProperty,
                    new Binding(nameof(InfoLabelFontFamily))
                        {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLabel.SetBinding(Label.FontSizeProperty, new Binding(nameof(InfoLabelFontSize))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLabel.SetBinding(Label.HorizontalTextAlignmentProperty,
                    new Binding(nameof(InfoLabelHorizontalTextAlignment))
                        {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLabel.SetBinding(Label.VerticalTextAlignmentProperty,
                    new Binding(nameof(InfoLabelVerticalTextAlignment))
                        {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLabel.SetBinding(Label.TextColorProperty, new Binding(nameof(InfoColor))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLabel.SetBinding(Label.TextProperty, new Binding(nameof(InfoLabelText))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLine = new BoxView
                {
                    HeightRequest = 1,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IsVisible = datePickerView.InfoViewType == InfoViewType.Line
                };
                datePickerView._infoLine.SetBinding(BackgroundColorProperty, new Binding(nameof(InfoColor))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView._infoLayout = new StackLayout
                {
                    Spacing = 0,
                    Orientation = StackOrientation.Vertical
                };
                datePickerView._infoLayout.SetBinding(IsVisibleProperty, new Binding(nameof(Info))
                    {Source = datePickerView, Mode = BindingMode.OneWay});

                datePickerView.SetCornerPaddingLayout();

                datePickerView._infoLayout.Children.Add(datePickerView._infoLine);
                datePickerView._infoLayout.Children.Add(datePickerView._infoLabel);
                datePickerView.StackChildren.Add(datePickerView._infoLayout);
            }
        }

        /// <summary>
        ///     The Info Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DatePickerView datePickerView) || !(datePickerView._infoLayout != null)) return;
            
            datePickerView._infoLayout.IsVisible = (bool) newValue;
            
            switch (datePickerView.InfoViewType)
            {
                case InfoViewType.Surrounded:
                    datePickerView._frameEntry.BorderColor =
                        (bool) newValue ? datePickerView.InfoColor : datePickerView.EntryBorderColor;
                    break;
                case InfoViewType.Background:
                    datePickerView._frameEntry.BackgroundColor = (bool) newValue
                        ? datePickerView.InfoColor
                        : datePickerView.EntryBackgroundColor;
                    break;
                case InfoViewType.Line:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     The Show Info Function.
        /// </summary>
        /// <param name="isVisible">Is visible property</param>
        /// <param name="info">The info boolean.</param>
        /// <param name="info">The information string.</param>
        public void ShowInfo(bool isVisible, string info)
        {
            InfoLabelText = info;
            InfoIsVisible = isVisible;
            Info = isVisible;
        }

        public new void Focus()
        {
            SetValue(IsFocusedProperty, true);
            _datePicker.Focus();
        }

        private void Date_Focused(object sender, FocusEventArgs e)
        {
            IsFocused = true;
            if (_label != null) _label.TextColor = LabelHighlightedColor;
            Focused?.Invoke(this, new FocusEventArgs(this, true));
        }

        private void Date_Unfocused(object sender, FocusEventArgs e)
        {
            IsFocused = false;
            if (_label != null) _label.TextColor = LabelTextColor;
            Unfocused?.Invoke(this, new FocusEventArgs(this, true));
        }

        private void Date_Selected(object sender, DateChangedEventArgs e)
        {
            if (!(sender is DatePicker datePicker)) return;
            
            Date = datePicker.Date;
            DateSelected?.Invoke(this, e);
        }

        private void SetCornerPaddingLayout()
        {
            if (EntryCornerRadius >= 1f)
            {
                var thick = Convert.ToDouble(EntryCornerRadius);
                _frameEntry.Padding = new Thickness(thick, 0, thick, 0);
                if (_infoLayout != null) _infoLayout.Padding = new Thickness(thick, 0, thick, 0);
                if (_label != null) _label.Margin = new Thickness(thick / 2, 0, thick / 2, 0);
            }
            else
            {
                _frameEntry.Padding = 0;
                if (_infoLayout != null) _infoLayout.Padding = 0;
                if (_label != null) _label.Margin = 0;
            }
        }

        public bool Validate()
        {
            Info = false;
            Validators?.Invoke(this, new EventArgs());
            return !Info;
        }
    }
}