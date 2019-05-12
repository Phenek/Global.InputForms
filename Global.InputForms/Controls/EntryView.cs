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

        /// <summary>
        ///     The Clip Board Menu property.
        /// </summary>
        public static readonly BindableProperty IsClipBoardMenuVisibleProperty =
            BindableProperty.Create(nameof(IsClipBoardMenuVisible), typeof(bool), typeof(BlankEntry), true);

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
                HorizontalOptions = LayoutOptions.FillAndExpand
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
            _entry.SetBinding(BlankEntry.IsClipBoardMenuVisibleProperty,
                new Binding(nameof(IsClipBoardMenuVisible)) { Source = this, Mode = BindingMode.OneWay });

            var fEntry = new Frame
            {
                Padding = 0,
                HeightRequest = 40,
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                Content = _entry
            };
            fEntry.SetBinding(IsEnabledProperty,
                new Binding(nameof(EntryIsEnabled)) { Source = this, Mode = BindingMode.OneWay });
            fEntry.SetBinding(InputTransparentProperty,
                new Binding(nameof(EntryIsEnabled)) { Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter() });
                
            _entry.Focused += FocusEntry;
            _entry.Unfocused += UnfocusEntry;
            _entry.TextChanged += OnEntryTextChanged;
            TextChanged += (sender, e) =>
            {
                if (InfoIsVisible)
                    Validate();
            };

            Unfocused += (sender, e) => Validate();
            

            Children.Add (fEntry, 2, 3, 1, 2);
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

        /// <summary>
        ///  Gets or sets the clip board menu visibility text.
        /// </summary>
        /// <value>The entry text.</value>
        public bool IsClipBoardMenuVisible
        {
            get => (bool)GetValue(IsClipBoardMenuVisibleProperty);
            set => SetValue(IsClipBoardMenuVisibleProperty, value);
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
                    if (entryView._entry.IsFocused) 
                        entryView._entry.TextChanged += entryView.EntryCursorChanged;
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

        private string AddMask(string str, bool addition = true)
        {
            if (string.IsNullOrEmpty(str)) return null;
            
            var sb = new StringBuilder(str);

            var nbX = 0;
            for (var i = 0; i < Mask.Length; ++i)
                if (Mask[i] != 'X' && nbX < str.Length && Mask[i] != str[nbX] 
                    || addition && nbX == str.Length && Mask[i] != 'X')
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

            //entryView._entry.IsReadOnly = true;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!(sender is Entry entry)) return;

            if (!string.IsNullOrEmpty(Mask))
            {
                var oldText = args.OldTextValue;
                var newText = args.NewTextValue;

                if (entry.Text == MaskedEntryText || newText == oldText)
                { 
                    return; 
                }

                if (string.IsNullOrEmpty(newText) || string.IsNullOrEmpty(oldText))
                {
                    EntryText = newText;
                    return;
                }

                var cursor = entry.CursorPosition;
                var nbDiff = newText.Count() - oldText.Count();
                var isAddition = true;

                var maskTmp = Mask;
                if (nbDiff > 0 && cursor < Mask.Count() && cursor - nbDiff < Mask.Count()) // Addition
                {
                    var a = (Mask[cursor] != 'X') ? 1 : 0;
                    for (var i = cursor + nbDiff; i < Mask.Count() && Mask[i] != 'X'; i++)
                    { ++a; }

                    var str = new string('X', nbDiff);
                    maskTmp = Mask.Insert(cursor, str);
                    _cursorPosition = cursor + nbDiff + a;
                }
                else if (nbDiff < 0 && cursor < Mask.Count()) // Deletion
                {
                    isAddition = false;
                    var i = 0;
                    var a = 0;
                    for (i = cursor - Math.Abs(nbDiff); i > 0 && Mask[i] != 'X'; i--) 
                    {
                        var maski = Mask[i];
                        ++a; 
                    }
                    // Math.Abs(nbDiff) > 2 occured when there is an Entry Selection.
                    //_cursorPosition = (Device.RuntimePlatform == Device.iOS) ? cursor : cursor - Math.Abs(nbDiff);

                    _cursorPosition = cursor - Math.Abs(nbDiff);

                    maskTmp = Mask.Remove(_cursorPosition, Math.Abs(nbDiff) + a); // +a;

                    if (Math.Abs(nbDiff) < 2 && _cursorPosition < newText.Count() && _cursorPosition + a < newText.Count())
                        newText = newText.Remove(_cursorPosition, Math.Min(a, newText.Count() -1));
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