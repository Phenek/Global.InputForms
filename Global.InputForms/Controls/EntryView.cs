using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class EntryView : StackLayout
    {
        /// <summary>
        ///     The Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty LabelFontAttributesProperty =
            BindableProperty.Create(nameof(LabelFontAttributes), typeof(FontAttributes), typeof(EntryView),
                FontAttributes.Bold);

        /// <summary>
        ///     The Label Font Family property.
        /// </summary>
        public static readonly BindableProperty LabelFontFamilyProperty =
            BindableProperty.Create(nameof(LabelFontFamily), typeof(string), typeof(EntryView), string.Empty);

        /// <summary>
        ///     The Label Font Size property.
        /// </summary>
        public static readonly BindableProperty LabelFontSizeProperty =
            BindableProperty.Create(nameof(LabelFontSize), typeof(double), typeof(EntryView),
                Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        /// <summary>
        ///     The Label Horizontal TextAlignment property.
        /// </summary>
        public static readonly BindableProperty LabelHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelHorizontalTextAlignment), typeof(TextAlignment), typeof(EntryView),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty LabelVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelVerticalTextAlignment), typeof(TextAlignment), typeof(EntryView),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Text property.
        /// </summary>
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(EntryView), string.Empty,
                propertyChanged: LabelTextChanged);

        /// <summary>
        ///     The Label Highlighted Color property.
        /// </summary>
        public static readonly BindableProperty LabelHighlightedColorProperty =
            BindableProperty.Create(nameof(LabelHighlightedColor), typeof(Color), typeof(EntryView), Color.Gray,
                propertyChanged: LabelHighlightedColorChanged);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty LabelTextColorProperty =
            BindableProperty.Create(nameof(LabelTextColor), typeof(Color), typeof(EntryView), Color.Black,
                propertyChanged: LabelTextColorChanged);

        /// <summary>
        ///     The Entry Corner Radius property.
        /// </summary>
        public static readonly BindableProperty EntryCornerRadiusProperty =
            BindableProperty.Create(nameof(EntryCornerRadius), typeof(float), typeof(EntryView), 0f,
                propertyChanged: EntryCornerRadiusChanged);

        /// <summary>
        ///     The Entry Border Color property.
        /// </summary>
        public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(nameof(EntryBorderColor), typeof(Color), typeof(EntryView), Color.Transparent,
                propertyChanged: EntryBorderColorChanged);

        /// <summary>
        ///     The Entry Background Color property.
        /// </summary>
        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(EntryView), Color.Transparent,
                propertyChanged: EntryBackgroundColorChanged);

        /// <summary>
        ///     The Entry Font Attributes property.
        /// </summary>
        public static readonly BindableProperty EntryFontAttributesProperty =
            BindableProperty.Create(nameof(EntryFontAttributes), typeof(FontAttributes), typeof(EntryView),
                FontAttributes.None);

        /// <summary>
        ///     The Entry Font Family property.
        /// </summary>
        public static readonly BindableProperty EntryFontFamilyProperty =
            BindableProperty.Create(nameof(EntryFontFamily), typeof(string), typeof(EntryView), string.Empty);

        /// <summary>
        ///     The Entry Font Size property.
        /// </summary>
        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(nameof(EntryFontSize), typeof(double), typeof(EntryView),
                Device.GetNamedSize(NamedSize.Medium, typeof(Entry)));

        /// <summary>
        ///     The Entry Horizontal Text Alignment property.
        /// </summary>
        public static readonly BindableProperty EntryHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(EntryHorizontalTextAlignment), typeof(TextAlignment), typeof(EntryView),
                TextAlignment.Start);

        /// <summary>
        ///     The Entry Placeholder property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderProperty =
            BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(EntryView), string.Empty);

        /// <summary>
        ///     The Entry Placeholder Color property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderColorProperty =
            BindableProperty.Create(nameof(EntryPlaceholderColor), typeof(Color), typeof(EntryView), Color.Black);

        /// <summary>
        ///     The Entry Text Color property.
        /// </summary>
        public static readonly BindableProperty EntryTextColorProperty =
            BindableProperty.Create(nameof(EntryTextColor), typeof(Color), typeof(EntryView), Color.Black);

        /// <summary>
        ///     The Entry Text property.
        /// </summary>
        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(nameof(EntryText), typeof(string), typeof(EntryView), string.Empty,
                propertyChanged: EntryTextChanged);


        /// <summary>
        ///     The Masked Entry Text property.
        /// </summary>
        public static readonly BindableProperty MaskedEntryTextProperty =
            BindableProperty.Create(nameof(MaskedEntryText), typeof(string), typeof(EntryView), string.Empty,
                propertyChanged: MaskEntryTextChanged);

        /// <summary>
        ///     The Entry Mask property.
        /// </summary>
        public static readonly BindableProperty MaskProperty = BindableProperty.Create(nameof(Mask), typeof(string),
            typeof(EntryView), string.Empty, propertyChanged: EntryMaskChanged);


        public static readonly BindableProperty IsSpellCheckEnabledProperty =
            BindableProperty.Create(nameof(IsSpellCheckEnabled), typeof(bool), typeof(Entry), true,
                BindingMode.OneTime);

        public static readonly BindableProperty IsTextPredictionEnabledProperty =
            BindableProperty.Create(nameof(IsTextPredictionEnabled), typeof(bool), typeof(Entry), true,
                BindingMode.OneTime);

        /// <summary>
        ///     The IsEnabled property.
        /// </summary>
        public static readonly BindableProperty EntryIsEnabledProperty =
            BindableProperty.Create(nameof(EntryIsEnabled), typeof(bool), typeof(EntryView), true,
                propertyChanged: EntryIsEnabledChanged);

        /// <summary>
        ///     The IsPassword property.
        /// </summary>
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EntryView), false);

        /// <summary>
        ///     The Keyboard property.
        /// </summary>
        public static BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryView), Keyboard.Default);

        /// <summary>
        ///     The Info View Type property.
        /// </summary>
        public static readonly BindableProperty InfoViewTypeProperty =
            BindableProperty.Create(nameof(InfoViewType), typeof(InfoViewType), typeof(EntryView), InfoViewType.Line,
                propertyChanged: InfoViewTypeChanged);

        /// <summary>
        ///     The Info Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontAttributesProperty =
            BindableProperty.Create(nameof(InfoLabelFontAttributes), typeof(FontAttributes), typeof(EntryView),
                FontAttributes.Bold);

        /// <summary>
        ///     The Info Label Font Family property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontFamilyProperty =
            BindableProperty.Create(nameof(InfoLabelFontFamily), typeof(string), typeof(EntryView), string.Empty);

        /// <summary>
        ///     The Info Label Font Size property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontSizeProperty =
            BindableProperty.Create(nameof(InfoLabelFontSize), typeof(double), typeof(EntryView),
                Device.GetNamedSize(NamedSize.Micro, typeof(Label)));

        /// <summary>
        ///     The Info Label Horizontal TextAlignment property.
        /// </summary>
        public static readonly BindableProperty InfoLabelHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoLabelHorizontalTextAlignment), typeof(TextAlignment), typeof(EntryView),
                TextAlignment.Start);

        /// <summary>
        ///     The Info Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty InfoLabelVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoLabelVerticalTextAlignment), typeof(TextAlignment), typeof(EntryView),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty InfoColorProperty =
            BindableProperty.Create(nameof(InfoColor), typeof(Color), typeof(EntryView), Color.Red);

        /// <summary>
        ///     The Info Label Text property.
        /// </summary>
        public static readonly BindableProperty InfoLabelTextProperty =
            BindableProperty.Create(nameof(InfoLabelText), typeof(string), typeof(EntryView), string.Empty,
                propertyChanged: InfoLabelTextChanged);

        /// <summary>
        ///     The Info is visible property.
        /// </summary>
        public static readonly BindableProperty InfoIsVisibleProperty =
            BindableProperty.Create(nameof(InfoIsVisible), typeof(bool), typeof(EntryView), false,
                propertyChanged: InfoIsVisibleChanged);

        /// <summary>
        ///     The Info property.
        /// </summary>
        public static readonly BindableProperty InfoProperty =
            BindableProperty.Create(nameof(Info), typeof(bool), typeof(EntryView), false);

        /// <summary>
        ///     Border Relative To Corner Radius Property.
        /// </summary>
        public static readonly BindableProperty BorderRelativeProperty =
            BindableProperty.Create(nameof(BorderRelative), typeof(bool), typeof(EntryView), true,
                propertyChanged: BorderRelativeChanged);

        private readonly BlankEntry _entry;
        private readonly Frame _frameEntry;
        private Label _infoLabel;
        private StackLayout _infoLayout;
        private BoxView _infoLine;
        private Label _label;

        private Dictionary<int, char> _positions;
        public EventHandler<TextChangedEventArgs> TextChanged;
        public EventHandler<EventArgs> Validators;

        public EntryView()
        {
            Orientation = StackOrientation.Vertical;
            Spacing = 0;

            _entry = new BlankEntry
            {
                HeightRequest = 40,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _entry.SetBinding(Entry.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.IsPasswordProperty,
                new Binding(nameof(IsPassword)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(InputView.KeyboardProperty,
                new Binding(nameof(Keyboard)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(InputView.IsSpellCheckEnabledProperty,
                new Binding(nameof(IsSpellCheckEnabled)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.IsTextPredictionEnabledProperty,
                new Binding(nameof(IsTextPredictionEnabled)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryText)) {Source = this, Mode = BindingMode.TwoWay});

            _frameEntry = new Frame
            {
                Padding = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                HasShadow = false,
                Content = _entry
            };
            _frameEntry.SetBinding(IsEnabledProperty,
                new Binding(nameof(EntryIsEnabled)) {Source = this, Mode = BindingMode.TwoWay});
            _frameEntry.SetBinding(Frame.CornerRadiusProperty,
                new Binding(nameof(EntryCornerRadius)) {Source = this, Mode = BindingMode.TwoWay});

            _entry.TextChanged += Entry_Changed;
            _entry.Focused += Entry_Focused;
            _entry.Unfocused += Entry_Unfocused;
            _entry.TextChanged += OnEntryTextChanged;
            TextChanged += (sender, e) =>
            {
                if (InfoIsVisible)
                    Validate();
            };

            Unfocused += (sender, e) => Validate();

            Children.Add(_frameEntry);
        }

        public bool EntryIsEnabled
        {
            get => (bool) GetValue(EntryIsEnabledProperty);
            set => SetValue(EntryIsEnabledProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry text.
        /// </summary>
        /// <value>The entry text.</value>
        public string EntryText
        {
            get => (string) GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the masked entry text.
        /// </summary>
        /// <value>The entry text.</value>
        public string MaskedEntryText
        {
            get => (string) GetValue(MaskedEntryTextProperty);
            set => SetValue(MaskedEntryTextProperty, value);
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

        /// <summary>
        ///     Gets or sets the entry mask.
        /// </summary>
        /// <value>The entry mask.</value>
        public string Mask
        {
            get => (string) GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        public bool IsTextPredictionEnabled
        {
            get => (bool) GetValue(IsTextPredictionEnabledProperty);
            set => SetValue(IsTextPredictionEnabledProperty, value);
        }

        public bool IsSpellCheckEnabled
        {
            get => (bool) GetValue(IsSpellCheckEnabledProperty);
            set => SetValue(IsSpellCheckEnabledProperty, value);
        }

        /// <summary>
        ///     Gets or sets the IsPassword value.
        /// </summary>
        /// <value>the IsPassword value.</value>
        public bool IsPassword
        {
            get => (bool) GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Keyboard value.
        /// </summary>
        /// <value>the Keyboard value.</value>
        public Keyboard Keyboard
        {
            get => (Keyboard) GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
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
        ///     Gets or sets the info color.
        /// </summary>
        /// <value>The info color.</value>
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

        public bool BorderRelative
        {
            get => (bool) GetValue(BorderRelativeProperty);
            set => SetValue(BorderRelativeProperty, value);
        }

        public new event EventHandler<FocusEventArgs> Focused;
        public new event EventHandler<FocusEventArgs> Unfocused;


        private void Entry_Changed(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        private static void EntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryView entryView) || string.IsNullOrEmpty(entryView.Mask)) return;
            
            var masked = entryView.AddMask((string) newValue);
            if (entryView._entry.Text == masked) return;
            
            entryView._entry.TextChanged -= entryView.OnEntryTextChanged;
            entryView._entry.Text = masked;
            entryView._entry.TextChanged += entryView.OnEntryTextChanged;
        }

        private static void MaskEntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //To Do
            if (!(bindable is EntryView entryView)) return;
            
            if (!string.IsNullOrEmpty(entryView.Mask))
            {
                /*
                    EntryView._entry.TextChanged -= EntryView.OnEntryTextChanged;
                    EntryView._entry.Text = EntryView.AddMask((string)newValue);
                    EntryView._entry.TextChanged -= EntryView.OnEntryTextChanged;
                    */
            }
        }

        private string AddMask(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            
            var sb = new StringBuilder(str);

            var nbX = 0;
            for (var i = 0; i < Mask.Length; ++i)
                if (Mask[i] != 'X' && nbX < str.Length && Mask[i] != str[nbX])
                    sb.Insert(i, Mask[i]);
                else
                    ++nbX;
            return sb.ToString();

        }

        private string RemoveMask(string maskedString)
        {
            var sb = new StringBuilder(maskedString);

            for (var i = Mask.Length - 1; i >= 0; --i)
                if (Mask[i] != 'X' && i < maskedString.Length)
                    sb.Remove(i, 1);
            return sb.ToString();
        }

        private static void EntryMaskChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryView entryView)) return;
            
            if (string.IsNullOrEmpty(entryView.Mask))
            {
                entryView._entry.SetBinding(Entry.TextProperty,
                    new Binding(nameof(EntryText)) {Source = entryView, Mode = BindingMode.TwoWay});
                entryView._positions = null;
                return;
            }

            entryView._entry.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryMaskChanged)) {Source = entryView, Mode = BindingMode.TwoWay});

            var list = new Dictionary<int, char>();
            for (var i = 0; i < entryView.Mask.Length; i++)
                if (entryView.Mask[i] != 'X')
                    list.Add(i, entryView.Mask[i]);

            entryView._positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!(sender is Entry entry)) return;
            
            if (!string.IsNullOrEmpty(Mask))
            {
                var text = entry.Text;
                if (string.IsNullOrWhiteSpace(text) || _positions == null)
                    return;

                if (text.Length > Mask.Length)
                {
                    _entry.TextChanged -= OnEntryTextChanged;
                    entry.Text = text.Remove(text.Length - 1);
                    _entry.TextChanged += OnEntryTextChanged;
                    return;
                }

                foreach (var position in _positions)
                    if (text.Length >= position.Key + 1)
                    {
                        var value = position.Value.ToString();
                        if (text.Substring(position.Key, 1) != value)
                            text = text.Insert(position.Key, value);
                    }

                if (entry.Text != text)
                {
                    _entry.TextChanged -= OnEntryTextChanged;
                    entry.Text = text;
                    EntryText = RemoveMask(text);

                    entry.TextChanged += OnEntryTextChanged;
                }

                if (EntryText != RemoveMask(text))
                    EntryText = RemoveMask(text);
            }
            else
            {
                MaskedEntryText = EntryText;
            }
        }

        private static void EntryIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //Hack for Android Input Text
            if (bindable is EntryView entryView) entryView._entry.InputTransparent = !(bool) newValue;
        }

        /// <summary>
        ///     The Label Text property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryView entryView) || entryView._label != null) return;
            
            entryView._label = new Label
            {
                TextColor = entryView.LabelTextColor
            };

            entryView._label.SetBinding(Label.FontAttributesProperty, new Binding(nameof(LabelFontAttributes))
                {Source = entryView, Mode = BindingMode.TwoWay});

            entryView._label.SetBinding(Label.FontFamilyProperty, new Binding(nameof(LabelFontFamily))
                {Source = entryView, Mode = BindingMode.TwoWay});

            entryView._label.SetBinding(Label.FontSizeProperty, new Binding(nameof(LabelFontSize))
                {Source = entryView, Mode = BindingMode.TwoWay});

            entryView._label.SetBinding(Label.HorizontalTextAlignmentProperty,
                new Binding(nameof(LabelHorizontalTextAlignment))
                    {Source = entryView, Mode = BindingMode.TwoWay});

            entryView._label.SetBinding(Label.VerticalTextAlignmentProperty,
                new Binding(nameof(LabelVerticalTextAlignment))
                    {Source = entryView, Mode = BindingMode.TwoWay});

            entryView._label.SetBinding(Label.TextProperty, new Binding(nameof(LabelText))
                {Source = entryView, Mode = BindingMode.TwoWay});

            entryView.SetCornerPaddingLayout();
            entryView.Children.Insert(0, entryView._label);
        }

        /// <summary>
        ///     The Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelHighlightedColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryView entryView && entryView._label is Label && entryView.IsFocused)
                entryView._label.TextColor = (Color) newValue;
        }

        /// <summary>
        ///     The Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryView entryView && entryView._label is Label && !entryView.IsFocused)
                entryView._label.TextColor = (Color) newValue;
        }

        /// <summary>
        ///     The entry corner radius property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryView entryView) entryView.SetCornerPaddingLayout();
        }

        /// <summary>
        ///     The entry border color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryView entryView)
                entryView._frameEntry.BorderColor = (Color) newValue;
        }

        /// <summary>
        ///     The Entry Background Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryView entryView)
                entryView._frameEntry.BackgroundColor = (Color) newValue;
        }

        /// <summary>
        ///     The Info Label Font Attributes property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoViewTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryView entryView && entryView._infoLine is BoxView)
                entryView._infoLine.IsVisible = (InfoViewType) newValue == InfoViewType.Line;
        }

        /// <summary>
        ///     The Info Label Text property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryView entryView)) return;
            
            if (entryView._infoLabel is Label label)
            {
                label.Text = (string) newValue;
            }
            else
            {
                entryView._infoLabel = new Label();
                entryView._infoLabel.SetBinding(Label.FontAttributesProperty,
                    new Binding(nameof(InfoLabelFontAttributes))
                        {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLabel.SetBinding(Label.FontFamilyProperty, new Binding(nameof(InfoLabelFontFamily))
                    {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLabel.SetBinding(Label.FontSizeProperty, new Binding(nameof(InfoLabelFontSize))
                    {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLabel.SetBinding(Label.HorizontalTextAlignmentProperty,
                    new Binding(nameof(InfoLabelHorizontalTextAlignment))
                        {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLabel.SetBinding(Label.VerticalTextAlignmentProperty,
                    new Binding(nameof(InfoLabelVerticalTextAlignment))
                        {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLabel.SetBinding(Label.TextColorProperty, new Binding(nameof(InfoColor))
                    {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLabel.SetBinding(Label.TextProperty, new Binding(nameof(InfoLabelText))
                    {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLine = new BoxView
                {
                    HeightRequest = 1,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IsVisible = entryView.InfoViewType == InfoViewType.Line
                };
                entryView._infoLine.SetBinding(BackgroundColorProperty, new Binding(nameof(InfoColor))
                    {Source = entryView, Mode = BindingMode.TwoWay});

                entryView._infoLayout = new StackLayout
                {
                    Spacing = 0,
                    Orientation = StackOrientation.Vertical
                };
                entryView._infoLayout.SetBinding(IsVisibleProperty, new Binding(nameof(Info))
                    {Source = entryView, Mode = BindingMode.TwoWay});

                entryView.SetCornerPaddingLayout();

                entryView._infoLayout.Children.Add(entryView._infoLine);
                entryView._infoLayout.Children.Add(entryView._infoLabel);
                entryView.Children.Add(entryView._infoLayout);
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
            if (!(bindable is EntryView entryView) || !(entryView._infoLayout != null)) return;
            
            entryView._infoLayout.IsVisible = (bool) newValue;
            switch (entryView.InfoViewType)
            {
                case InfoViewType.Surrounded:
                    entryView._frameEntry.BorderColor =
                        (bool) newValue ? entryView.InfoColor : entryView.EntryBorderColor;
                    break;
                case InfoViewType.Background:
                    entryView._frameEntry.BackgroundColor =
                        (bool) newValue ? entryView.InfoColor : entryView.EntryBackgroundColor;
                    break;
                case InfoViewType.Line:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void BorderRelativeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryView entryView) entryView.SetCornerPaddingLayout();
        }

        /// <summary>
        ///     The Show Info Function.
        /// </summary>
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
            _entry.Focus();
        }

        public new void Unfocus()
        {
            _entry.Unfocus();
        }

        public event EventHandler Completed
        {
            add => _entry.Completed += value;
            remove => _entry.Completed -= value;
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (_label != null) _label.TextColor = LabelHighlightedColor;
            Focused?.Invoke(this, new FocusEventArgs(this, true));
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            IsFocused = false;
            if (_label != null) _label.TextColor = LabelTextColor;
            Unfocused?.Invoke(this, new FocusEventArgs(this, true));
        }

        private void SetCornerPaddingLayout()
        {
            if (EntryCornerRadius >= 1f)
            {
                var thick = 0.0;
                if (BorderRelative) thick = Convert.ToDouble(EntryCornerRadius);
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