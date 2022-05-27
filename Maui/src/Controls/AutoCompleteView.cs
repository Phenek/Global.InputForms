using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

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
                .OrderByDescending(x =>
                    x.ToString().ToLowerInvariant().StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                .ThenBy(x => x.ToString(), new NaturalSortComparer<string>())
                .Take(40).ToList();

        /// <summary>
        ///     The sorting algorithm property.
        /// </summary>
        public static readonly BindableProperty SortingAlgorithmProperty = BindableProperty.Create(
            nameof(SortingAlgorithm),
            typeof(Func<string, IEnumerable<object>, IEnumerable<object>>),
            typeof(AutoCompleteView), _defaultAlgo);

        public static readonly BindableProperty SelectionChangedCommandProperty =
            BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(AutoCompleteView));

        public static readonly BindableProperty SelectionChangedCommandParameterProperty =
            BindableProperty.Create(nameof(SelectionChangedCommandParameter), typeof(ICommand), typeof(AutoCompleteView));

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AutoCompleteView));

        public static readonly BindableProperty ItemDataTemplateProperty =
            BindableProperty.Create(nameof(ItemDataTemplate), typeof(DataTemplate), typeof(AutoCompleteView));

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteView), null);

        private readonly TapGestureRecognizer _backgroundTap;

        private readonly Frame _frameList;
        private readonly CollectionView _collection;

        private CancellationTokenSource _cts;

        public event EventHandler<EventArgs> BackgroundClicked;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutoCompleteView" /> class.
        /// </summary>
        public AutoCompleteView()
        {
            _collection = new CollectionView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                SelectionMode = SelectionMode.Single,
                BackgroundColor = Colors.Transparent
            };

            _collection.SetBinding(ItemsView.ItemTemplateProperty,
                new Binding(nameof(ItemDataTemplate)) { Source = this, Mode = BindingMode.OneWay });

            _collection.SetBinding(SelectableItemsView.SelectionChangedCommandProperty,
                new Binding(nameof(SelectionChangedCommand)) { Source = this, Mode = BindingMode.OneWay });

            _collection.SetBinding(SelectableItemsView.SelectionChangedCommandParameterProperty,
                new Binding(nameof(SelectionChangedCommandParameter)) { Source = this, Mode = BindingMode.OneWay });

            _frameList = new Frame
            {
                Padding = 0,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HasShadow = false,
                IsClippedToBounds = true,
                BackgroundColor = Colors.Transparent,
                Content = _collection
            };

            _backgroundTap = new TapGestureRecognizer();
            _backgroundTap.Tapped += BackGroundTapped;
            _frameList.GestureRecognizers.Add(_backgroundTap);

            TextChanged += (sender, e) => { _ = TextChangedHandler(e.NewTextValue); };

            _collection.SelectionChanged += SelectionItemChanged;

            ShowCollection(false);
            _collection.ItemsSource = new List<object>();

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Grid.SetRow(_frameList, 1);
            Grid.SetRowSpan(_frameList, 3);
            Grid.SetColumn(_frameList, 3);
            Grid.SetColumnSpan(_frameList, 1);
            Children.Add(_frameList); //Todo overload (1, 4, 3, 4)
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged
        {
            add => _collection.SelectionChanged += value;
            remove => _collection.SelectionChanged -= value;
        }

        private void SelectionItemChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = e.CurrentSelection.FirstOrDefault();
            EntryText = SelectedItem.ToString();
            ShowCollection(false);
        }

        public Func<string, IEnumerable<object>, IEnumerable<object>> SortingAlgorithm
        {
            get => (Func<string, IEnumerable<object>, IEnumerable<object>>)GetValue(SortingAlgorithmProperty);
            set => SetValue(SortingAlgorithmProperty, value);
        }

        public ICommand SelectionChangedCommand
        {
            get => (ICommand)GetValue(SelectionChangedCommandProperty);
            set => SetValue(SelectionChangedCommandProperty, value);
        }

        public object SelectionChangedCommandParameter
        {
            get => GetValue(SelectionChangedCommandParameterProperty);
            set => SetValue(SelectionChangedCommandParameterProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public DataTemplate ItemDataTemplate
        {
            get => (DataTemplate)GetValue(ItemDataTemplateProperty);
            set => SetValue(ItemDataTemplateProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            if (child == _frameList)
                SetRow((BindableObject)child, 3);
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
                                _collection.ItemsSource = filteredSuggestions;

                                ShowCollection(filteredSuggestions.Any());

                                if (filteredSuggestions.Any() && _frameList.GestureRecognizers.Contains(_backgroundTap))
                                {
                                    _backgroundTap.Tapped -= BackGroundTapped;
                                    _frameList.GestureRecognizers.Remove(_backgroundTap);
                                }
                                else if (!filteredSuggestions.Any() &&
                                            !_frameList.GestureRecognizers.Contains(_backgroundTap))
                                {
                                    _backgroundTap.Tapped += BackGroundTapped;
                                    _frameList.GestureRecognizers.Add(_backgroundTap);
                                }
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _collection.ItemsSource = filteredSuggestions;
                            });
                        }
                    }, _cts.Token);
                }
                catch (TaskCanceledException) // if the operation is cancelled, do nothing
                {
                }
            }
        }

        private void ShowCollection(bool show)
        {
            _collection.IsVisible = show;
        }

        private void BackGroundTapped(object sender, EventArgs e)
        {
            BackgroundClicked?.Invoke(sender, e);
        }
    }

    public class NaturalSortComparer<T> : IComparer<string>, IDisposable
    {
        private readonly bool isAscending;

        private Dictionary<string, string[]> table = new Dictionary<string, string[]>();

        public NaturalSortComparer(bool inAscendingOrder = true)
        {
            isAscending = inAscendingOrder;
        }

        public void Dispose()
        {
            table.Clear();
            table = null;
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

            for (var i = 0; i < x1.Length && i < y1.Length; i++)
                if (x1[i] != y1[i])
                {
                    returnVal = PartCompare(x1[i], y1[i]);
                    return isAscending ? returnVal : -returnVal;
                }

            if (y1.Length > x1.Length)
                returnVal = 1;
            else if (x1.Length > y1.Length)
                returnVal = -1;
            else
                returnVal = 0;

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
    }
}