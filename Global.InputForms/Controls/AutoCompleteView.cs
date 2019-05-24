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
    public class AutoCompleteView : EntryView
    {
        /// <summary>
        ///     The execute on suggestion click property.
        /// </summary>
        public static readonly BindableProperty ExecuteOnItemClickProperty =
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
        public static readonly BindableProperty ListBackgroundColorProperty =
            BindableProperty.Create(nameof(ListBackgroundColor), typeof(Color), typeof(AutoCompleteView),
                Color.Red, propertyChanged: ListBackgroundColorChanged);

        /// <summary>
        ///     The suggestion item data template property.
        /// </summary>
        public static readonly BindableProperty ItemDataTemplateProperty =
            BindableProperty.Create(nameof(ItemDataTemplate), typeof(DataTemplate), typeof(AutoCompleteView),
                null, propertyChanged: SuggestionItemDataTemplateChanged);

        /// <summary>
        ///     The suggestions property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteView), null,
                propertyChanged: ItemsSourceChanged);
                
        private readonly Frame _frameList;

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

            TextChanged += (sender, e) =>
            {
                TextChangedHandler((string)e.NewTextValue);
            };

            _lstSuggestions.ItemSelected += (s, e) =>
            {
                EntryText = e.SelectedItem.ToString();

                _availableSuggestions.Clear();
                _lstSuggestions.ItemsSource = _availableSuggestions;
                ShowHideListbox(false);
                OnSelectedItemChanged(e.SelectedItem);
            };

            ShowHideListbox(false);
            _lstSuggestions.ItemsSource = _availableSuggestions;

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Children.Add(_frameList, 1, 4, 3, 4);
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (child == _frameList)
                SetRow(child, 3);
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
            get => (bool) GetValue(ExecuteOnItemClickProperty);
            set => SetValue(ExecuteOnItemClickProperty, value);
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
        public Color ListBackgroundColor
        {
            get => (Color) GetValue(ListBackgroundColorProperty);
            set => SetValue(ListBackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the suggestion item data template.
        /// </summary>
        /// <value>The sugestion item data template.</value>
        public DataTemplate ItemDataTemplate
        {
            get => (DataTemplate) GetValue(ItemDataTemplateProperty);
            set => SetValue(ItemDataTemplateProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Suggestions.
        /// </summary>
        /// <value>The Suggestions.</value>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     Occurs when [selected item changed].
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

        public event EventHandler<EventArgs> BackgroundClicked;

        private void BackGroundTapped(object sender, EventArgs e)
        {
            BackgroundClicked?.Invoke(sender, e);
        }

        /// <summary>
        ///     Suggestions the background color changed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ListBackgroundColorChanged(BindableObject obj, object oldValue, object newValue)
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
        private static void ItemsSourceChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (obj is AutoCompleteView autoCompleteView)
                autoCompleteView.ItemsSource = (IEnumerable) newValue;
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
                    if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && ItemsSource != null)
                    {
                        List<object> filteredSuggestions = null;
                        await Task.Run(() =>
                        {
                            filteredSuggestions = ItemsSource.Cast<object>()
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
        ///     <paramref name="show" />/ listbox.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        private void ShowHideListbox(bool show)
        {
            _lstSuggestions.IsVisible = show;
        }
    }
}