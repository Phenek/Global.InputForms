using System;
using Xamarin.Forms;

namespace Global.InputForms
{
    public abstract class EntryLayout : Grid
    {
        #region ● Label Region
        /// <summary>
        ///     The Entry Text property.
        /// </summary>
        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(nameof(EntryText), typeof(string), typeof(EntryLayout), string.Empty,
                propertyChanged: EntryTextChanged);

        /// <summary>
        ///     The Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty LabelFontAttributesProperty =
            BindableProperty.Create(nameof(LabelFontAttributes), typeof(FontAttributes), typeof(EntryLayout),
                FontAttributes.Bold);

        /// <summary>
        ///     The Label Font Family property.
        /// </summary>
        public static readonly BindableProperty LabelFontFamilyProperty =
            BindableProperty.Create(nameof(LabelFontFamily), typeof(string), typeof(EntryLayout), string.Empty);

        /// <summary>
        ///     The Label Font Size property.
        /// </summary>
        public static readonly BindableProperty LabelFontSizeProperty =
            BindableProperty.Create(nameof(LabelFontSize), typeof(double), typeof(EntryLayout),
                Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        /// <summary>
        ///     The Label Horizontal TextAlignment property.
        /// </summary>
        public static readonly BindableProperty LabelHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelHorizontalTextAlignment), typeof(TextAlignment), typeof(EntryLayout),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty LabelVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelVerticalTextAlignment), typeof(TextAlignment), typeof(EntryLayout),
                TextAlignment.Center);

        /// <summary>
        ///     The Label Text property.
        /// </summary>
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(EntryLayout), string.Empty,
                propertyChanged: LabelTextChanged);

        /// <summary>
        ///     The Label Highlighted Color property.
        /// </summary>
        public static readonly BindableProperty LabelHighlightedColorProperty =
            BindableProperty.Create(nameof(LabelHighlightedColor), typeof(Color), typeof(EntryLayout), Color.Gray,
                propertyChanged: ColorChanged);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty LabelTextColorProperty =
            BindableProperty.Create(nameof(LabelTextColor), typeof(Color), typeof(EntryLayout), Color.Black,
                propertyChanged: ColorChanged);

        /// <summary>
        ///     The Floating Label property.
        /// </summary>
        public static readonly BindableProperty FloatingLabelProperty =
            BindableProperty.Create(nameof(FloatingLabel), typeof(bool), typeof(EntryLayout), true,
                propertyChanged: FloatingLabelChanged);

        /// <summary>
        ///     Gets or sets the label font attributes.
        /// </summary>
        /// <value>The label font attributes.</value>
        public FontAttributes LabelFontAttributes
        {
            get => (FontAttributes)GetValue(LabelFontAttributesProperty);
            set => SetValue(LabelFontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font family.
        /// </summary>
        /// <value>The label font family.</value>
        public string LabelFontFamily
        {
            get => (string)GetValue(LabelFontFamilyProperty);
            set => SetValue(LabelFontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font size.
        /// </summary>
        /// <value>The label font size.</value>
        public double LabelFontSize
        {
            get => (double)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label horizontal text alignment.
        /// </summary>
        /// <value>The label horizontal text alignment.</value>
        public TextAlignment LabelHorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(LabelHorizontalTextAlignmentProperty);
            set => SetValue(LabelHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label vertical text alignment.
        /// </summary>
        /// <value>The label vertical text alignment.</value>
        public TextAlignment LabelVerticalTextAlignment
        {
            get => (TextAlignment)GetValue(LabelVerticalTextAlignmentProperty);
            set => SetValue(LabelVerticalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text.
        /// </summary>
        /// <value>The label text.</value>
        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color LabelHighlightedColor
        {
            get => (Color)GetValue(LabelHighlightedColorProperty);
            set => SetValue(LabelHighlightedColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color LabelTextColor
        {
            get => (Color)GetValue(LabelTextColorProperty);
            set => SetValue(LabelTextColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Floating Label property.
        /// </summary>
        /// <value>The Floating Label.</value>
        public bool FloatingLabel
        {
            get => (bool)GetValue(FloatingLabelProperty);
            set => SetValue(FloatingLabelProperty, value);
        }


        /// <summary>
        ///     The Label Text property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryLayout entryLayout) || entryLayout._label != null) return;

            entryLayout._label = new Label
            {
                TextColor = entryLayout.LabelTextColor,
                InputTransparent = true,
            };

            entryLayout._label.SetBinding(Label.FontAttributesProperty, new Binding(nameof(LabelFontAttributes))
            { Source = entryLayout, Mode = BindingMode.OneWay });
            entryLayout._label.SetBinding(Label.FontFamilyProperty, new Binding(nameof(LabelFontFamily))
            { Source = entryLayout, Mode = BindingMode.OneWay });
            entryLayout._label.SetBinding(Label.HorizontalTextAlignmentProperty, new Binding(nameof(LabelHorizontalTextAlignment))
            { Source = entryLayout, Mode = BindingMode.OneWay });
            entryLayout._label.SetBinding(Label.VerticalTextAlignmentProperty, new Binding(nameof(LabelVerticalTextAlignment))
            { Source = entryLayout, Mode = BindingMode.OneWay });
            entryLayout._label.SetBinding(Label.TextProperty, new Binding(nameof(LabelText))
            { Source = entryLayout, Mode = BindingMode.OneWay });

            FloatingLabelChanged(bindable, entryLayout.FloatingLabel, entryLayout.FloatingLabel);
        }

        private static void FloatingLabelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryLayout entryLayout && entryLayout._label != null)
            {
                if (entryLayout.Children.Contains(entryLayout._label))
                    entryLayout.Children.Remove(entryLayout._label);
                if (!entryLayout.FloatingLabel)
                {
                    entryLayout._label.SetBinding(Label.FontSizeProperty, new Binding(nameof(LabelFontSize))
                    { Source = entryLayout, Mode = BindingMode.OneWay });

                    entryLayout.SetCornerPaddingLayout();
                    entryLayout.Children.Add(entryLayout._label, 1, 3, 0, 1);
                }
                else
                {
                    entryLayout._dumboLabel.Height = new GridLength(entryLayout.LabelFontSize + 10, GridUnitType.Absolute);
                    entryLayout._label.SetBinding(Label.FontSizeProperty, new Binding(nameof(EntryFontSize))
                    { Source = entryLayout, Mode = BindingMode.OneWay });

                    entryLayout.Unfocused += entryLayout.FloatingLabelUnFocused;
                    entryLayout.Focused += entryLayout.FloatingLabelFocused;

                    entryLayout.FloatingLabelWithoutAnimation();

                    entryLayout.Children.Add(entryLayout._label, 2, 3, 1, 2);
                }
            }
        }

        private void FloatingLabelUnFocused(object sender, FocusEventArgs e)
        {
            if (_label == null || !string.IsNullOrEmpty(EntryText)) return;

            var translateY = 0.0;
            var translateX = 0.0;

            if (EntryLayoutType == EntryLayoutType.Besieged)
            {
                translateY = - _dumboLabel.Height.Value / 2;
            }

            var smoothAnimation = new Animation
                {
                {0, 1, new Animation(f => _label.TranslationY = f, _label.TranslationY, translateY, Easing.Linear)},
                {0, 1, new Animation(f => _label.TranslationX = f, _label.TranslationX, translateX, Easing.Linear)},
                {0, 1, new Animation(f => _label.FontSize = f, _label.FontSize, EntryFontSize, Easing.Linear)}
                };

            if (EntryLayoutType == EntryLayoutType.Besieged)
                smoothAnimation.Add(0, 1, new Animation(f => Input.TranslationY = f, Input.TranslationY, translateY, Easing.Linear));

            Device.BeginInvokeOnMainThread(() => smoothAnimation.Commit(this, "EntryAnimation", 16, 300, Easing.Linear, null));
        }

        private void FloatingLabelFocused(object sender, FocusEventArgs e)
        {
            if (_label == null || !string.IsNullOrEmpty(EntryText)) return;

            var translateY = -(((_dumboView.Height - EntryFontSize) / 2) + EntryFontSize);
            var translateX = -_dumboView.Width;

            var smoothAnimation = new Animation
                {
                {0, 1, new Animation(f => _label.TranslationY = f, _label.TranslationY, translateY, Easing.Linear)},
                {0, 1, new Animation(f => _label.TranslationX = f, _label.TranslationX, translateX, Easing.Linear)},
                {0, 1, new Animation(f => _label.FontSize = f, _label.FontSize, LabelFontSize, Easing.Linear)}
                };

            if (EntryLayoutType == EntryLayoutType.Besieged)
                smoothAnimation.Add(0, 1, new Animation(f => Input.TranslationY = f, Input.TranslationY, 0, Easing.Linear));

            Device.BeginInvokeOnMainThread(() => smoothAnimation.Commit(this, "EntryAnimation", 16, 300, Easing.Linear, null));
        }

        private void FloatingLabelWithoutAnimation()
        {
            if (!FloatingLabel || _label == null || IsFocused) return;

            if (!string.IsNullOrEmpty(EntryText))
            {
                var translateY = -(((_dumboView.Height - EntryFontSize) / 2) + EntryFontSize);
                var translateX = -_dumboView.Width;

                _label.TranslationY = translateY;
                _label.TranslationX = translateX;
                _label.FontSize = LabelFontSize;
            }
            else
            {
                var translateY = -_dumboLabel.Height.Value / 2;
                Input.TranslationY = _label.TranslationY = translateY;
            }
        }

        #endregion

        #region ● Info Region
        /// <summary>
        ///     The Info Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty InfoFontAttributesProperty =
            BindableProperty.Create(nameof(InfoFontAttributes), typeof(FontAttributes), typeof(EntryLayout),
                FontAttributes.Bold);

        /// <summary>
        ///     The Info Label Font Family property.
        /// </summary>
        public static readonly BindableProperty InfoFontFamilyProperty =
            BindableProperty.Create(nameof(InfoFontFamily), typeof(string), typeof(EntryLayout), string.Empty);

        /// <summary>
        ///     The Info Label Font Size property.
        /// </summary>
        public static readonly BindableProperty InfoFontSizeProperty =
            BindableProperty.Create(nameof(InfoFontSize), typeof(double), typeof(EntryLayout),
                Device.GetNamedSize(NamedSize.Micro, typeof(Label)));

        /// <summary>
        ///     The Info Label Horizontal TextAlignment property.
        /// </summary>
        public static readonly BindableProperty InfoHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoHorizontalTextAlignment), typeof(TextAlignment), typeof(EntryLayout),
                TextAlignment.Start);

        /// <summary>
        ///     The Info Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty InfoVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoVerticalTextAlignment), typeof(TextAlignment), typeof(EntryLayout),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty InfoColorProperty =
            BindableProperty.Create(nameof(InfoColor), typeof(Color), typeof(EntryLayout), Color.Red);

        /// <summary>
        ///     The Info Label Text property.
        /// </summary>
        public static readonly BindableProperty InfoTextProperty =
            BindableProperty.Create(nameof(InfoText), typeof(string), typeof(EntryLayout), string.Empty,
                propertyChanged: InfoTextChanged);

        /// <summary>
        ///     The Info is visible property.
        /// </summary>
        public static readonly BindableProperty InfoIsVisibleProperty =
            BindableProperty.Create(nameof(InfoIsVisible), typeof(bool), typeof(EntryLayout), false,
                propertyChanged: InfoIsVisibleChanged);

        /// <summary>
        ///     The Info property.
        /// </summary>
        public static readonly BindableProperty InfoProperty =
            BindableProperty.Create(nameof(Info), typeof(bool), typeof(EntryLayout), false);

        /// <summary>
        ///     The Info View Type property.
        /// </summary>
        public static readonly BindableProperty InfoViewTypeProperty =
            BindableProperty.Create(nameof(InfoViewType), typeof(InfoViewType), typeof(EntryLayout), InfoViewType.Line,
                propertyChanged: InfoViewTypeChanged);


        /// <summary>
        ///     Gets or sets the info label font attributes.
        /// </summary>
        /// <value>The info label font attributes.</value>
        public FontAttributes InfoFontAttributes
        {
            get => (FontAttributes)GetValue(InfoFontAttributesProperty);
            set => SetValue(InfoFontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label font family.
        /// </summary>
        /// <value>The info label font family.</value>
        public string InfoFontFamily
        {
            get => (string)GetValue(InfoFontFamilyProperty);
            set => SetValue(InfoFontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label font size.
        /// </summary>
        /// <value>The info label font size.</value>
        public double InfoFontSize
        {
            get => (double)GetValue(InfoFontSizeProperty);
            set => SetValue(InfoFontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label horizontal text alignment.
        /// </summary>
        /// <value>The info label horizontal text alignment.</value>
        public TextAlignment InfoHorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(InfoHorizontalTextAlignmentProperty);
            set => SetValue(InfoHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label vertical text alignment.
        /// </summary>
        /// <value>The info label vertical text alignment.</value>
        public TextAlignment InfoVerticalTextAlignment
        {
            get => (TextAlignment)GetValue(InfoVerticalTextAlignmentProperty);
            set => SetValue(InfoVerticalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info label text.
        /// </summary>
        /// <value>The info label text.</value>
        public string InfoText
        {
            get => (string)GetValue(InfoTextProperty);
            set => SetValue(InfoTextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info color.
        /// </summary>
        /// <value>The info color.</value>
        public Color InfoColor
        {
            get => (Color)GetValue(InfoColorProperty);
            set => SetValue(InfoColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info visibility.
        /// </summary>
        /// <value>The info visibility.</value>
        public bool InfoIsVisible
        {
            get => (bool)GetValue(InfoIsVisibleProperty);
            set => SetValue(InfoIsVisibleProperty, value);
        }

        /// <summary>
        ///     Gets or sets the boolean info .
        /// </summary>
        /// <value>The boolean info.</value>
        public bool Info
        {
            get => (bool)GetValue(InfoProperty);
            set => SetValue(InfoProperty, value);
        }

        /// <summary>
        ///     Gets or sets the info view type  value.
        /// </summary>
        /// <value>the Keyboard value.</value>
        public InfoViewType InfoViewType
        {
            get => (InfoViewType)GetValue(InfoViewTypeProperty);
            set => SetValue(InfoViewTypeProperty, value);
        }

        /// <summary>
        ///     The Info Label Text property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryLayout entryLayout)) return;

            if (entryLayout._infoLabel is Label label)
            {
                label.Text = (string)newValue;
            }
            else
            {
                entryLayout._infoLabel = new Label();
                entryLayout._infoLabel.SetBinding(Label.FontAttributesProperty,
                    new Binding(nameof(InfoFontAttributes))
                    { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout._infoLabel.SetBinding(Label.FontFamilyProperty, new Binding(nameof(InfoFontFamily))
                { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout._infoLabel.SetBinding(Label.FontSizeProperty, new Binding(nameof(InfoFontSize))
                { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout._infoLabel.SetBinding(Label.HorizontalTextAlignmentProperty,
                    new Binding(nameof(InfoHorizontalTextAlignment))
                    { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout._infoLabel.SetBinding(Label.VerticalTextAlignmentProperty,
                    new Binding(nameof(InfoVerticalTextAlignment))
                    { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout._infoLabel.SetBinding(Label.TextColorProperty, new Binding(nameof(InfoColor))
                { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout._infoLabel.SetBinding(Label.TextProperty, new Binding(nameof(InfoText))
                { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout._infoLabel.SetBinding(IsVisibleProperty, new Binding(nameof(InfoIsVisible))
                { Source = entryLayout, Mode = BindingMode.OneWay });

                entryLayout.SetCornerPaddingLayout();
                entryLayout.Children.Add(entryLayout._infoLabel, 1, 4, 2, 3);
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
            if (!(bindable is EntryLayout entryLayout)) return;

            switch (entryLayout.InfoViewType)
            {
                case InfoViewType.Surrounded:
                    entryLayout._frameEntry.BorderColor =
                        (bool)newValue ? entryLayout.InfoColor : entryLayout.EntryBorderColor;
                    break;
                case InfoViewType.Background:
                    entryLayout._frameEntry.BackgroundColor =
                        (bool)newValue ? entryLayout.InfoColor : entryLayout.EntryBackgroundColor;
                    break;
                case InfoViewType.Line:
                    entryLayout._line.BackgroundColor =
                        (bool)newValue ? entryLayout.InfoColor :
                        entryLayout.IsFocused ? entryLayout.LineHighlightedColor : entryLayout.LineColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     The Info view type property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoViewTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryLayout entryLayout)
            {
                ColorChanged(bindable, oldValue, newValue);
                if (entryLayout.Info)
                    InfoIsVisibleChanged(bindable, false, true);
            }
        }

        #endregion

        #region ● Entry & Layout
        /// <summary>
        ///     The Entry Corner Radius property.
        /// </summary>
        public static readonly BindableProperty EntryCornerRadiusProperty =
            BindableProperty.Create(nameof(EntryCornerRadius), typeof(float), typeof(EntryLayout), 0f,
                propertyChanged: EntryCornerRadiusChanged);

        /// <summary>
        ///     The Entry Horizontal Text Alignment property.
        /// </summary>
        public static readonly BindableProperty EntryHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(EntryHorizontalTextAlignment), typeof(TextAlignment), typeof(DatePickerView),
                TextAlignment.Start, propertyChanged: TextAlignmentChanged);

        /// <summary>
        ///     The Entry Height property.
        /// </summary>
        public static readonly BindableProperty EntryHeightRequestProperty =
            BindableProperty.Create(nameof(EntryHeightRequest), typeof(double), typeof(EntryLayout), 30.0);

        /// <summary>
        ///     The Entry Border Color property.
        /// </summary>
        public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(nameof(EntryBorderColor), typeof(Color), typeof(EntryLayout), Color.Transparent,
                propertyChanged: ColorChanged);

        /// <summary>
        ///     The Entry Background Color property.
        /// </summary>
        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(EntryLayout), Color.Transparent,
                propertyChanged: ColorChanged);

        /// <summary>
        ///     The Entry Font Attributes property.
        /// </summary>
        public static readonly BindableProperty EntryFontAttributesProperty =
            BindableProperty.Create(nameof(EntryFontAttributes), typeof(FontAttributes), typeof(EntryLayout),
                FontAttributes.None);

        /// <summary>
        ///     The Entry Font Family property.
        /// </summary>
        public static readonly BindableProperty EntryFontFamilyProperty =
            BindableProperty.Create(nameof(EntryFontFamily), typeof(string), typeof(EntryLayout), string.Empty);

        /// <summary>
        ///     The Entry Font Size property.
        /// </summary>
        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(nameof(EntryFontSize), typeof(double), typeof(EntryLayout),
                Device.GetNamedSize(NamedSize.Medium, typeof(Entry)));

        /// <summary>
        ///     The Entry Placeholder property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderProperty =
            BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(EntryLayout), string.Empty);

        /// <summary>
        ///     The Entry Placeholder Color property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderColorProperty =
            BindableProperty.Create(nameof(EntryPlaceholderColor), typeof(Color), typeof(EntryLayout), Color.Black);

        /// <summary>
        ///     The Entry Text Color property.
        /// </summary>
        public static readonly BindableProperty EntryTextColorProperty =
            BindableProperty.Create(nameof(EntryTextColor), typeof(Color), typeof(EntryLayout), Color.Black);

        /// <summary>
        ///     The Is Read Only property.
        /// </summary>
        public static readonly BindableProperty IsReadOnlyProperty =
            BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(EntryLayout), false);

        /// <summary>
        ///     The Entry Line Color property.
        /// </summary>
        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(EntryLayout), Color.Transparent,
                propertyChanged: ColorChanged);

        /// <summary>
        ///     The Entry Line Color property.
        /// </summary>
        public static readonly BindableProperty LineHighlightedColorProperty =
            BindableProperty.Create(nameof(LineHighlightedColor), typeof(Color), typeof(EntryLayout),
                Color.Transparent);

        /// <summary>
        ///     Border Relative To Corner Radius Property.
        /// </summary>
        public static readonly BindableProperty BorderRelativeProperty =
            BindableProperty.Create(nameof(BorderRelative), typeof(bool), typeof(EntryLayout), true,
                propertyChanged: BorderRelativeChanged);

        /// <summary>
        ///     The Info View Type property.
        /// </summary>
        public static readonly BindableProperty EntryLayoutTypeProperty =
            BindableProperty.Create(nameof(EntryLayoutType), typeof(EntryLayoutType), typeof(EntryLayout), EntryLayoutType.Surrounded,
                propertyChanged: EntryLayoutTypeChanged);

        private readonly Frame _frameEntry;
        private readonly BoxView _line;
        private Label _infoLabel;
        private Label _label;
        private View _dumboView;
        private RowDefinition _dumboLabel;
        protected Command TextAlignmentCommand;
        protected View Input;

        public EventHandler<EventArgs> Validators;
        public event EventHandler<TextChangedEventArgs> TextChanged;

        public EntryLayout()
        {
            ColumnSpacing = 0;
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
            };
            RowSpacing = 0;
            _dumboLabel = new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) };
            RowDefinitions = new RowDefinitionCollection
            {
                _dumboLabel,
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
            };

            _frameEntry = new Frame
            {
                Padding = 0,
                HasShadow = false
            };
            _frameEntry.SetBinding(Frame.CornerRadiusProperty,
                new Binding(nameof(EntryCornerRadius)) {Source = this, Mode = BindingMode.OneWay});
            _frameEntry.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});

            _line = new BoxView
            {
                HeightRequest = 2,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = LineColor
            };

            _dumboView = new BoxView()
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                MinimumHeightRequest = 0,
                MinimumWidthRequest = 0,
                HeightRequest = 0,
                WidthRequest = 0
            };
            Children.Add(_dumboView, 1, 2, 1, 2);
            _dumboView.SizeChanged += _indigo_SizeChanged;

            Children.Add(_frameEntry, 1, 4, 1, 2);
            Children.Add(_line, 1, 4, 1, 2);

            EntryLayoutType = EntryLayoutType.Besieged;
        }

        private void _indigo_SizeChanged(object sender, EventArgs e)
        {
            FloatingLabelWithoutAnimation();
        }

        public new bool IsFocused { get; set; }



        /// <summary>
        ///     Gets or sets the entry text.
        /// </summary>
        /// <value>The entry text.</value>
        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry corner radius.
        /// </summary>
        /// <value>The entry Corner Radius.</value>
        public float EntryCornerRadius
        {
            get => (float) GetValue(EntryCornerRadiusProperty);
            set => SetValue(EntryCornerRadiusProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry height.
        /// </summary>
        /// <value>The entry Corner Radius.</value>
        public double EntryHeightRequest
        {
            get => (double) GetValue(EntryHeightRequestProperty);
            set => SetValue(EntryHeightRequestProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry border color.
        /// </summary>
        /// <value>The entry Border color.</value>
        public Color EntryBorderColor
        {
            get => (Color) GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
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
        ///     Gets or sets the entry line color.
        /// </summary>
        /// <value>The entry line color.</value>
        public Color LineColor
        {
            get => (Color) GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry highlighted line color.
        /// </summary>
        /// <value>The entry line color.</value>
        public Color LineHighlightedColor
        {
            get => (Color) GetValue(LineHighlightedColorProperty);
            set => SetValue(LineHighlightedColorProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool) GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public bool BorderRelative
        {
            get => (bool) GetValue(BorderRelativeProperty);
            set => SetValue(BorderRelativeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry layout type  value.
        /// </summary>
        /// <value>the Keyboard value.</value>
        public EntryLayoutType EntryLayoutType
        {
            get => (EntryLayoutType)GetValue(EntryLayoutTypeProperty);
            set => SetValue(EntryLayoutTypeProperty, value);
        }

        public new event EventHandler<FocusEventArgs> Focused;
        public new event EventHandler<FocusEventArgs> Unfocused;

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (child != _label && child != _infoLabel)
                SetRow(child, 1);
        }

        private static void EntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryLayout entryLayout)
            {
                entryLayout.FloatingLabelWithoutAnimation();
            }
        }

        /// <summary>
        ///     The entry corner radius property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryLayout entryLayout) entryLayout.SetCornerPaddingLayout();
        }

        public static void TextAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryLayout entryLayout
                && entryLayout.TextAlignmentCommand != null
                && entryLayout.TextAlignmentCommand.CanExecute(null))
                entryLayout.TextAlignmentCommand.Execute(null);
        }

        /// <summary>
        ///     The Entry Background Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryLayout entryLayout)
            {
                if (entryLayout._label is Label)
                    entryLayout._label.TextColor = entryLayout.IsFocused
                        ? entryLayout.LabelHighlightedColor
                        : entryLayout.LabelTextColor;

                entryLayout._frameEntry.BackgroundColor = entryLayout.EntryBackgroundColor;
                entryLayout._frameEntry.BorderColor = entryLayout.EntryBorderColor;
                entryLayout._line.BackgroundColor =
                    entryLayout.IsFocused ? entryLayout.LineHighlightedColor : entryLayout.LineColor;
            }
        }

        private static void BorderRelativeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EntryLayout entryLayout) entryLayout.SetCornerPaddingLayout();
        }

        private static void EntryLayoutTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryLayout entryLayout)) return;

            switch (entryLayout.EntryLayoutType)
            {
                case EntryLayoutType.Besieged:
                    
                    SetRow(entryLayout._frameEntry, 0);
                    SetColumn(entryLayout._frameEntry, 0);
                    SetRowSpan(entryLayout._frameEntry, 2);
                    SetColumnSpan(entryLayout._frameEntry, 5);
                    break;
                case EntryLayoutType.Surrounded:
                    break;
                case EntryLayoutType.Line:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     The show info method.
        /// </summary>
        /// <param name="info">The info boolean.</param>
        /// <param name="info">The information string.</param>
        public void ShowInfo(bool isVisible, string info)
        {
            InfoText = info;
            InfoIsVisible = isVisible;
            Info = isVisible;
        }

        public new abstract void Focus();
        public new abstract void Unfocus();

        protected void FocusEntry(object sender, FocusEventArgs e)
        {
            IsFocused = true;
            Focused?.Invoke(this, new FocusEventArgs(this, true));
            if (_label != null) _label.TextColor = LabelHighlightedColor;
            _line.BackgroundColor = InfoViewType == InfoViewType.Line && Info ? InfoColor : LineHighlightedColor;
        }

        protected void UnfocusEntry(object sender, FocusEventArgs e)
        {
            IsFocused = false;
            Unfocused?.Invoke(this, new FocusEventArgs(this, false));
            if (_label != null) _label.TextColor = LabelTextColor;
            _line.BackgroundColor = InfoViewType == InfoViewType.Line && Info ? InfoColor : LineColor;
        }

        protected void SendEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, new TextChangedEventArgs(e.OldTextValue, e.NewTextValue));
        }

        public bool Validate()
        {
            Info = false;
            Validators?.Invoke(this, new EventArgs());
            return !Info;
        }

        protected virtual void SetCornerPaddingLayout()
        {
            if (EntryCornerRadius >= 1f)
            {
                var thick = 0.0;
                if (BorderRelative) thick = Convert.ToDouble(EntryCornerRadius);
                if (_line != null) _line.Margin = new Thickness(thick, 0, thick, 0);
                if (_infoLabel != null) _infoLabel.Margin = new Thickness(thick, 0, thick, 0);
                if (_label != null) _label.Margin = new Thickness(thick / 2, 0, thick / 2, 0);
                if (Input != null) Input.Margin = new Thickness(thick, 0, thick, 0);
            }
            else
            {
                if (_line != null) _line.Margin = 0;
                if (_infoLabel != null) _infoLabel.Margin = 0;
                if (_label != null) _label.Margin = 0;
                if (Input != null) Input.Margin = 0;
            }
        }

        #endregion
    }
}
