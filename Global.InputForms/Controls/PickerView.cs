using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList),
            typeof(PickerView), null);

        /// <summary>
        ///     The Format property.
        /// </summary>
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int),
            typeof(PickerView), -1);

        /// <summary>
        ///     The Minimum Date property.
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
            typeof(object), typeof(PickerView), null);

        /// <summary>
        ///     The Maximum Date property.
        /// </summary>
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
            typeof(string), typeof(PickerView), string.Empty);

        /// <summary>
        ///     The Maximum Date property.
        /// </summary>
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor),
            typeof(Color), typeof(PickerView), Color.Black);

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(PickerView), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(PickerView), "Cancel");

        private readonly BlankPicker _picker;
        private readonly Frame _pFrame;
        public EventHandler SelectedIndexChanged;

        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;

        public PickerView()
        {
            _picker = new BlankPicker
            {
                BackgroundColor = Color.Transparent,
            };
            _picker.SetBinding(Picker.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(Picker.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(Picker.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(BlankPicker.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(BlankPicker.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(BlankPicker.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(Picker.TextColorProperty,
                new Binding(nameof(EntryTextColor)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) { Source = this, Mode = BindingMode.OneWay });

            _picker.SetBinding(Picker.ItemsSourceProperty,
                new Binding(nameof(ItemsSource)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(Picker.SelectedIndexProperty,
                new Binding(nameof(SelectedIndex)) { Source = this, Mode = BindingMode.TwoWay });
            _picker.SetBinding(Picker.SelectedItemProperty,
                new Binding(nameof(SelectedItem)) { Source = this, Mode = BindingMode.TwoWay });
            _picker.SetBinding(Picker.TitleProperty,
                new Binding(nameof(Title)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(Picker.TitleColorProperty,
                new Binding(nameof(TitleColor)) { Source = this, Mode = BindingMode.OneWay });

            _picker.SetBinding(BlankPicker.DoneButtonTextProperty,
                new Binding(nameof(DoneButtonText)) { Source = this, Mode = BindingMode.OneWay });
            _picker.SetBinding(BlankPicker.CancelButtonTextProperty,
                new Binding(nameof(CancelButtonText)) { Source = this, Mode = BindingMode.OneWay });

            _pFrame = new Frame
            {
                Padding = 0,
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                Content = _picker
            };
            _pFrame.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter() });
            _pFrame.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay});
            _pFrame.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) { Source = this, Mode = BindingMode.OneWay });

            TextAlignmentCommand = new Command(() => TextAlignmentChanged());

            _picker.Focused += FocusEntry;
            _picker.Unfocused += UnfocusEntry;
            _picker.SelectedIndexChanged += IndexChanged;

            _picker.DoneClicked += (sender, e) => DoneClicked?.Invoke(this, e);
            _picker.CancelClicked += (sender, e) => CancelClicked?.Invoke(this, e);

            Children.Add(_pFrame, 2, 3, 1, 2);
        }

        public IList ItemsSource 
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public Color TitleColor
        {
            get => (Color)GetValue(TitleColorProperty);
            set => SetValue(TitleColorProperty, value);
        }

        public string DoneButtonText
        {
            get => (string)GetValue(DoneButtonTextProperty);
            set => SetValue(DoneButtonTextProperty, value);
        }

        public string CancelButtonText
        {
            get => (string)GetValue(CancelButtonTextProperty);
            set => SetValue(CancelButtonTextProperty, value);
        }

        private void TextAlignmentChanged()
        {
            switch (EntryHorizontalTextAlignment)
            {
                case TextAlignment.Center:
                    _picker.HorizontalOptions = LayoutOptions.Center;
                    break;
                case TextAlignment.End:
                    _picker.HorizontalOptions = LayoutOptions.End;
                    break;
                case TextAlignment.Start:
                    _picker.HorizontalOptions = LayoutOptions.Start;
                    break;
            }
        }

        public override void Focus()
        {
            _picker.Focus();
        }

        public override void Unfocus()
        {
            _picker.Unfocus();
        }

        void IndexChanged(object sender, EventArgs e)
        {
            if (!(sender is BlankPicker picker)) return;
            SelectedIndexChanged?.Invoke(this, e);
        }

        protected override void SetCornerPaddingLayout()
        {
            base.SetCornerPaddingLayout();

            if (EntryCornerRadius >= 1f)
            {
                var thick = 0.0;
                if (BorderRelative) thick = Convert.ToDouble(EntryCornerRadius);
                _picker.Margin = new Thickness(thick, 0, thick, 0);
            }
            else
            {
                _picker.Margin = 0;
            }
        }
    }
}
