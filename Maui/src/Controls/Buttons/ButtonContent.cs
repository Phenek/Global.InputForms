using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using static Microsoft.Maui.Controls.Button;

namespace Global.InputForms
{
    [ContentProperty(nameof(Content))]
    public class ButtonContent : Border
    {
        public static new readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View),
            typeof(ButtonContent), propertyChanged: ContentChanged);

        /// <summary>
        ///     The command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
            typeof(ICommand), typeof(ButtonContent));

        /// <summary>
        ///     The command parameter property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonContent));

        /// <summary>
        ///     The content layout property.
        /// </summary>
        public static readonly BindableProperty ContentLayoutProperty =
            BindableProperty.Create(nameof(ContentLayout), typeof(ButtonContentLayout), typeof(ButtonContent),
                new ButtonContentLayout(ButtonContentLayout.ImagePosition.Left, 10),
                propertyChanged: ContentLayoutChanged);

        /// <summary>
        ///     The corner radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius),
            typeof(CornerRadius), typeof(ButtonContent), new CornerRadius(), propertyChanged: CornerRadiusChanged);

        /// <summary>
        ///     The Border Width property.
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(ButtonContent), 0d);

        /// <summary>
        ///     The image property.
        /// </summary>
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource),
            typeof(FileImageSource), typeof(ButtonContent), default(FileImageSource));

        /// <summary>
        ///     The Font Attributes property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(ButtonContent),
                FontAttributes.Bold);

        /// <summary>
        ///     The Font Family property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ButtonContent), string.Empty);

        /// <summary>
        ///     The Font Size property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ButtonContent),
                Device.GetNamedSize(NamedSize.Small, typeof(Microsoft.Maui.Controls.Button)));

        /// <summary>
        ///     The Text property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ButtonContent), string.Empty);

        /// <summary>
        ///     The is busy property.
        /// </summary>
        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool),
            typeof(ButtonContent), false, propertyChanged: IsBusyChanged);

        /// <summary>
        ///     The loader color property.
        /// </summary>
        public static readonly BindableProperty LoaderColorProperty = BindableProperty.Create(nameof(LoaderColor),
            typeof(Color), typeof(ButtonContent), Colors.White);

        /// <summary>
        ///     The loader color property.
        /// </summary>
        public static readonly BindableProperty LoaderProperty = BindableProperty.Create(nameof(Loader),
            typeof(View), typeof(ButtonContent), null, propertyChanged : LoaderChanged);

        /// <summary>
        ///     The Background Color Property.
        /// </summary>
        public new static readonly BindableProperty BackProperty =
            BindableProperty.Create(nameof(Back), typeof(Brush), typeof(ButtonContent), Brush.White,
                propertyChanged: BackgroundColorChanged);

        /// <summary>
        ///     The Text Color Property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
            typeof(Color), typeof(ButtonContent), Colors.RoyalBlue, propertyChanged: TextColorChanged);

        /// <summary>
        ///     The Border Color Property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
            typeof(Brush), typeof(ButtonContent), Brush.LightGray, propertyChanged: BorderColorChanged);

        /// <summary>
        ///     The Highlight Background Color Property.
        /// </summary>
        public static readonly BindableProperty HighlightBackgroundColorProperty =
            BindableProperty.Create(nameof(HighlightBackgroundColor), typeof(Brush), typeof(ButtonContent),
                Brush.LightGray);

        /// <summary>
        ///     The Highlight Border Color Property.
        /// </summary>
        public static readonly BindableProperty HighlightBorderColorProperty =
            BindableProperty.Create(nameof(HighlightBorderColor), typeof(Brush), typeof(ButtonContent),
                Brush.LightGray);

        /// <summary>
        ///     The Highlight Text Color Property.
        /// </summary>
        public static readonly BindableProperty HighlightTextColorProperty =
            BindableProperty.Create(nameof(HighlightTextColor), typeof(Color), typeof(ButtonContent), Colors.White);

        private readonly Grid _grid;
        private readonly BlankButton _button;
        private readonly ActivityIndicator _loader;

        public event EventHandler<bool> BusyChanged;

        public ButtonContent()
        {
            this.SetBinding(StrokeThicknessProperty,
                new Binding(nameof(BorderWidth)) { Source = this, Mode = BindingMode.OneWay });

            _grid = new Grid()
            {
                ColumnSpacing = 0,
                ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } },
                RowSpacing = 0,
                RowDefinitions = new RowDefinitionCollection{ new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }}
            };

            _button = new BlankButton
            {
                BackgroundColor = Colors.Transparent,
                TextColor = TextColor,
                BorderColor = Colors.Transparent,
                BorderWidth = 0,
            };

            _button.SetBinding(Microsoft.Maui.Controls.Button.CommandProperty,
                new Binding(nameof(Command)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Microsoft.Maui.Controls.Button.CommandParameterProperty,
                new Binding(nameof(CommandParameter)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Microsoft.Maui.Controls.Button.ImageSourceProperty,
                new Binding(nameof(ImageSource)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Microsoft.Maui.Controls.Button.FontFamilyProperty,
                new Binding(nameof(FontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Microsoft.Maui.Controls.Button.FontAttributesProperty,
                new Binding(nameof(FontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Microsoft.Maui.Controls.Button.FontSizeProperty,
                new Binding(nameof(FontSize)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Microsoft.Maui.Controls.Button.TextProperty,
                new Binding(nameof(Text)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Microsoft.Maui.Controls.Button.FontAttributesProperty,
                new Binding(nameof(FontAttributes)) {Source = this, Mode = BindingMode.OneWay});

            _button.Pressed += Button_Pressed;
            _button.Released += Button_Released;
            _button.Clicked += Button_Clicked;

            _grid.Add(_button); //Todo overload (0, 0)
            base.Content = _grid;

            Loader = _loader = new ActivityIndicator
            {
                Margin = 0,
                IsRunning = true,
                IsVisible = false,
                InputTransparent = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            _loader.SetBinding(ActivityIndicator.ColorProperty,
                new Binding(nameof(LoaderColor)) { Source = this, Mode = BindingMode.OneWay });
        }

        public new View Content
        {
            get => (View) GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Command.
        /// </summary>
        /// <value>The Command.</value>
        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Command Parameter.
        /// </summary>
        /// <value>The Command Parameter.</value>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Content Layout.
        /// </summary>
        /// <value>The Content Layout.</value>
        public ButtonContentLayout ContentLayout
        {
            get => (ButtonContentLayout) GetValue(ContentLayoutProperty);
            set => SetValue(ContentLayoutProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Border Width.
        /// </summary>
        /// <value>The Border Width.</value>
        public double BorderWidth
        {
            get => (double) GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Corner Radius.
        /// </summary>
        /// <value>The Corner Radius.</value>
        //[System.ComponentModel.TypeConverter(typeof(CornerRadius))]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        ///     Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public FileImageSource ImageSource
        {
            get => (FileImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        /// <summary>
        ///     Gets or sets the  font attributes.
        /// </summary>
        /// <value>The  font attributes.</value>
        public FontAttributes FontAttributes
        {
            get => (FontAttributes) GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the  font family.
        /// </summary>
        /// <value>The  font family.</value>
        public string FontFamily
        {
            get => (string) GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the  font size.
        /// </summary>
        /// <value>The font size.</value>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        /// <value>The  text.</value>
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        ///     Gets or sets is busy value.
        /// </summary>
        /// <value>The IsBusy boolean.</value>
        public bool IsBusy
        {
            get => (bool) GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        /// <summary>
        ///     Gets or sets loader color value.
        /// </summary>
        /// <value>The loader color.</value>
        public Color LoaderColor
        {
            get => (Color) GetValue(LoaderColorProperty);
            set => SetValue(LoaderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets ths custom loader View value.
        /// </summary>
        /// <value>The loader color.</value>
        public View Loader
        {
            get => (View)GetValue(LoaderProperty);
            set => SetValue(LoaderProperty, value);
        }

        /// <summary>
        ///     Gets or sets background color value.
        /// </summary>
        /// <value>The background color.</value>
        public new Brush Back
        {
            get => (Brush) GetValue(BackProperty);
            set => SetValue(BackProperty, value);
        }

        /// <summary>
        ///     Gets or sets text color value.
        /// </summary>
        /// <value>The text color.</value>
        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets border color value.
        /// </summary>
        /// <value>The border color.</value>
        public Brush BorderColor
        {
            get => (Brush) GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets highlight background color.
        /// </summary>
        /// <value>The highlight background color.</value>
        public Brush HighlightBackgroundColor
        {
            get => (Brush) GetValue(HighlightBackgroundColorProperty);
            set => SetValue(HighlightBackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets highlight border color.
        /// </summary>
        /// <value>The highlight border color.</value>
        public Brush HighlightBorderColor
        {
            get => (Brush) GetValue(HighlightBorderColorProperty);
            set => SetValue(HighlightBorderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets highlight text color.
        /// </summary>
        /// <value>The highlight text color.</value>
        public Color HighlightTextColor
        {
            get => (Color) GetValue(HighlightTextColorProperty);
            set => SetValue(HighlightTextColorProperty, value);
        }

        //Events
        public event EventHandler Clicked;
        public event EventHandler Pressed;
        public event EventHandler Released;


        /// <summary>
        ///     The Content Layout property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private new static void ContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ButtonContent button)) return;

            if (oldValue is View oldContent && button._grid.Children.Contains(oldContent))
            {
                button._grid.Children.Remove(oldContent);
            }

            if (newValue is View newContent)
            {
                button._grid.Children.Insert(0, newContent); //Todo overload (0,0)
            }
        }

        /// <summary>
        ///     The Content Layout property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ContentLayoutChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ButtonContent button) button._button.ContentLayout = (ButtonContentLayout) newValue;
        }

        /// <summary>
        ///     The Corner Radius property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void CornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ButtonContent button)
            {
                var roundRectangle = new RoundRectangle();
                roundRectangle.CornerRadius = button.CornerRadius;
                button.StrokeShape = roundRectangle;
            }
        }

        /// <summary>
        ///     The Is Loading changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void IsBusyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ButtonContent button)) return;

            if ((bool) newValue)
            {
                button.Loader.IsVisible = true;
                button._button.Text = string.Empty;
                button._button.ImageSource = null;
            }
            else
            {
                button.Loader.IsVisible = false;
                button._button.Text = button.Text;
                button._button.ImageSource = button.ImageSource;
            }
            button.BusyChanged?.Invoke(button, (bool) newValue);
        }

        private static void LoaderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ButtonContent button)) return;

            if (oldValue is View oldLoader && button._grid.Children.Contains(oldLoader))
            {
                button._grid.Children.Remove(oldLoader);
            }

            if (newValue is View newLoader)
            {
                newLoader.IsVisible = button.IsBusy;
                button._grid.Children.Add(newLoader); //Todo overload (0,0)
            }
        }

        /// <summary>
        ///     The Background Color changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void BackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ButtonContent button) button.SetReleasedColors();
        }

        /// <summary>
        ///     The Text Color changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void TextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ButtonContent button) button._button.TextColor = (Color) newValue;
        }

        /// <summary>
        ///     The Border Color changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void BorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ButtonContent button) button.SetReleasedColors();
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            SetPressedColors();
            Pressed?.Invoke(this, new EventArgs());
        }

        private void Button_Released(object sender, EventArgs e)
        {
            SetReleasedColors();
            Released?.Invoke(this, new EventArgs());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, new EventArgs());
        }

        private void SetPressedColors()
        {
            base.Background = HighlightBackgroundColor;
            base.Stroke = HighlightBorderColor;
            _button.TextColor = HighlightTextColor;
        }

        private void SetReleasedColors()
        {
            base.Background = Back;
            base.Stroke = BorderColor;
            _button.TextColor = TextColor;
        }
    }
}