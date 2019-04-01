using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class AutoCompleteView : ContentView
    {
        /// <summary>
        ///     The execute on suggestion click property.
        /// </summary>
        public static readonly BindableProperty ExecuteOnSuggestionClickProperty =
            BindableProperty.Create(nameof(ExecuteOnSuggestionClick), typeof(bool), typeof(AutoCompleteView), false);

        /// <summary>
        ///     The selected command property.
        /// </summary>
        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(AutoCompleteView), null);

        /// <summary>
        ///     The selected item property.
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AutoCompleteView), null);

        /// <summary>
        ///     The suggestion background color property.
        /// </summary>
        public static readonly BindableProperty SuggestionBackgroundColorProperty =
            BindableProperty.Create(nameof(SuggestionBackgroundColor), typeof(Color), typeof(AutoCompleteView),
                Color.Red, propertyChanged: SuggestionBackgroundColorChanged);

        /// <summary>
        ///     The suggestion item data template property.
        /// </summary>
        public static readonly BindableProperty SuggestionItemDataTemplateProperty =
            BindableProperty.Create(nameof(SuggestionItemDataTemplate), typeof(DataTemplate), typeof(AutoCompleteView),
                null, propertyChanged: SuggestionItemDataTemplateChanged);

        /// <summary>
        ///     The suggestions property.
        /// </summary>
        public static readonly BindableProperty SuggestionsProperty =
            BindableProperty.Create(nameof(Suggestions), typeof(IEnumerable), typeof(AutoCompleteView), null,
                propertyChanged: SuggestionChanged);

        public static readonly BindableProperty IsSpellCheckEnabledProperty =
            BindableProperty.Create(nameof(IsSpellCheckEnabled), typeof(bool), typeof(AutoCompleteView), true,
                BindingMode.OneTime);

        public static readonly BindableProperty IsTextPredictionEnabledProperty =
            BindableProperty.Create(nameof(IsTextPredictionEnabled), typeof(bool), typeof(AutoCompleteView), true,
                BindingMode.OneTime);

        /// <summary>
        ///     The Keyboard property.
        /// </summary>
        public static BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(AutoCompleteView), Keyboard.Default);

        /// <summary>
        ///     Border Relative To Corner Radius Property.
        /// </summary>
        public static readonly BindableProperty BorderRelativeProperty =
            BindableProperty.Create(nameof(BorderRelative), typeof(bool), typeof(AutoCompleteView), true,
                propertyChanged: BorderRelativeChanged);

        /// <summary>
        ///     The Entry Corner Radius property.
        /// </summary>
        public static readonly BindableProperty EntryCornerRadiusProperty =
            BindableProperty.Create(nameof(EntryCornerRadius), typeof(float), typeof(AutoCompleteView), 0f,
                propertyChanged: EntryCornerRadiusChanged);

        /// <summary>
        ///     The Entry Border Color property.
        /// </summary>
        public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(nameof(EntryBorderColor), typeof(Color), typeof(AutoCompleteView),
                Color.Transparent, propertyChanged: EntryBorderColorChanged);

        /// <summary>
        ///     The Entry Background Color property.
        /// </summary>
        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(AutoCompleteView),
                Color.Transparent, propertyChanged: EntryBackgroundColorChanged);

        /// <summary>
        ///     The Entry Font Attributes property.
        /// </summary>
        public static readonly BindableProperty EntryFontAttributesProperty =
            BindableProperty.Create(nameof(EntryFontAttributes), typeof(FontAttributes), typeof(AutoCompleteView),
                FontAttributes.None);

        /// <summary>
        ///     The Entry Font Family property.
        /// </summary>
        public static readonly BindableProperty EntryFontFamilyProperty =
            BindableProperty.Create(nameof(EntryFontFamily), typeof(string), typeof(AutoCompleteView), string.Empty);

        /// <summary>
        ///     The Entry Font Size property.
        /// </summary>
        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(nameof(EntryFontSize), typeof(double), typeof(AutoCompleteView),
                Device.GetNamedSize(NamedSize.Medium, typeof(AutoCompleteView)));

        /// <summary>
        ///     The Entry Horizontal Text Alignment property.
        /// </summary>
        public static readonly BindableProperty EntryHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(EntryHorizontalTextAlignment), typeof(TextAlignment),
                typeof(AutoCompleteView), TextAlignment.Start);

        /// <summary>
        ///     The Entry Placeholder property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderProperty =
            BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(AutoCompleteView), string.Empty);

        /// <summary>
        ///     The Entry Placeholder Color property.
        /// </summary>
        public static readonly BindableProperty EntryPlaceholderColorProperty =
            BindableProperty.Create(nameof(EntryPlaceholderColor), typeof(Color), typeof(AutoCompleteView),
                Color.Black);

        /// <summary>
        ///     The Entry Text Color property.
        /// </summary>
        public static readonly BindableProperty EntryTextColorProperty =
            BindableProperty.Create(nameof(EntryTextColor), typeof(Color), typeof(AutoCompleteView), Color.Black);

        /// <summary>
        ///     The Entry Text property.
        /// </summary>
        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(nameof(EntryText), typeof(string), typeof(AutoCompleteView), string.Empty,
                propertyChanged: EntryTextChanged);

        private readonly BlankEntry _entry;
        private readonly Frame _frameEntry;
        private readonly Frame _frameList;
        private readonly StackLayout _stackLayout;

        private ObservableCollection<object> _availableSuggestions;

        private CancellationTokenSource _cts;
        private readonly ListView _lstSuggestions;
        private readonly TapGestureRecognizer _backgroundTap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutoCompleteView" /> class.
        /// </summary>
        public AutoCompleteView()
        {
            _availableSuggestions = new ObservableCollection<object>();
            _stackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.Transparent
            };

            _entry = new BlankEntry
            {
                HeightRequest = 40,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _entry.SetBinding(Entry.FontAttributesProperty,
                new Binding(nameof(EntryFontAttributes)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.FontFamilyProperty,
                new Binding(nameof(EntryFontFamily)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.FontSizeProperty,
                new Binding(nameof(EntryFontSize)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.HorizontalTextAlignmentProperty,
                new Binding(nameof(EntryHorizontalTextAlignment)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.PlaceholderProperty,
                new Binding(nameof(EntryPlaceholder)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.PlaceholderColorProperty,
                new Binding(nameof(EntryPlaceholderColor)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.TextColorProperty,
                new Binding(nameof(EntryTextColor)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(InputView.KeyboardProperty,
                new Binding(nameof(Keyboard)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(InputView.IsSpellCheckEnabledProperty,
                new Binding(nameof(IsSpellCheckEnabled)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.IsTextPredictionEnabledProperty,
                new Binding(nameof(IsTextPredictionEnabled)) {Source = this, Mode = BindingMode.TwoWay});
            _entry.SetBinding(Entry.TextProperty,
                new Binding(nameof(EntryText)) {Source = this, Mode = BindingMode.TwoWay});

            _frameEntry = new Frame
            {
                Padding = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                HasShadow = false,
                Content = _entry
            };
            _frameEntry.SetBinding(Frame.CornerRadiusProperty,
                new Binding(nameof(EntryCornerRadius)) {Source = this, Mode = BindingMode.TwoWay});

            _lstSuggestions = new ListView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                SeparatorVisibility = SeparatorVisibility.None,
                HasUnevenRows = true
            };

            _frameList = new Frame
            {
                Padding = 0,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HasShadow = false,
                IsClippedToBounds = true,
                BackgroundColor = Color.Transparent,
                Content = _lstSuggestions
            };

            _backgroundTap = new TapGestureRecognizer();
            _backgroundTap.Tapped += BackGroundTapped;
            _frameList.GestureRecognizers.Add(_backgroundTap);

            _stackLayout.Children.Add(_frameEntry);
            _stackLayout.Children.Add(_frameList);

            Content = _stackLayout;

            _entry.TextChanged += (s, e) =>
            {
                EntryText = e.NewTextValue;
                OnTextChanged(e);
            };

            _lstSuggestions.ItemSelected += (s, e) =>
            {
                _entry.Text = e.SelectedItem.ToString();

                _availableSuggestions.Clear();
                _lstSuggestions.ItemsSource = _availableSuggestions;
                ShowHideListbox(false);
                OnSelectedItemChanged(e.SelectedItem);
            };

            _entry.Focused += Entry_Focused;
            _entry.Unfocused += Entry_Unfocused;

            ShowHideListbox(false);
            _lstSuggestions.ItemsSource = _availableSuggestions;

            _entry.Focus();
        }

        /// <summary>
        ///     Gets the available Suggestions.
        /// </summary>
        /// <value>The available Suggestions.</value>
        public IEnumerable AvailableSuggestions => _availableSuggestions;

        /// <summary>
        ///     Gets or sets a value indicating whether [execute on sugestion click].
        /// </summary>
        /// <value><c>true</c> if [execute on sugestion click]; otherwise, <c>false</c>.</value>
        public bool ExecuteOnSuggestionClick
        {
            get => (bool) GetValue(ExecuteOnSuggestionClickProperty);
            set => SetValue(ExecuteOnSuggestionClickProperty, value);
        }

        /// <summary>
        ///     Gets or sets the selected command.
        /// </summary>
        /// <value>The selected command.</value>
        public ICommand SelectedCommand
        {
            get => (ICommand) GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }

        /// <summary>
        ///     Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        ///     Gets or sets the color of the sugestion background.
        /// </summary>
        /// <value>The color of the sugestion background.</value>
        public Color SuggestionBackgroundColor
        {
            get => (Color) GetValue(SuggestionBackgroundColorProperty);
            set => SetValue(SuggestionBackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the suggestion item data template.
        /// </summary>
        /// <value>The sugestion item data template.</value>
        public DataTemplate SuggestionItemDataTemplate
        {
            get => (DataTemplate) GetValue(SuggestionItemDataTemplateProperty);
            set => SetValue(SuggestionItemDataTemplateProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Suggestions.
        /// </summary>
        /// <value>The Suggestions.</value>
        public IEnumerable Suggestions
        {
            get => (IEnumerable) GetValue(SuggestionsProperty);
            set => SetValue(SuggestionsProperty, value);
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
        ///     Gets or sets the Keyboard value.
        /// </summary>
        /// <value>the Keyboard value.</value>
        public Keyboard Keyboard
        {
            get => (Keyboard) GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry corner radius.
        /// </summary>
        /// <value>The entry background color.</value>
        public float EntryCornerRadius
        {
            get => (float) GetValue(EntryCornerRadiusProperty);
            set => SetValue(EntryCornerRadiusProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry border color.
        /// </summary>
        /// <value>The entry background color.</value>
        public Color EntryBorderColor
        {
            get => (Color) GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry background color.
        /// </summary>
        /// <value>The entry background color.</value>
        public Color EntryBackgroundColor
        {
            get => (Color) GetValue(EntryBackgroundColorProperty);
            set => SetValue(EntryBackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry font attributes.
        /// </summary>
        /// <value>The entry font attributes.</value>
        public FontAttributes EntryFontAttributes
        {
            get => (FontAttributes) GetValue(EntryFontAttributesProperty);
            set => SetValue(EntryFontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry font family.
        /// </summary>
        /// <value>The entry font family.</value>
        public string EntryFontFamily
        {
            get => (string) GetValue(EntryFontFamilyProperty);
            set => SetValue(EntryFontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry font size.
        /// </summary>
        /// <value>The entry font size.</value>
        [TypeConverter(typeof(FontSizeConverter))]
        public double EntryFontSize
        {
            get => (double) GetValue(EntryFontSizeProperty);
            set => SetValue(EntryFontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry horizontal text alignment.
        /// </summary>
        /// <value>The entry horizontal text alignment.</value>
        public TextAlignment EntryHorizontalTextAlignment
        {
            get => (TextAlignment) GetValue(EntryHorizontalTextAlignmentProperty);
            set => SetValue(EntryHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry placeholder.
        /// </summary>
        /// <value>The entry placeholdeer.</value>
        public string EntryPlaceholder
        {
            get => (string) GetValue(EntryPlaceholderProperty);
            set => SetValue(EntryPlaceholderProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry placeholder color.
        /// </summary>
        /// <value>The entry placeholder color.</value>
        public Color EntryPlaceholderColor
        {
            get => (Color) GetValue(EntryPlaceholderColorProperty);
            set => SetValue(EntryPlaceholderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the entry text color.
        /// </summary>
        /// <value>The entry text color.</value>
        public Color EntryTextColor
        {
            get => (Color) GetValue(EntryTextColorProperty);
            set => SetValue(EntryTextColorProperty, value);
        }

        public bool BorderRelative
        {
            get => (bool) GetValue(BorderRelativeProperty);
            set => SetValue(BorderRelativeProperty, value);
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
        ///     Occurs when [selected item changed].
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

        /// <summary>
        ///     Occurs when [text changed].
        /// </summary>
        public event EventHandler<TextChangedEventArgs> TextChanged;

        public new event EventHandler<FocusEventArgs> Focused;
        public new event EventHandler<FocusEventArgs> Unfocused;
        public event EventHandler<EventArgs> BackgroundClicked;

        private void BackGroundTapped(object sender, EventArgs e)
        {
            BackgroundClicked?.Invoke(sender, e);
        }

        /// <summary>
        ///     The entry corner radius property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoCompleteView autoCompleteView)
                autoCompleteView.SetCornerPaddingLayout();
        }

        /// <summary>
        ///     The entry border color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoCompleteView autoCompleteView)
                autoCompleteView._frameEntry.BorderColor = (Color) newValue;
        }

        /// <summary>
        ///     The Entry Background Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void EntryBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoCompleteView autoCompleteView)
                autoCompleteView._frameEntry.BackgroundColor = (Color) newValue;
        }

        private static void BorderRelativeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoCompleteView autoCompleteView)
                autoCompleteView.SetCornerPaddingLayout();
        }

        /// <summary>
        ///     Suggestions the background color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SuggestionBackgroundColorChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (obj is AutoCompleteView autoCompleteView)
                autoCompleteView._lstSuggestions.BackgroundColor = (Color) newValue;
        }

        /// <summary>
        ///     Suggestions the height changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SuggestionHeightRequestChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (obj is AutoCompleteView autoCompleteView)
                autoCompleteView._lstSuggestions.HeightRequest = (double) newValue;
        }

        /// <summary>
        ///     Suggestions the item data template changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldShowSearchValue">The old show search value.</param>
        /// <param name="newShowSearchValue">The new show search value.</param>
        private static void SuggestionItemDataTemplateChanged(BindableObject obj, object oldShowSearchValue,
            object newShowSearchValue)
        {
            if (obj is AutoCompleteView autoCompleteView)
                autoCompleteView._lstSuggestions.ItemTemplate = (DataTemplate) newShowSearchValue;
        }

        /// <summary>
        ///     Suggestions the item data template changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old show search value.</param>
        /// <param name="newValue">The new show search value.</param>
        private static void SuggestionChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (obj is AutoCompleteView autoCompleteView)
                autoCompleteView.Suggestions = (IEnumerable) newValue;
        }

        /// <summary>
        ///     Texts the changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old place holder value.</param>
        /// <param name="newValue">The new place holder value.</param>
        private static void EntryTextChanged(BindableObject obj, object oldValue, object newValue)
        {
            var newPlaceHolderValue = (string) newValue;

            if (obj is AutoCompleteView control) control.TextChangedHandler((string) newValue);
        }

        private async void TextChangedHandler(string text) // async void only for event handlers
        {
            try
            {
                _cts?.Cancel(); // cancel previous search
            }
            catch (ObjectDisposedException) // in case previous search completed
            {
            }

            using (_cts = new CancellationTokenSource())
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(333), _cts.Token); // buffer
                    var cleanedNewPlaceHolderValue =
                        Regex.Replace((text ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);
                    if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && Suggestions != null)
                    {
                        List<object> filteredSuggestions = null;
                        await Task.Run(() =>
                        {
                            filteredSuggestions = Suggestions.Cast<object>()
                                .Where(x => Regex.Replace(x.ToString().ToLowerInvariant(), @"\s+", string.Empty)
                                    .Contains(cleanedNewPlaceHolderValue))
                                .OrderByDescending(x => Regex.Replace(x.ToString()
                                        .ToLowerInvariant(), @"\s+", string.Empty)
                                    .StartsWith(cleanedNewPlaceHolderValue, StringComparison.CurrentCulture)).ToList();
                        });
                        _availableSuggestions = new ObservableCollection<object>();
                        if (filteredSuggestions != null && filteredSuggestions.Count > 0)
                        {
                            foreach (var suggestion in filteredSuggestions) _availableSuggestions.Add(suggestion);
                            ShowHideListbox(true);
                        }
                        else
                        {
                            ShowHideListbox(false);
                        }
                    }
                    else
                    {
                        if (_availableSuggestions.Count > 0)
                        {
                            _availableSuggestions = new ObservableCollection<object>();
                            ShowHideListbox(false);
                        }
                    }

                    _lstSuggestions.ItemsSource = _availableSuggestions;

                    if (_availableSuggestions.Any() && _frameList.GestureRecognizers.Contains(_backgroundTap))
                    {
                        _backgroundTap.Tapped -= BackGroundTapped;
                        _frameList.GestureRecognizers.Remove(_backgroundTap);
                    }
                    else if (!_availableSuggestions.Any() && !_frameList.GestureRecognizers.Contains(_backgroundTap))
                    {
                        _backgroundTap.Tapped += BackGroundTapped;
                        _frameList.GestureRecognizers.Add(_backgroundTap);
                    }
                }
                catch (TaskCanceledException) // if the operation is cancelled, do nothing
                {
                }
            }
        }

        /// <summary>
        ///     Called when [selected item changed].
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        private void OnSelectedItemChanged(object selectedItem)
        {
            SelectedItem = selectedItem;

            if (SelectedCommand != null)
                SelectedCommand.Execute(selectedItem);

            SelectedItemChanged?.Invoke(this, new SelectedItemChangedEventArgs(selectedItem));
        }

        /// <summary>
        ///     Handles the <see cref="E:TextChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void OnTextChanged(TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     <paramref name="show" />/ listbox.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        private void ShowHideListbox(bool show)
        {
            _lstSuggestions.IsVisible = show;
        }

        public new void Focus()
        {
            _entry.Focus();
        }

        private void SetCornerPaddingLayout()
        {
            if (EntryCornerRadius >= 1f)
            {
                var thick = 0.0;
                if (BorderRelative) thick = Convert.ToDouble(EntryCornerRadius);
                _frameEntry.Padding = new Thickness(thick, 0, thick, 0);
            }
            else
            {
                _frameEntry.Padding = 0;
            }
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            Focused?.Invoke(this, new FocusEventArgs(this, true));
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            Unfocused?.Invoke(this, new FocusEventArgs(this, true));
        }
    }
}