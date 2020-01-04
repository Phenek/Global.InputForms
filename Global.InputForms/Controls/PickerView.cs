using System;
using System.Collections;
using Global.InputForms.Converters;
using Xamarin.Forms;

namespace Global.InputForms
{
    [ContentProperty(nameof(Children))]
    public class PickerView : EntryLayout
    {
        /// <summary>
        ///     The Date property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IList),
            typeof(PickerView),
            propertyChanged: ItemsSourceChanged);

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex),
            typeof(int),
            typeof(PickerView),
            -1,
            propertyChanged: SelectedIdexChanged);

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
            typeof(object), typeof(PickerView),
            propertyChanged: SelectedItemChanged);

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
            typeof(string), typeof(PickerView), string.Empty);

        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor),
            typeof(Color), typeof(PickerView), Color.Black);

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(PickerView), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(PickerView), "Cancel");

        public static readonly BindableProperty UpdateModeProperty =
            BindableProperty.Create(nameof(UpdateMode), typeof(UpdateMode), typeof(PickerView), UpdateMode.Immediately);

        private readonly BlankPicker _picker;
        public EventHandler SelectedIndexChanged;

        public PickerView()
        {
            _picker = new BlankPicker
            {
                BackgroundColor = Color.Transparent
            };
            Input = _picker;
            _picker.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryText)) {Source = this, Mode = BindingMode.OneWayToSource});
            _picker.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryText)) {Source = this, Mode = BindingMode.TwoWay});
            _picker.SetBinding(Entry.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(Entry.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(Entry.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(Entry.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(Entry.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(Entry.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(Entry.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(MarginProperty,
                new Binding(nameof(EntryMargin)) {Source = this, Mode = BindingMode.OneWay});

            //_picker.SetBinding(BlankPicker.ItemsSourceProperty,
            //    new Binding(nameof(ItemsSource)) {Source = this, Mode = BindingMode.OneWay});
            //_picker.SetBinding(BlankPicker.SelectedIndexProperty,
            //    new Binding(nameof(SelectedIndex)) {Source = this, Mode = BindingMode.TwoWay});
            //_picker.SetBinding(BlankPicker.SelectedItemProperty,
            //    new Binding(nameof(SelectedItem)) {Source = this, Mode = BindingMode.TwoWay});
            _picker.SetBinding(BlankPicker.TitleProperty,
                new Binding(nameof(Title)) {Source = this, Mode = BindingMode.OneWay});

            _picker.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly))
                    {Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter()});
            _picker.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) {Source = this, Mode = BindingMode.OneWay});
            //Todo For Xamarin.Forms 4.0
            //_picker.SetBinding(Picker.TitleColorProperty,
            //new Binding(nameof(TitleColor)) { Source = this, Mode = BindingMode.OneWay });

            _picker.SetBinding(BlankPicker.DoneButtonTextProperty,
                new Binding(nameof(DoneButtonText)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(BlankPicker.CancelButtonTextProperty,
                new Binding(nameof(CancelButtonText)) {Source = this, Mode = BindingMode.OneWay});
            _picker.SetBinding(BlankPicker.UpdateModeProperty,
                new Binding(nameof(UpdateMode)) {Source = this, Mode = BindingMode.OneWay});

            _picker.Focused += FocusEntry;
            _picker.Unfocused += UnfocusEntry;
            _picker.SelectedIndexChanged += IndexChanged;
            _picker.TextChanged += SendEntryTextChanged;

            _picker.DoneClicked += (sender, e) => DoneClicked?.Invoke(this, e);
            _picker.CancelClicked += (sender, e) => CancelClicked?.Invoke(this, e);

            FloatingLabelWithoutAnimation();

            Children.Add(_picker, 2, 3, 1, 2);
        }

        public UpdateMode UpdateMode
        {
            get => (UpdateMode) GetValue(UpdateModeProperty);
            set => SetValue(UpdateModeProperty, value);
        }

        public IList ItemsSource
        {
            get => (IList) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public Color TitleColor
        {
            get => (Color) GetValue(TitleColorProperty);
            set => SetValue(TitleColorProperty, value);
        }

        public string DoneButtonText
        {
            get => (string) GetValue(DoneButtonTextProperty);
            set => SetValue(DoneButtonTextProperty, value);
        }

        public string CancelButtonText
        {
            get => (string) GetValue(CancelButtonTextProperty);
            set => SetValue(CancelButtonTextProperty, value);
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PickerView picker)
                picker._picker.ItemsSource = (IList) newValue;
        }

        private static void SelectedIdexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PickerView picker)
                picker._picker.SelectedIndex = (int) newValue;
        }

        private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PickerView picker)
                picker._picker.SelectedItem = (string) newValue;
        }

        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;

        public override void Focus()
        {
            _picker.Focus();
        }

        public override void Unfocus()
        {
            _picker.Unfocus();
        }

        private void IndexChanged(object sender, EventArgs e)
        {
            if (_picker.SelectedIndex != SelectedIndex)
            {
                SelectedIndex = _picker.SelectedIndex;
                SelectedItem = ItemsSource[SelectedIndex];
            }
            SelectedIndexChanged?.Invoke(this, e);
        }
    }
}