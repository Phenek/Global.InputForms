using System;
using System.Collections.Generic;
using Global.InputForms.Interfaces;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class CheckContent : StackLayout, ICheckable
    {
        public static readonly BindableProperty CheckedProperty = BindableProperty.Create(nameof(Checked), typeof(bool),
            typeof(CheckContent), false, propertyChanged: OnCheckedPropertyChanged);

        public static readonly BindableProperty ItemProperty = BindableProperty.Create(nameof(Item),
            typeof(KeyValuePair<string, object>), typeof(CheckButton), new KeyValuePair<string, object>(),
            propertyChanged: OnItemPropertyChanged);

        /// <summary>
        ///     The Icon position property.
        /// </summary>
        public static readonly BindableProperty IconPositionProperty = BindableProperty.Create(nameof(IconPosition),
            typeof(IconPosition), typeof(CheckContent), IconPosition.Start, propertyChanged: IconPositionChanged);

        /// <summary>
        ///     The Icon Size property.
        /// </summary>
        public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize),
            typeof(double), typeof(CheckContent), 20.0);

        public CheckContent()
        {
            Orientation = StackOrientation.Horizontal;

            _icon = new Frame
            {
                Padding = 0,
                HasShadow = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                IsClippedToBounds = false
            };
            UpdateContent(CheckType);

            _icon.SetBinding(HeightRequestProperty,
                new Binding(nameof(IconSize)) {Source = this, Mode = BindingMode.OneWay});
            _icon.SetBinding(WidthRequestProperty,
                new Binding(nameof(IconSize)) {Source = this, Mode = BindingMode.OneWay});
            _icon.SetBinding(MinimumHeightRequestProperty,
                new Binding(nameof(IconSize)) { Source = this, Mode = BindingMode.OneWay });
            _icon.SetBinding(MinimumWidthRequestProperty,
                new Binding(nameof(IconSize)) { Source = this, Mode = BindingMode.OneWay });

            var tap = new TapGestureRecognizer();
            tap.Tapped += OnChecked;
            GestureRecognizers.Add(tap);

            Children.Add(_icon);

            if (Checked)
                SetIconChecked();
            else
                SetIconUncheked();
        }

        public string Key
        {
            get => Item.Key ?? string.Empty;
            set
            {
                if (value == null) return;
                Item = new KeyValuePair<string, object>(value, Item.Value);
            }
        }

        public object Value
        {
            get => Item.Value;
            set
            {
                if (value == null) return;
                Item = new KeyValuePair<string, object>(Item.Key, value);
            }
        }

        /// <summary>
        ///     Gets or sets the icon size.
        /// </summary>
        /// <value>The icon size.</value>
        public double IconSize
        {
            get => (double) GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        /// <summary>
        ///     The Checked property.
        /// </summary>
        BindableProperty ICheckable.CheckedProperty => CheckedProperty;

        /// <summary>
        ///     The Item property.
        /// </summary>
        BindableProperty ICheckable.ItemProperty => ItemProperty;

        public bool DisableCheckOnClick { get; set; }
        public int Index { get; set; }

        event EventHandler<bool> ICheckable.CheckedChanged
        {
            add => CheckedChanged += value;
            remove => CheckedChanged += value;
        }

        event EventHandler<bool> ICheckable.Clicked
        {
            add => Clicked += value;
            remove => Clicked += value;
        }

        public bool Checked
        {
            get => (bool) GetValue(CheckedProperty);
            set
            {
                if (Checked != value) SetValue(CheckedProperty, value);
            }
        }

        public KeyValuePair<string, object> Item
        {
            get => (KeyValuePair<string, object>) GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        string ICheckable.Key
        {
            get => Key;
            set => Key = value;
        }

        object ICheckable.Value
        {
            get => Value;
            set => Value = value;
        }

        public event EventHandler<KeyValuePair<string, object>> ItemChanged;
        public event EventHandler<bool> CheckedChanged;
        public event EventHandler<bool> Clicked;

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (IconPosition == IconPosition.End && Children[Children.Count - 1] != _icon)
            {
                Children.Remove(_icon);
                Children.Add(_icon);
            }
            else if (IconPosition == IconPosition.Start && Children[0] != _icon)
            {
                Children.Remove(_icon);
                Children.Insert(0, _icon);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (height > 0 && _fill is Frame fill && fill.Height > 0)
                fill.CornerRadius = (float) fill.Height * CornerRadius / (float) IconSize;
        }

        private static void OnCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CheckContent checkBox) || checkBox._icon == null) return;

            checkBox.SetCheckedColorsStyles();
            checkBox.CheckedChanged?.Invoke(bindable, (bool) newValue);
        }

        private static void OnItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent CheckBox)
                CheckBox.ItemChanged?.Invoke(CheckBox, CheckBox.Item);
        }

        private void Animation(object sender, bool e)
        {
            if (!(_icon is Frame)) return;

            _icon.Scale = .8;
            _icon.ScaleTo(1, easing: Easing.SpringOut);
        }

        private void OnChecked(object sender, EventArgs e)
        {
            if (!DisableCheckOnClick)
                Checked = !Checked;
            Clicked?.Invoke(this, Checked);
        }

        #region CheckIcon Bindable Property

        /// <summary>
        ///     The Check Type property.
        /// </summary>
        public static readonly BindableProperty CheckTypeProperty =
            BindableProperty.Create(nameof(CheckType), typeof(CheckType), typeof(CheckContent), CheckType.None,
                propertyChanged: CheckTypeChanged);

        /// <summary>
        ///     The Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty CheckLabelFontAttributesProperty =
            BindableProperty.Create(nameof(CheckLabelFontAttributes), typeof(FontAttributes), typeof(CheckContent),
                FontAttributes.Bold, propertyChanged: CheckLabelFontAttributesChanged);

        /// <summary>
        ///     The Check Label Font Family property.
        /// </summary>
        public static readonly BindableProperty CheckLabelFontFamilyProperty =
            BindableProperty.Create(nameof(CheckLabelFontFamily), typeof(string), typeof(CheckContent), string.Empty,
                propertyChanged: CheckLabelFontFamilyChanged);

        /// <summary>
        ///     The Check Label Font Size property.
        /// </summary>
        public static readonly BindableProperty CheckLabelFontSizeProperty =
            BindableProperty.Create(nameof(CheckLabelFontSize), typeof(double), typeof(CheckContent), -1.0,
                propertyChanged: CheckLabelFontSizeChanged);

        /// <summary>
        ///     The Check Label Font Size property.
        /// </summary>
        public static readonly BindableProperty CheckLabelMarginProperty =
            BindableProperty.Create(nameof(CheckLabelMargin), typeof(Thickness), typeof(CheckContent), new Thickness(),
                propertyChanged: CheckLabelMarginChanged);

        /// <summary>
        ///     The Checked Label Text property.
        /// </summary>
        public static readonly BindableProperty CheckedLabelProperty =
            BindableProperty.Create(nameof(CheckedLabel), typeof(string), typeof(CheckContent), string.Empty);

        /// <summary>
        ///     The Unchecked Label Text property.
        /// </summary>
        public static readonly BindableProperty UncheckedLabelProperty =
            BindableProperty.Create(nameof(UncheckedLabel), typeof(string), typeof(CheckContent), string.Empty,
                propertyChanged: UncheckedLabelTextChanged);

        /// <summary>
        ///     The Image Checked property.
        /// </summary>
        public static readonly BindableProperty ImageCheckedProperty = BindableProperty.Create(nameof(ImageChecked),
            typeof(ImageSource), typeof(CheckContent), default(ImageSource), propertyChanged: ImageCheckedChanged);

        /// <summary>
        ///     The Image Unchecked property.
        /// </summary>
        public static readonly BindableProperty ImageUncheckedProperty = BindableProperty.Create(nameof(ImageUnchecked),
            typeof(ImageSource), typeof(CheckContent), default(ImageSource), propertyChanged: ImageUncheckedChanged);

        /// <summary>
        ///     The Image Aspect property.
        /// </summary>
        public static readonly BindableProperty ImageAspectProperty = BindableProperty.Create(nameof(ImageAspect),
            typeof(Aspect), typeof(CheckContent), Aspect.AspectFill, propertyChanged: ImageAspectChanged);

        /// <summary>
        ///     The Image Is Opaque property.
        /// </summary>
        public static readonly BindableProperty ImageIsOpaqueProperty = BindableProperty.Create(nameof(ImageIsOpaque),
            typeof(bool), typeof(CheckContent), false, propertyChanged: ImageIsOpaqueChanged);

        /// <summary>
        ///     The Corner Radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CheckContent), 5f,
                propertyChanged: CornerRadiusChanged);

        /// <summary>
        ///     The UnChecked Color property.
        /// </summary>
        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(CheckContent), Color.Black,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The UnChecked background Color property.
        /// </summary>
        public new static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CheckContent), Color.White,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The UnChecked Border Color property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CheckContent), Color.White,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Checked Color property.
        /// </summary>
        public static readonly BindableProperty CheckedColorPropety =
            BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(CheckContent), Color.Black,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Checked background Color property.
        /// </summary>
        public static readonly BindableProperty CheckedBackgroundColorPropety =
            BindableProperty.Create(nameof(CheckedBackgroundColor), typeof(Color), typeof(CheckContent), Color.WhiteSmoke,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Checked border Color property.
        /// </summary>
        public static readonly BindableProperty CheckedBorderColorPropety =
            BindableProperty.Create(nameof(CheckedBorderColor), typeof(Color), typeof(CheckContent), Color.Black,
                propertyChanged: ColorsChanged);

        #endregion

        #region CheckBox

        private readonly Frame _icon;
        private Label _checkLabel;
        private Frame _fill;
        private Image _image;

        #endregion

        #region CheckIcon Getter/Setter & Changed

        /// <summary>
        ///     Gets or sets the iamge position.
        /// </summary>
        /// <value>The label text color.</value>
        public IconPosition IconPosition
        {
            get => (IconPosition) GetValue(IconPositionProperty);
            set => SetValue(IconPositionProperty, value);
        }

        /// <summary>
        ///     The image position property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void IconPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CheckContent CheckBox) || CheckBox._icon == null) return;

            CheckBox.Children.Remove(CheckBox._icon);

            if (CheckBox.IconPosition == IconPosition.End)
                CheckBox.Children.Add(CheckBox._icon);
            else
                CheckBox.Children.Insert(0, CheckBox._icon);
        }

        /// <summary>
        ///     Gets or sets the label font attributes.
        /// </summary>
        /// <value>The label font attributes.</value>
        public FontAttributes CheckLabelFontAttributes
        {
            get => (FontAttributes) GetValue(CheckLabelFontAttributesProperty);
            set => SetValue(CheckLabelFontAttributesProperty, value);
        }

        /// <summary>
        ///     The Label Font Attributes property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void CheckLabelFontAttributesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._checkLabel != null)
                checkBox._checkLabel.FontAttributes = (FontAttributes) newValue;
        }

        /// <summary>
        ///     Gets or sets the label font family.
        /// </summary>
        /// <value>The label font family.</value>
        public string CheckLabelFontFamily
        {
            get => (string) GetValue(CheckLabelFontFamilyProperty);
            set => SetValue(CheckLabelFontFamilyProperty, value);
        }

        /// <summary>
        ///     The Label Font Family property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void CheckLabelFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._checkLabel != null)
                checkBox._checkLabel.FontFamily = (string) newValue;
        }

        /// <summary>
        ///     Gets or sets the label font size.
        /// </summary>
        /// <value>The label font size.</value>
        public double CheckLabelFontSize
        {
            get => (double) GetValue(CheckLabelFontSizeProperty);
            set => SetValue(CheckLabelFontSizeProperty, value);
        }

        /// <summary>
        ///     The Label Font Size property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void CheckLabelFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._checkLabel != null)
                checkBox._checkLabel.FontSize = (double) newValue;
        }

        /// <summary>
        ///     Gets or sets the label Margin Property.
        /// </summary>
        /// <value>The label Margin.</value>
        public Thickness CheckLabelMargin
        {
            get => (Thickness) GetValue(CheckLabelMarginProperty);
            set => SetValue(CheckLabelMarginProperty, value);
        }

        /// <summary>
        ///     The Label margin property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void CheckLabelMarginChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._checkLabel != null)
                checkBox._checkLabel.Margin = (Thickness) newValue;
        }

        /// <summary>
        ///     Gets or sets the label text.
        /// </summary>
        /// <value>The label text.</value>
        public string UncheckedLabel
        {
            get => (string) GetValue(UncheckedLabelProperty);
            set => SetValue(UncheckedLabelProperty, value);
        }

        private static void UncheckedLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkIcon && checkIcon.CheckType == CheckType.Custom)
                checkIcon._checkLabel.Text = (string) newValue;
        }

        /// <summary>
        ///     Gets or sets the label text.
        /// </summary>
        /// <value>The label text.</value>
        public string CheckedLabel
        {
            get => (string) GetValue(CheckedLabelProperty);
            set => SetValue(CheckedLabelProperty, value);
        }

        /// <summary>
        ///     Gets or sets the checked image.
        /// </summary>
        /// <value>The Checked Image.</value>
        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource ImageChecked
        {
            get => (ImageSource) GetValue(ImageCheckedProperty);
            set => SetValue(ImageCheckedProperty, value);
        }

        /// <summary>
        ///     The Checked Image property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        [TypeConverter(typeof(ImageSourceConverter))]
        private static void ImageCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._image is Image image)
                image.Source = (ImageSource) newValue;
        }

        /// <summary>
        ///     Gets or sets the unchecked image.
        /// </summary>
        /// <value>The unchecked image.</value>
        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource ImageUnchecked
        {
            get => (ImageSource) GetValue(ImageUncheckedProperty);
            set => SetValue(ImageUncheckedProperty, value);
        }

        /// <summary>
        ///     The Unchecked Image property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ImageUncheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._image is Image image)
                image.Source = (ImageSource) newValue;
        }

        /// <summary>
        ///     Gets or sets the image aspect.
        /// </summary>
        /// <value>The image height.</value>
        public Aspect ImageAspect
        {
            get => (Aspect) GetValue(ImageAspectProperty);
            set => SetValue(ImageAspectProperty, value);
        }

        /// <summary>
        ///     The aspect image property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ImageAspectChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._image is Image image)
                image.Aspect = (Aspect) newValue;
        }


        /// <summary>
        ///     Gets or sets the label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public bool ImageIsOpaque
        {
            get => (bool) GetValue(ImageIsOpaqueProperty);
            set => SetValue(ImageIsOpaqueProperty, value);
        }

        /// <summary>
        ///     The Is Opaque Image property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ImageIsOpaqueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox && checkBox._image is Image image)
                image.IsOpaque = (bool) newValue;
        }

        /// <summary>
        ///     Gets or sets the label text.
        /// </summary>
        /// <value>The label text.</value>
        public float CornerRadius
        {
            get => (float) GetValue(CornerRadiusProperty);
            set
            {
                _icon.CornerRadius = value;
                SetValue(CornerRadiusProperty, value);
            }
        }

        private static void CornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CheckContent checkBox)) return;

            checkBox.CornerRadius = (float) newValue;
            if (checkBox._fill != null && checkBox._fill.Height > 0)
                checkBox._fill.CornerRadius =
                    (float) checkBox._fill.Height * (float) newValue / (float) checkBox.Height;
        }

        /// <summary>
        ///     Gets or sets the unchecked color.
        /// </summary>
        /// <value>The unchecked color.</value>
        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        ///     The Unchecked Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ColorsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox) checkBox.SetCheckedColorsStyles();
        }

        /// <summary>
        ///     Gets or sets the Background color.
        /// </summary>
        /// <value>The Background color.</value>
        public new Color BackgroundColor
        {
            get => (Color) GetValue(BackgroundColorProperty);
            set
            {
                _icon.BackgroundColor = value;
                SetValue(BackgroundColorProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Border color.
        /// </summary>
        /// <value>The Border color.</value>
        public Color BorderColor
        {
            get => (Color) GetValue(BorderColorProperty);
            set
            {
                _icon.BorderColor = value;
                SetValue(BorderColorProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Checked color.
        /// </summary>
        /// <value>The Checked color.</value>
        public Color CheckedColor
        {
            get => (Color) GetValue(CheckedColorPropety);
            set => SetValue(CheckedColorPropety, value);
        }

        /// <summary>
        ///     Gets or sets the Checked Background color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color CheckedBackgroundColor
        {
            get => (Color) GetValue(CheckedBackgroundColorPropety);
            set => SetValue(CheckedBackgroundColorPropety, value);
        }

        /// <summary>
        ///     Gets or sets the Checked Border color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color CheckedBorderColor
        {
            get => (Color) GetValue(CheckedBorderColorPropety);
            set => SetValue(CheckedBorderColorPropety, value);
        }

        /// <summary>
        ///     Gets or sets the Check Type value.
        /// </summary>
        /// <value>the Keyboard value.</value>
        public CheckType CheckType
        {
            get => (CheckType) GetValue(CheckTypeProperty);
            set => SetValue(CheckTypeProperty, value);
        }

        /// <summary>
        ///     The Check Type property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void CheckTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckContent checkBox) checkBox.UpdateContent((CheckType) newValue);
        }

        public void SetIconChecked()
        {
            if (CheckType != CheckType.Image)
            {
                _icon.BackgroundColor = CheckedBackgroundColor;
                _icon.BorderColor = CheckedBorderColor;
            }

            switch (CheckType)
            {
                case CheckType.None:
                    break;
                case CheckType.Fill:
                    _fill.BackgroundColor = CheckedColor;
                    _fill.IsVisible = true;
                    break;
                case CheckType.Image:
                    _image.Source = ImageChecked;
                    break;
                case CheckType.Custom:
                    _checkLabel.Text = CheckedLabel;
                    _checkLabel.TextColor = CheckedColor;
                    break;
                default:
                    _checkLabel.TextColor = CheckedColor;
                    _checkLabel.IsVisible = true;
                    break;
            }
        }

        public void SetIconUncheked()
        {
            if (CheckType != CheckType.Image)
            {
                _icon.BackgroundColor = BackgroundColor;
                _icon.BorderColor = BorderColor;
            }

            switch (CheckType)
            {
                case CheckType.None:
                    break;
                case CheckType.Fill:
                    _fill.IsVisible = false;
                    break;
                case CheckType.Image:
                    _image.Source = ImageUnchecked;
                    break;
                case CheckType.Custom:
                    _checkLabel.Text = UncheckedLabel;
                    _checkLabel.TextColor = Color;
                    break;
                default:
                    _checkLabel.IsVisible = false;
                    _checkLabel.TextColor = Color;
                    break;
            }
        }

        private void UpdateLabel(CheckType type)
        {
            switch (type)
            {
                case CheckType.Check:
                    _checkLabel.Text = "✓";
                    break;
                case CheckType.Cross:
                    _checkLabel.Text = "✕";
                    break;
                case CheckType.Star:
                    _checkLabel.Text = "★";
                    break;
                case CheckType.Custom:
                    _checkLabel.Text = UncheckedLabel;
                    break;
            }
        }

        private void UpdateContent(CheckType type)
        {
            switch (type)
            {
                case CheckType.None:
                    break;
                case CheckType.Image:
                    if (_image == null)
                        _image = new Image
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Source = ImageUnchecked,
                            Aspect = ImageAspect,
                            IsOpaque = ImageIsOpaque
                        };
                    _icon.BackgroundColor = Color.Transparent;
                    _icon.BorderColor = Color.Transparent;
                    _icon.Content = _image;
                    break;
                case CheckType.Fill:
                    if (_fill == null)
                        _fill = new Frame
                        {
                            Padding = 0,
                            Margin = 4,
                            HasShadow = false
                        };
                    _icon.Content = _fill;
                    _fill.CornerRadius = (float) _fill.Height * CornerRadius / (float) Height;
                    break;
                default:
                    if (_checkLabel == null)
                    {
                        _checkLabel = new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            Margin = CheckLabelMargin,
                            FontAttributes = CheckLabelFontAttributes,
                            FontFamily = CheckLabelFontFamily,
                            FontSize = CheckLabelFontSize < 0 ? Height * .9 : CheckLabelFontSize,
                            TextColor = CheckedColor
                        };
                        UpdateLabel(type);
                    }

                    _icon.Content = _checkLabel;
                    break;
            }
        }

        public void SetCheckedColorsStyles()
        {
            Animation(this, Checked);
            if (Checked)
                SetIconChecked();
            else
                SetIconUncheked();
        }
    }

    #endregion
}