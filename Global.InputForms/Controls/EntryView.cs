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

        private readonly BlankEntry _entry;
        private readonly Frame _pFrame;

        public EventHandler<TextChangedEventArgs> TextChanged;
        public int _cursorPosition;
        public bool _blockNextChanged;
        private bool _isAdditon;

        public EntryView()
        {
            _entry = new BlankEntry
            {
                BackgroundColor = Color.Transparent,
            };
            _entry.SetBinding(Entry.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(HeightProperty,
                 new Binding(nameof(EntryFontFamily)) { Source = this, Mode = BindingMode.OneWay });
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
            _entry.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) { Source = this, Mode = BindingMode.OneWay });

            _entry.SetBinding(Entry.IsPasswordProperty,
                new Binding(nameof(IsPassword)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(InputView.KeyboardProperty,
                new Binding(nameof(Keyboard)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(InputView.IsSpellCheckEnabledProperty,
                new Binding(nameof(IsSpellCheckEnabled)) {Source = this, Mode = BindingMode.OneWay});
            _entry.SetBinding(Entry.IsTextPredictionEnabledProperty,
                new Binding(nameof(IsTextPredictionEnabled)) {Source = this, Mode = BindingMode.OneWay});
            //Todo For Xamarin.Forms 4.0
            //_entry.SetBinding(InputView.IsReadOnlyProperty,
            //new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay });
            _entry.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay });

            _pFrame = new Frame
            {
                Padding = 0,
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                Content = _entry
            };
            _pFrame.SetBinding(IsEnabledProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay, Converter = new InverseBooleanConverter() });
            _pFrame.SetBinding(InputTransparentProperty,
                new Binding(nameof(IsReadOnly)) { Source = this, Mode = BindingMode.OneWay });
            _pFrame.SetBinding(HeightRequestProperty,
                new Binding(nameof(EntryHeightRequest)) { Source = this, Mode = BindingMode.OneWay });


            _entry.Focused += FocusEntry;
            _entry.Unfocused += UnfocusEntry;
            _entry.TextChanged += OnEntryTextChanged;
            TextChanged += (sender, e) =>
            {
                if (InfoIsVisible)
                    Validate();
            };
            Unfocused += (sender, e) =>
            {
                Validate();
            };

            Children.Add (_pFrame, 2, 3, 1, 2);
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

        private static void EntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is EntryView entryView)) return;

            if (string.IsNullOrEmpty(entryView.Mask) && entryView._entry.Text != (string)newValue)
            {
                entryView._entry.Text = (string)newValue;
            }
            else
            {
                var masked = entryView.AddMask((string)newValue, entryView._isAdditon);
                if (entryView.MaskedEntryText != masked)
                {
                    entryView.MaskedEntryText = masked;
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
            if (!(bindable is EntryView entryView)) return;

            if (!string.IsNullOrEmpty(entryView.Mask))
            {
                //entryView._entry.TextChanged += entryView.EntryCursorChanged;
                if (entryView._entry.Text != (string)newValue)
                    entryView._entry.Text = (string)newValue;
                var light = entryView.RemoveMask((string)newValue);
                if (entryView.EntryText != light)
                {
                    entryView.EntryText = light;
                }
            }
        }

        private string AddMask(string str, bool isAddition = true)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            var sb = new StringBuilder(str);

            var nbX = 0;
            for (var i = 0; i < Mask.Length; ++i)
                if (Mask[i] != 'X' && nbX < str.Length // && Mask[i] != str[nbX]
                    || isAddition && nbX == str.Length && Mask[i] != 'X')
                    sb.Insert(i, Mask[i]);
                else
                    ++nbX;
            var s = sb.ToString();
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
            if (!string.IsNullOrEmpty(Mask))
            {
                _cursorPosition = entry.CursorPosition;
                if (entry.Text == MaskedEntryText || args.NewTextValue == null)
                    return;

                var oldText = args.OldTextValue;
                var newText = args.NewTextValue;
                if (string.IsNullOrEmpty(oldText))
                {
                    _isAdditon = true;
                    var m = AddMask(newText, true);
                    MaskedEntryText = m;
                    return;
                }
                var cursor = entry.CursorPosition;
                _isAdditon = newText.Count() - oldText.Count() > 0;

                int startIndex = oldText.Zip(newText, (c1, c2) => c1 == c2).TakeWhile(b => b).Count();
                int endIndex = oldText.Substring(startIndex).Reverse().Zip(newText.Substring(startIndex).Reverse(), (c1, c2) => c1 == c2).TakeWhile(b => b).Count();

                var oldCount = oldText.Count();
                var newCount = newText.Count();

                var oldMiddleString = oldText.Substring(startIndex, oldText.Count() - startIndex - endIndex);
                var newMiddleString = newText.Substring(startIndex, newText.Count() - startIndex - endIndex);

                var diff = Math.Abs(oldText.Count() - newText.Count());

                /*
                if (startIndex == entry.CursorPosition)
                    Console.WriteLine("startIndex OK");
                else if (Device.RuntimePlatform == Device.Android && entry.CursorPosition == oldText.Count() - endIndex)
                    Console.WriteLine("Android End selection");
               
                else if (diff > 1 && entry.CursorPosition < startIndex)
                {
                    Console.WriteLine("Wrong startIndex start from cursor");
                    startIndex = entry.CursorPosition;
                    endIndex = oldText.Substring(startIndex).Reverse().Zip(newText.Substring(startIndex).Reverse(), (c1, c2) => c1 == c2).TakeWhile(b => b).Count();

                    oldCount = oldText.Count();
                    newCount = newText.Count();

                    oldMiddleString = oldText.Substring(startIndex, oldText.Count() - startIndex - endIndex);
                    newMiddleString = newText.Substring(startIndex, newText.Count() - startIndex - endIndex);

                    Console.WriteLine("StartIndex Fixe");
                }
                else
                {
                    Console.WriteLine("startIndex Diff vs cursor");
                    Console.WriteLine($"Cursor : {entry.CursorPosition}");
                    Console.WriteLine($"oldMiddleString : {oldMiddleString}");
                    Console.WriteLine($"newMiddleString : {newMiddleString}");
                }
                */

                var endLightStr = "";
                var startLightStr = RemoveMask(newText.Substring(0, startIndex));
                if (startIndex + oldMiddleString.Count() < Mask.Count() )
                {
                    var endMaskedText = newText.Substring(newText.Count() - endIndex, endIndex);
                    var maskTmp = Mask.Substring(startIndex + oldMiddleString.Count());
                    endLightStr = RemoveMask(endMaskedText, maskTmp);
                }

                var newLightText = startLightStr + newMiddleString + endLightStr;
                var oldLightText = RemoveMask(oldText);

                _cursorPosition = startIndex + newMiddleString.Count();


                //EntryText = newLightText;


                var masked = AddMask(newLightText, _isAdditon);
                MaskedEntryText = masked;

                /*
                // this order no more
                var masked = AddMask(EntryText, isAddition);
                entry.TextChanged += EntryCursorChanged;
                MaskedEntryText = masked;
                entry.Text = masked;
                EntryText = newLightText;
                */

            }
            else
            {
                if (entry.Text == EntryText)
                    return;
                MaskedEntryText = EntryText = args.NewTextValue;
            }
        }

        public override void Focus()
        {
            _entry.Focus();
            _entry.CursorPosition = (string.IsNullOrEmpty(_entry.Text)) ? 0 : _entry.Text.Count();
        }

        public override void Unfocus()
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