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
        ///     The default sorting algorithm.
        /// </summary>
        private static readonly Func<string, IEnumerable<object>, IEnumerable<object>> _defaultAlgo =
            (text, values) => values
                                .Where(x => x.ToString().IndexOf(text, StringComparison.CurrentCultureIgnoreCase) > -1)
                                .OrderByDescending(x => x.ToString().ToLowerInvariant().StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                                .ThenBy(x => x.ToString(), new NaturalSortComparer<string>())
                                .Take(40).ToList();

        /// <summary>
        ///     The sorting algorithm click property.
        /// </summary>
        public static readonly BindableProperty SortingAlgorithmProperty = BindableProperty.Create(nameof(SortingAlgorithm),
            typeof(Func<string, IEnumerable<object>, IEnumerable<object>>),
            typeof(AutoCompleteView), _defaultAlgo);

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

        private CancellationTokenSource _cts;
        private readonly ListView _lstSuggestions;
        private readonly TapGestureRecognizer _backgroundTap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutoCompleteView" /> class.
        /// </summary>
        public AutoCompleteView()
        {
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
                ShowHideListbox(false);
                OnSelectedItemChanged(e.SelectedItem);
            };

            ShowHideListbox(false);
            _lstSuggestions.ItemsSource = new List<object>();

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Children.Add(_frameList, 1, 4, 3, 4);
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (child == _frameList)
                SetRow(child, 3);
        }

        public Func<string, IEnumerable<object>, IEnumerable<object>> SortingAlgorithm
        {
            get { return (Func<string, IEnumerable<object>, IEnumerable<object>>)GetValue(SortingAlgorithmProperty); }
            set { SetValue(SortingAlgorithmProperty, value); }
        }

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

        private async Task TextChangedHandler(string text) // async void only for event handlers
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
                    await Task.Run(() =>
                    {
                        var filteredSuggestions = new List<object>();
                        if (!string.IsNullOrEmpty(text) && ItemsSource != null)
                        {
                            filteredSuggestions = SortingAlgorithm(text, ItemsSource.Cast<object>()).ToList();
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _lstSuggestions.ItemsSource = filteredSuggestions;

                                ShowHideListbox(filteredSuggestions.Any());

                                if (filteredSuggestions.Any() && _frameList.GestureRecognizers.Contains(_backgroundTap))
                                {
                                    _backgroundTap.Tapped -= BackGroundTapped;
                                    _frameList.GestureRecognizers.Remove(_backgroundTap);
                                }
                                else if (!filteredSuggestions.Any() && !_frameList.GestureRecognizers.Contains(_backgroundTap))
                                {
                                    _backgroundTap.Tapped += BackGroundTapped;
                                    _frameList.GestureRecognizers.Add(_backgroundTap);
                                }
                            });
                        }
                        else
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _lstSuggestions.ItemsSource = filteredSuggestions;
                            });
                    }, _cts.Token);
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

    public class NaturalSortComparer<T> : IComparer<string>, IDisposable
    {
        private bool isAscending;

        public NaturalSortComparer(bool inAscendingOrder = true)
        {
            this.isAscending = inAscendingOrder;
        }

        #region IComparer<string> Members

        public int Compare(string x, string y)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparer<string> Members

        int IComparer<string>.Compare(string x, string y)
        {
            if (x == y)
                return 0;

            string[] x1, y1;

            if (!table.TryGetValue(x, out x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                table.Add(x, x1);
            }

            if (!table.TryGetValue(y, out y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                table.Add(y, y1);
            }

            int returnVal;

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                {
                    returnVal = PartCompare(x1[i], y1[i]);
                    return isAscending ? returnVal : -returnVal;
                }
            }

            if (y1.Length > x1.Length)
            {
                returnVal = 1;
            }
            else if (x1.Length > y1.Length)
            {
                returnVal = -1;
            }
            else
            {
                returnVal = 0;
            }

            return isAscending ? returnVal : -returnVal;
        }

        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
                return left.CompareTo(right);

            if (!int.TryParse(right, out y))
                return left.CompareTo(right);

            return x.CompareTo(y);
        }

        #endregion

        private Dictionary<string, string[]> table = new Dictionary<string, string[]>();

        public void Dispose()
        {
            table.Clear();
            table = null;
        }
    }
}