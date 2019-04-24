using System;
using Xamarin.Forms;

namespace Global.InputForms
{
    [ContentProperty(nameof(Content))]
    public class FrameInfo : StackLayout
    {
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content),
            typeof(View), typeof(ContentView), null, propertyChanged: OnContentChanged);

        /// <summary>
        ///     The Info Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontAttributesProperty =
            BindableProperty.Create(nameof(InfoLabelFontAttributes), typeof(FontAttributes), typeof(FrameInfo),
                FontAttributes.Bold);

        /// <summary>
        ///     The Info Label Font Family property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontFamilyProperty =
            BindableProperty.Create(nameof(InfoLabelFontFamily), typeof(string), typeof(FrameInfo), string.Empty);

        /// <summary>
        ///     The Info Label Font Size property.
        /// </summary>
        public static readonly BindableProperty InfoLabelFontSizeProperty =
            BindableProperty.Create(nameof(InfoLabelFontSize), typeof(double), typeof(FrameInfo),
                Device.GetNamedSize(NamedSize.Micro, typeof(Label)));

        /// <summary>
        ///     The Info Label Horizontal TextAlignment property.
        /// </summary>
        public static readonly BindableProperty InfoLabelHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoLabelHorizontalTextAlignment), typeof(TextAlignment), typeof(FrameInfo),
                TextAlignment.Start);

        /// <summary>
        ///     The Info Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty InfoLabelVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(InfoLabelVerticalTextAlignment), typeof(TextAlignment), typeof(FrameInfo),
                TextAlignment.Start);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty InfoColorProperty =
            BindableProperty.Create(nameof(InfoColor), typeof(Color), typeof(FrameInfo), Color.Red,
                propertyChanged: InfoColorChanged);

        /// <summary>
        ///     The Info Label Text property.
        /// </summary>
        public static readonly BindableProperty InfoLabelTextProperty =
            BindableProperty.Create(nameof(InfoLabelText), typeof(string), typeof(FrameInfo), string.Empty,
                propertyChanged: InfoLabelTextChanged);

        /// <summary>
        ///     The Info is visible property.
        /// </summary>
        public static readonly BindableProperty InfoIsVisibleProperty =
            BindableProperty.Create(nameof(InfoIsVisible), typeof(bool), typeof(FrameInfo), false,
                propertyChanged: InfoIsVisibleChanged);

        /// <summary>
        ///     The Info is visible property.
        /// </summary>
        public static readonly BindableProperty InfoProperty =
            BindableProperty.Create(nameof(Info), typeof(bool), typeof(FrameInfo), false);

        private readonly Frame _frameLayout;

        private Label _infoLabel;


        public EventHandler<EventArgs> Validators;

        public FrameInfo()
        {
            Spacing = 0;
            Orientation = StackOrientation.Vertical;

            _frameLayout = new Frame
            {
                Padding = 10,
                BackgroundColor = Color.Transparent,
                CornerRadius = 5,
                BorderColor = Info ? InfoColor : Color.Transparent,
                HasShadow = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Children.Add(_frameLayout);
        }

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
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
            set
            {
                SetValue(InfoIsVisibleProperty, value);
                _infoLabel.IsVisible = value;
                _frameLayout.BorderColor = value ? InfoColor : Color.Transparent;
            }
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


        private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FrameInfo frameInfo)
            {
                frameInfo._frameLayout.Content = (View)newValue;
                frameInfo._frameLayout.Content.Parent = frameInfo;
            }
        }

        /// <summary>
        ///     The Info Label Font Attributes property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelFontAttributesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FrameInfo frameInfo && frameInfo._infoLabel is Label label)
                label.FontAttributes = (FontAttributes) newValue;
        }

        /// <summary>
        ///     The Info Label Font Family property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FrameInfo frameInfo && frameInfo._infoLabel is Label label)
                label.FontFamily = (string) newValue;
        }

        /// <summary>
        ///     The Info Label Font Size property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FrameInfo frameInfo && frameInfo._infoLabel is Label label)
                label.FontSize = (double) newValue;
        }

        /// <summary>
        ///     The Info Label Horizontal TextAlignment property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelHorizontalTextAlignmentChanged(BindableObject bindable, object oldValue,
            object newValue)
        {
            if (bindable is FrameInfo frameInfo && frameInfo._infoLabel is Label label)
                label.HorizontalTextAlignment = (TextAlignment) newValue;
        }

        /// <summary>
        ///     The Info Label Vertical Text Alignment property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelVerticalTextAlignmentChanged(BindableObject bindable, object oldValue,
            object newValue)
        {
            if (bindable is FrameInfo frameInfo && frameInfo._infoLabel is Label label)
                label.VerticalTextAlignment = (TextAlignment) newValue;
        }

        /// <summary>
        ///     The Info Label Text property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FrameInfo frameInfo) || frameInfo._infoLabel != null) return;
            
            frameInfo._infoLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            frameInfo._infoLabel.SetBinding(Label.FontAttributesProperty, new Binding(nameof(InfoLabelFontAttributes))
                {Source = frameInfo, Mode = BindingMode.OneWay});
            frameInfo._infoLabel.SetBinding(Label.FontFamilyProperty, new Binding(nameof(InfoLabelFontFamily))
                {Source = frameInfo, Mode = BindingMode.OneWay});
            frameInfo._infoLabel.SetBinding(Label.FontSizeProperty, new Binding(nameof(InfoLabelFontSize))
                {Source = frameInfo, Mode = BindingMode.OneWay});
            frameInfo._infoLabel.SetBinding(Label.HorizontalTextAlignmentProperty, new Binding(nameof(InfoLabelHorizontalTextAlignment))
                {Source = frameInfo, Mode = BindingMode.OneWay});
            frameInfo._infoLabel.SetBinding(Label.VerticalTextAlignmentProperty, new Binding(nameof(InfoLabelVerticalTextAlignment))
                {Source = frameInfo, Mode = BindingMode.OneWay});
            frameInfo._infoLabel.SetBinding(Label.TextProperty, new Binding(nameof(InfoLabelText))
                {Source = frameInfo, Mode = BindingMode.OneWay});
            frameInfo._infoLabel.SetBinding(Label.TextColorProperty, new Binding(nameof(InfoColor))
                {Source = frameInfo, Mode = BindingMode.OneWay});
            frameInfo._infoLabel.SetBinding(IsVisibleProperty, new Binding(nameof(InfoIsVisible))
                {Source = frameInfo, Mode = BindingMode.OneWay});
                    
            frameInfo.Children.Add(frameInfo._infoLabel);
        }

        /// <summary>
        ///     The Info Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FrameInfo frameInfo && frameInfo._infoLabel is Label label)
                label.TextColor = (Color) newValue;
        }

        /// <summary>
        ///     The Info Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void InfoIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FrameInfo frameInfo) || !(frameInfo._infoLabel is Label label)) return;
            
            label.IsVisible = (bool) newValue;
            frameInfo._frameLayout.BorderColor = (bool) newValue ? frameInfo.InfoColor : Color.Transparent;
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

        public bool Validate()
        {
            Info = false;
            Validators?.Invoke(_frameLayout.Content, new EventArgs());
            return !Info;
        }
    }
}