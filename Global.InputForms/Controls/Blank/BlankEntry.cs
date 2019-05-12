using Xamarin.Forms;

namespace Global.InputForms
{
    public class BlankEntry : Entry
    {
        /// <summary>
        ///     The Clip Board Menu property.
        /// </summary>
        public static readonly BindableProperty IsClipBoardMenuVisibleProperty =
            BindableProperty.Create(nameof(IsClipBoardMenuVisible), typeof(bool), typeof(BlankEntry), true);

        /// <summary>
        ///     Gets or sets the clip board menu visibility text.
        /// </summary>
        /// <value>The entry text.</value>
        public bool IsClipBoardMenuVisible
        {
            get => (bool)GetValue(IsClipBoardMenuVisibleProperty);
            set => SetValue(IsClipBoardMenuVisibleProperty, value);
        }
    }
}