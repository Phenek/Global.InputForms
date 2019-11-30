using Global.InputForms;
using Xamarin.Forms;

namespace SampleApp.Controls
{
    public partial class ButtonInputs : ButtonContent
    {
        /// <summary>
        ///     The Entry Value property.
        /// </summary>
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(string), typeof(ButtonInputs), string.Empty);

        /// <summary>
        ///     The Entry Title property.
        /// </summary>
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(ButtonInputs), string.Empty);

        public ButtonInputs()
        {
            InitializeComponent();

            _title.SetBinding(Label.TextProperty,
                new Binding(nameof(Title)) {Source = this, Mode = BindingMode.OneWay});
        }

        /// <summary>
        ///     Gets or sets the Value text.
        /// </summary>
        /// <value>The entry text.</value>
        public string Value
        {
            get => (string) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Title text.
        /// </summary>
        /// <value>The entry text.</value>
        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}