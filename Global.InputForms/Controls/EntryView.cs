using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Global.InputForms.Converters;
using Xamarin.Forms;

namespace Global.InputForms
{
    [ContentProperty(nameof(Children))]
    public class EntryView : EntryLayout
    {
        /// <summary>
        ///     The Entry Text property.
        /// </summary>
        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(nameof(EntryText), typeof(string), typeof(EntryView), string.Empty,
                propertyChanged: EntryTextChanged);

        /// <summary>
        ///     The Entry Horizontal Text Alignment property.
        /// </summary>
        public static readonly BindableProperty EntryHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(EntryHorizontalTextAlignment), typeof(TextAlignment), typeof(DatePickerView),
                TextAlignment.Start);

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
        ///     The IsPassword property.
        /// </summary>
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EntryView), false);

        /// <summary>
        ///     The Keyboard property.
        /// </summary>
        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryView), Keyboard.Default);

        private readonly BlankEntry _entry;

        public EventHandler<TextChangedEventArgs> TextChanged;
        public int _cursorPosition;
        public bool _blockNextChanged;

        public EntryView()
        {
            _entry = new BlankEntry
            {
                HeightRequest = 40,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            _entry.SetBinding(Entry.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.IsPasswordProperty,
                new Binding(nameof(IsPassword)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(InputView.KeyboardProperty,
                new Binding(nameof(Keyboard)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(InputView.IsSpellCheckEnabledProperty,
                new Binding(nameof(IsSpellCheckEnabled)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.IsTextPredictionEnabledProperty,
                new Binding(nameof(IsTextPredictionEnabled)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(InputView.IsReadOnlyProperty,
               new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay });

            _entry.Focused += FocusEntry;
            _entry.Unfocused += UnfocusEntry;
            _entry.TextChanged += OnEntryTextChanged;
            TextChanged += (sender, e) =>
            {
                if (InfoIsVisible)
                    Validate();
            };

            Unfocused += (sender, e) => Validate();
            

            Children.Add (_entry, 2, 3, 1, 2);
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
        ///     Gets or sets the entry horizontal text alignment.
        /// </summary>
        /// <value>The entry horizontal text alignment.</value>
        public TextAlignment EntryHorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(EntryHorizontalTextAlignmentProperty);
            set => SetValue(EntryHorizontalTextAlignmentProperty, value);
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

        private static void EntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryView entryView)) return;

            if (string.IsNullOrEmpty(entryView.Mask) && entryView._entry.Text != (string)newValue)
            {
                entryView._entry.Text = (string)newValue;
            }
            else
            {
                var masked = entryView.AddMask((string)newValue, false);
                entryView.MaskedEntryText = masked;
                if (entryView._entry.Text != masked)
                {
                    //entryView._entry.TextChanged += entryView.EntryCursorChanged;
                    entryView._entry.Text = masked;
               }
            }
            entryView.TextChanged?.Invoke(entryView, new TextChangedEventArgs((string)oldValue, (string)newValue));
        }

        void EntryCursorChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is Entry entry)) return;
            entry.CursorPosition = _cursorPosition;
            entry.TextChanged -= EntryCursorChanged;
        }


        private static void MaskEntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            /*
            //To Do
            if (!(bindable is EntryView entryView)) return;
            
            if (!string.IsNullOrEmpty(entryView.Mask))
            {
                    EntryView._entry.TextChanged -= EntryView.OnEntryTextChanged;
                    EntryView._entry.Text = EntryView.AddMask((string)newValue);
                    EntryView._entry.TextChanged -= EntryView.OnEntryTextChanged;
            }
            */
        }

        private string AddMask(string str, bool isAddition = true)
        {
            if (string.IsNullOrEmpty(str)) return null;

            var sb = new StringBuilder(str);

            var nbX = 0;
            for (var i = 0; i < Mask.Length; ++i)
                if (Mask[i] != 'X' && nbX < str.Length && Mask[i] != str[nbX] 
                    || isAddition && nbX == str.Length && Mask[i] != 'X')
                    sb.Insert(i, Mask[i]);
                else
                    ++nbX;
            return sb.ToString();
        }

        private string RemoveMask(string maskedString, string mask = null)
        {
            if (mask == null)
                mask = Mask;
            var sb = new StringBuilder(maskedString);

            for (var i = mask.Length - 1; i >= 0; --i)
                if (mask[i] != 'X' && i < maskedString.Length)
                    sb.Remove(i, 1);
            return sb.ToString();
        }

        private static void EntryMaskChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryView entryView)) return;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!(sender is Entry entry)) return;
            var isAddition = true;
            if (!string.IsNullOrEmpty(Mask))
            {
                if (entry.Text == MaskedEntryText)
                { 
                    return; 
                }

                var oldText = args.OldTextValue;
                var newText = args.NewTextValue;

                if (string.IsNullOrEmpty(newText) || string.IsNullOrEmpty(oldText))
                {
                    EntryText = newText;
                    return;
                }

                var cursor = entry.CursorPosition;
                var nbDiff = newText.Count() - oldText.Count();

                var maskTmp = Mask;
                if (nbDiff > 0 && cursor < Mask.Count() && cursor - nbDiff < Mask.Count()) // Addition
                {
                    //var a = 0;
                    //for (var i = cursor + nbDiff; i < Mask.Count() && Mask[i] != 'X'; i++)
                    //{ ++a; }

                    var str = new string('X', nbDiff);
                    maskTmp = Mask.Insert(cursor, str);
                    //_cursorPosition = cursor + nbDiff + a;
                }
                else if (nbDiff < 0 && cursor < Mask.Count()) // Deletion
                {
                    isAddition = false;
                    //var i = 0;
                    //var a = 0;
                    //for (i = cursor - Math.Abs(nbDiff); i > 0 && Mask[i] != 'X'; i--) 
                    //{
                    //    var maski = Mask[i];
                    //    ++a; 
                    //}
                    //_cursorPosition = (i > 0) ? i : 0;
                    _cursorPosition = cursor - Math.Abs(nbDiff);
                    //maskTmp = Mask.Remove(_cursorPosition, Math.Abs(nbDiff));//_cursorPosition, Math.Abs(nbDiff )+ a);
                    //newText = newText.Remove(_cursorPosition, Math.Min(Math.Abs(nbDiff) + a, newText.Count() - _cursorPosition));

                    if (Device.RuntimePlatform == Device.iOS && Math.Abs(nbDiff) > 1) //iOS
                    {
                        maskTmp = Mask.Remove(cursor, Math.Abs(nbDiff));
                        _cursorPosition = cursor;
                    }
                    else
                    {
                        maskTmp = Mask.Remove(_cursorPosition, Math.Abs(nbDiff));
                    }
                }
                var newLightText = RemoveMask(newText, maskTmp);
                var oldLightText = RemoveMask(oldText);

                EntryText = newLightText;

                if (EntryText == newLightText)
                {
                    var masked = AddMask(newLightText, isAddition);
                    MaskedEntryText = masked;
                    entry.Text = masked;
                }
            }
            else
            {
                MaskedEntryText = EntryText = args.NewTextValue;
            }
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

        protected override void SetCornerPaddingLayout()
        {
            base.SetCornerPaddingLayout();

            if (EntryCornerRadius >= 1f)
            {
                var thick = 0.0;
                if (BorderRelative) thick = Convert.ToDouble(EntryCornerRadius);
                _entry.Margin = new Thickness(thick, 0, thick, 0);
            }
            else
            {
                _entry.Margin = 0;
            }
        }
    }
}