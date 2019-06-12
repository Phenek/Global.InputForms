using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using static Xamarin.Forms.Button;

namespace Global.InputForms
{
    [ContentProperty(nameof(Content))]
    public class ButtonContent : Grid
    {
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View),
            typeof(ButtonContent), null);

        /// <summary>
        ///     The command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
            typeof(ICommand), typeof(ButtonContent), null);

        /// <summary>
        ///     The command parameter property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonContent), null);

        /// <summary>
        ///     The content layout property.
        /// </summary>
        public static readonly BindableProperty ContentLayoutProperty =
            BindableProperty.Create(nameof(ContentLayout), typeof(ButtonContentLayout), typeof(ButtonContent),
                new ButtonContentLayout(ButtonContentLayout.ImagePosition.Left, 10),
                propertyChanged: ContentLayoutChanged);

        /// <summary>
        ///     The padding button property.
        /// </summary>
        public new static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding),
            typeof(Thickness), typeof(ButtonContent), new Thickness(0, 0, 0, 0), propertyChanged: BorderChanged);

        /// <summary>
        ///     The border width property.
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth),
            typeof(double), typeof(ButtonContent), 0d, propertyChanged: BorderChanged);

        /// <summary>
        ///     The corner radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius),
            typeof(int), typeof(ButtonContent), 0, propertyChanged: CornerRadiusChanged);

        /// <summary>
        ///     The image property.
        /// </summary>
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image),
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
                Device.GetNamedSize(NamedSize.Small, typeof(Button)));

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
            typeof(Color), typeof(ButtonContent), Color.White);

        /// <summary>
        ///     The Background Color Property.
        /// </summary>
        public new static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ButtonContent), Color.White,
                propertyChanged: BackgroundColorChanged);

        /// <summary>
        ///     The Text Color Property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
            typeof(Color), typeof(ButtonContent), Color.RoyalBlue, propertyChanged: TextColorChanged);

        /// <summary>
        ///     The Border Color Property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
            typeof(Color), typeof(ButtonContent), Color.LightGray, propertyChanged: BorderColorChanged);

        /// <summary>
        ///     The Highlight Background Color Property.
        /// </summary>
        public static readonly BindableProperty HighlightBackgroundColorProperty =
            BindableProperty.Create(nameof(HighlightBackgroundColor), typeof(Color), typeof(ButtonContent),
                Color.LightGray);

        /// <summary>
        ///     The Highlight Border Color Property.
        /// </summary>
        public static readonly BindableProperty HighlightBorderColorProperty =
            BindableProperty.Create(nameof(HighlightBorderColor), typeof(Color), typeof(ButtonContent),
                Color.LightGray);

        /// <summary>
        ///     The Highlight Text Color Property.
        /// </summary>
        public static readonly BindableProperty HighlightTextColorProperty =
            BindableProperty.Create(nameof(HighlightTextColor), typeof(Color), typeof(ButtonContent), Color.White);

        /// <summary>
        ///     The Highlight Image Color Property.
        /// </summary>
        public static readonly BindableProperty HighlightImageColorProperty =
            BindableProperty.Create(nameof(HighlightImageColor), typeof(Color), typeof(ButtonContent),
                null);
        
        private readonly BlankButton _button;
        private readonly ActivityIndicator _loader;
        private readonly Frame _frame;

        public ButtonContent()
        {
            _frame = new Frame()
            {
                Padding = 0,
                HasShadow = false,
                CornerRadius = CornerRadius,
                BackgroundColor = Color.Transparent,
                InputTransparent = true,
                IsEnabled = false,
                IsClippedToBounds = true,
            };
            _frame.SetBinding(ContentView.ContentProperty,
                new Binding(nameof(Content)) {Source = this, Mode = BindingMode.OneWay});
            _frame.SetBinding(PaddingProperty, new Binding(nameof(Padding)) { Source = this, Mode = BindingMode.OneWay });

            _button = new BlankButton
            {
                HeightRequest = 0,
                BackgroundColor = BackgroundColor,
                TextColor = TextColor,
                BorderColor = BorderColor,
            };

            _button.SetBinding(Button.CommandProperty,
                new Binding(nameof(Command)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.CommandParameterProperty,
                new Binding(nameof(CommandParameter)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.BorderWidthProperty,
                new Binding(nameof(BorderWidth)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.ImageProperty,
                new Binding(nameof(Image)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.FontFamilyProperty,
                new Binding(nameof(FontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.FontAttributesProperty,
                new Binding(nameof(FontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.FontSizeProperty,
                new Binding(nameof(FontSize)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.TextProperty,
                new Binding(nameof(Text)) {Source = this, Mode = BindingMode.OneWay});
            _button.SetBinding(Button.FontAttributesProperty,
                new Binding(nameof(FontAttributes)) {Source = this, Mode = BindingMode.OneWay});

            _loader = new ActivityIndicator
            {
                Margin = 0,
                IsRunning = true,
                IsVisible = false,
                InputTransparent = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            _loader.SetBinding(ActivityIndicator.ColorProperty,
                new Binding(nameof(LoaderColor)) {Source = this, Mode = BindingMode.OneWay});

            _button.SetBinding(Button.PaddingProperty, new Binding(nameof(Padding)) { Source = this, Mode = BindingMode.OneWay });

            _button.Pressed += Button_Pressed;
            _button.Released += Button_Released;

            _button.Clicked += (sender, e) => { Clicked?.Invoke(this, new EventArgs()); };
            _button.Released += (sender, e) => { Released?.Invoke(this, new EventArgs()); };
            _button.Pressed += (sender, e) => { Pressed?.Invoke(this, new EventArgs()); };

            Children.Add(_button, 0, 0);
            Children.Add(_frame, 0, 0);
            Children.Add(_loader, 0, 0);

        }

        public View Content
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
        ///     Gets or sets the Padding button.
        /// </summary>
        /// <value>the padding button.</value>
        public new Thickness Padding
        {
            get => (Thickness) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
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
        public int CornerRadius
        {
            get => (int) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        ///     Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public FileImageSource Image
        {
            get => (FileImageSource) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
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
        [TypeConverter(typeof(FontSizeConverter))]
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
        ///     Gets or sets background color value.
        /// </summary>
        /// <value>The background color.</value>
        public new Color BackgroundColor
        {
            get => (Color) GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
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
        public Color BorderColor
        {
            get => (Color) GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets highlight background color.
        /// </summary>
        /// <value>The highlight background color.</value>
        public Color HighlightBackgroundColor
        {
            get => (Color) GetValue(HighlightBackgroundColorProperty);
            set => SetValue(HighlightBackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets highlight border color.
        /// </summary>
        /// <value>The highlight border color.</value>
        public Color HighlightBorderColor
        {
            get => (Color) GetValue(HighlightBorderColorProperty);
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

        /// <summary>
        ///     Gets or sets highlight image color.
        /// </summary>
        /// <value>The highlight image color.</value>
        public Color HighlightImageColor
        {
            get => (Color) GetValue(HighlightImageColorProperty);
            set => SetValue(HighlightImageColorProperty, value);
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
        private static void ContentLayoutChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ButtonContent button) button._button.ContentLayout = (ButtonContentLayout) newValue;
        }


        private static void BorderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ButtonContent button)) return;
            button._frame.Margin = new Thickness(button.BorderWidth);

            var frameCorner = (float)(button.CornerRadius - button.BorderWidth);
            if (frameCorner > 0)
                button._frame.CornerRadius = frameCorner;
            button._button.Padding = button.Padding;

        }

        /// <summary>
        ///     The Corner Radius property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void CornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ButtonContent button)) return;
            var value = (int)newValue;
            button._button.CornerRadius = value;

            var frameCorner = (float)(button.CornerRadius - button.BorderWidth);
            if (frameCorner > 0)
                button._frame.CornerRadius = frameCorner;
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
                button._loader.IsVisible = true;
                button._button.Text = string.Empty;
                button._button.Image = null;
            }
            else
            {
                button._loader.IsVisible = false;
                button._button.Text = button.Text;
                button._button.Image = button.Image;
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
            if (bindable is ButtonContent button) button._button.BackgroundColor = (Color) newValue;
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
            if (bindable is ButtonContent button) button._button.BorderColor = (Color) newValue;
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            _button.BackgroundColor = HighlightBackgroundColor;
            _button.BorderColor = HighlightBorderColor;
            _button.TextColor = HighlightTextColor;
        }

        private void Button_Released(object sender, EventArgs e)
        {
            _button.BackgroundColor = BackgroundColor;
            _button.BorderColor = BorderColor;
            _button.TextColor = TextColor;
        }
    }
}