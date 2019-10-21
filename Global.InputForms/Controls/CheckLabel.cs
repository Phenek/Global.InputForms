using Xamarin.Forms;

namespace Global.InputForms
{
    public class CheckLabel : CheckBox
    {
        /// <summary>
        ///     The Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes),
            typeof(FontAttributes), typeof(CheckLabel), FontAttributes.Bold);

        /// <summary>
        ///     The Label Font Family property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily),
            typeof(string), typeof(CheckLabel), string.Empty);

        /// <summary>
        ///     The Label Font Size property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize),
            typeof(double), typeof(CheckLabel), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

        /// <summary>
        ///     The Label Horizontal Text Alignment property.
        /// </summary>
        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(CheckLabel),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty VerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(CheckLabel),
                TextAlignment.Center);

        /// <summary>
        ///     The Label Text property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string),
            typeof(CheckLabel), string.Empty);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
            typeof(Color), typeof(CheckLabel), Color.Black);

        private readonly Label _label;

        public CheckLabel()
        {
            _label = new Label
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            _label.SetBinding(Label.FontAttributesProperty,
                new Binding(nameof(FontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _label.SetBinding(Label.FontFamilyProperty,
                new Binding(nameof(FontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _label.SetBinding(Label.FontSizeProperty,
                new Binding(nameof(FontSize)) {Source = this, Mode = BindingMode.OneWay});
            _label.SetBinding(Label.HorizontalTextAlignmentProperty,
                new Binding(nameof(HorizontalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _label.SetBinding(Label.VerticalTextAlignmentProperty,
                new Binding(nameof(VerticalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _label.SetBinding(Label.TextColorProperty,
                new Binding(nameof(TextColor)) {Source = this, Mode = BindingMode.OneWay});

            // Is It needed?
            _label.SetBinding(Label.TextProperty,
                new Binding(nameof(Text)) {Source = this, Mode = BindingMode.OneWay});

            ItemChanged += (sender, e) =>
            {
                if (e.Value is string str)
                    _label.Text = str;
            };

            Children.Add(_label);
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
        ///     Gets or sets the label horizontal text alignment.
        /// </summary>
        /// <value>The label horizontal text alignment.</value>
        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment) GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label vertical text alignment.
        /// </summary>
        /// <value>The label vertical text alignment.</value>
        public TextAlignment VerticalTextAlignment
        {
            get => (TextAlignment) GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text.
        /// </summary>
        /// <value>The label text.</value>
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
    }
}