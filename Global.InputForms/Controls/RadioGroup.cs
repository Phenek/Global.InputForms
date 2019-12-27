using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Global.InputForms.Extentions;
using Global.InputForms.Interfaces;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class RadioGroup : StackLayout
    {
        /// <summary>
        ///     The Items Source property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(CheckGroup), null, propertyChanged: ItemsSourceChanged);

        /// <summary>
        ///     Icon Template Property.
        /// </summary>
        private static readonly BindableProperty CheckTemplateProperty = BindableProperty.Create(nameof(CheckTemplate),
            typeof(ControlTemplate), typeof(CheckGroup), null, propertyChanged: CheckTemplateChanged);

        /// <summary>
        /// The Spacing property.
        /// </summary>
        public static readonly new BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(CheckGroup), 10.0, propertyChanged: SpacingChanged);

        /// <summary>
        ///     The deselectable property.
        /// </summary>
        public static readonly BindableProperty IsDeselectableProperty =
            BindableProperty.Create(nameof(IsDeselectable), typeof(bool), typeof(RadioGroup), false);

        /// <summary>
        ///     The Default Index property.
        /// </summary>
        public static readonly BindableProperty DefaultIndexProperty = BindableProperty.Create(nameof(DefaultIndex),
            typeof(int), typeof(RadioGroup), -1, propertyChanged: DefaultIndexChanged);

        /// <summary>
        ///     The Selected Index property.
        /// </summary>
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex),
            typeof(int), typeof(RadioGroup), -1, propertyChanged: OnSelectedIndexChanged);

        /// <summary>
        ///     The Selected Item property.
        /// </summary>
        public static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
            typeof(object), typeof(RadioGroup), null,
            propertyChanged: OnSelectedItemChanged);

        /// <summary>
        ///     The items
        /// </summary>
        public ObservableCollection<ICheckable> CheckList;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RadioGroup" /> class.
        /// </summary>
        public RadioGroup()
        {
            CheckList = new ObservableCollection<ICheckable>();
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     Gets or Set the Icon template.
        /// </summary>
        /// <value>The Icon template.</value>
        public ControlTemplate CheckTemplate
        {
            get => GetValue(CheckTemplateProperty) as ControlTemplate;
            set => SetValue(CheckTemplateProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Rate group selected index.
        /// </summary>
        /// <value>The Rate group selected index.</value>
        public int DefaultIndex
        {
            get => (int) GetValue(DefaultIndexProperty);
            set => SetValue(DefaultIndexProperty, value);
        }

        /// <summary>
        ///     Gets or sets deselectable value.
        /// </summary>
        /// <value>The deselectable value.</value>
        public bool IsDeselectable
        {
            get => (bool) GetValue(IsDeselectableProperty);
            set => SetValue(IsDeselectableProperty, value);
        }

        /// <summary>
        ///     Gets or sets the radio group selected index.
        /// </summary>
        /// <value>The radio group selected index.</value>
        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        /// <summary>
        ///     Gets or sets the radio group selected index.
        /// </summary>
        /// <value>The radio group selected index.</value>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            private set => SetValue(SelectedItemProperty, value);
        }

        public event EventHandler<int> SelectedIndexChanged;
        public event EventHandler<object> SelectedItemChanged;

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent is FrameInfo frameInfo) SelectedItemChanged += (sender, e) => frameInfo.Validate();
        }

        private void GenerateChekableList()
        {
            foreach (var item in CheckList) item.Clicked -= OnItemClicked;
            Children.Clear();
            CheckList.Clear();

            if (ItemsSource == null) return;

            var index = 0;
            foreach (var item in ItemsSource)
            {
                if (item is KeyValuePair<string, object> kvp)
                    AddItemToView(kvp);
                else
                    AddItemToView(new KeyValuePair<string, object>(index++.ToString(), item));
            }

            if (CheckList.Any() && DefaultIndex >= 0) CheckList[DefaultIndex].Checked = true;
        }

        public View GenerateCheckableView(object context)
        {
            if (!(CheckTemplate is ControlTemplate template)) return null;

            var temp = template.CreateContent();
            if (temp is View view && view is ICheckable)
            {
                if (view != context) view.BindingContext = context;
                return view;
            }

            Console.WriteLine("{RadioGroup}: CheckTemplate must implement interface ICheckable");
            throw new Exception("{RadioGroup}: CheckTemplate must implement interface ICheckable");
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is ICheckable checkable)
            {
                checkable.Clicked += OnItemClicked;
                checkable.Index = CheckList.Count;
                CheckList.Add(checkable);

                if (ItemsSource == null) return;
                if (DefaultIndex == checkable.Index) checkable.Checked = true;

                if (ItemsSource is IDictionary<object, object> dic && !dic.ContainsKey(checkable.Key))
                    dic.Add(checkable.Key, checkable.Value);
                else if (ItemsSource is IList list && !list.Contains(checkable.Value))
                {
                    list.Add(checkable.Value);
                    var index = 0;
                    foreach (var check in CheckList)
                        check.Key = index++.ToString();
                }
            }
            else
            {
                Console.WriteLine("{RadioGroup}: Element does not implement interface Icheckable");
                throw new Exception("{RadioGroup}: Element does not implement interface Icheckable");
            }
        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);

            if (!(child is ICheckable checkable)) return;

            checkable.Clicked -= OnItemClicked;
            CheckList.Remove(checkable);
            if (ItemsSource is IDictionary dic && dic.Contains(checkable.Key))
                dic.Remove(checkable.Key);
            else if (ItemsSource is IList list)
            {
                if (list.Contains(checkable.Value))
                    list.Remove(checkable.Value);
                var i = 0;
                foreach (var check in CheckList)
                    check.Key = i++.ToString();
            }

            var index = 0;
            foreach (var check in CheckList) check.Index = index++;
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemsSource is IList list && list.ContainsDuplicates())
            {
                Console.WriteLine("{RadioGroup}: Each elements must have a unique Key/Value!");
                throw new Exception("{RadioGroup}: Each elements must have a unique Key/Value!");
            }

            if (e.OldItems != null)
            {
                var index = e.OldStartingIndex;
                foreach (var item in e.OldItems)
                {
                    string key = item is KeyValuePair<string, object> kvp ? kvp.Key : index++.ToString();
                    var view = default(ICheckable);
                    foreach (var checkable in CheckList)
                        if (checkable.Item.Key == key)
                            view = checkable;
                    if (view != null) Children.Remove((View)view);
                }
            }

            if (e.NewItems != null)
            {
                var index = e.NewStartingIndex;
                foreach (var item in e.NewItems)
                {
                    string key = item is KeyValuePair<string, object> kvp ? kvp.Key : index++.ToString();
                    if (CheckList.All(c => c.Key != key))
                        if (item is KeyValuePair<string, object>)
                            AddItemToView(kvp);
                        else
                            AddItemToView(new KeyValuePair<string, object>(index++.ToString(), item));
                }
            }
        }

        private void AddItemToView(KeyValuePair<string, object> item)
        {
            if (!(GenerateCheckableView(null) is View view)) return;

            ((ICheckable) view).Item = item;
            Children.Add(view);
        }

        /// <summary>
        ///     The radio group Items Source changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RadioGroup radioGroup) || radioGroup.ItemsSource == null) return;

            if (oldValue is INotifyCollectionChanged oldSource)
                oldSource.CollectionChanged -= radioGroup.CollectionChanged;
            radioGroup.GenerateChekableList();
            if (newValue is INotifyCollectionChanged newSource)
                newSource.CollectionChanged += radioGroup.CollectionChanged;
        }

        private static void CheckTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadioGroup radioGroup && radioGroup.ItemsSource != null)
            {
                //RadioGroup.GenerateChekableList();
            }
        }

        /// <summary>
        ///     The Rate group default index changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void DefaultIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadioGroup radioGroup)
                radioGroup.SelectedIndex = radioGroup.DefaultIndex;
        }

        /// <summary>
        ///     The radio group selected index changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RadioGroup radioGroup)) return;

            radioGroup.SelectedIndexChanged?.Invoke(radioGroup, (int) newValue);

            if ((int) newValue < 0)
            {
                foreach (var button in radioGroup.CheckList) button.Checked = false;
                radioGroup.SelectedItem = null;
                return;
            }

            foreach (var button in radioGroup.CheckList)
                if (button.Index == radioGroup.SelectedIndex)
                {
                    radioGroup.SelectedItem = radioGroup.ItemsSource is IDictionary ? button.Item : button.Item.Value;
                    button.Checked = true;
                }
                else
                {
                    button.Checked = false;
                }
        }

        /// <summary>
        ///     The radio group selected index changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadioGroup radioGroup)
                radioGroup.SelectedItemChanged?.Invoke(radioGroup, newValue);
        }

        /// <summary>
        /// The StackLayout Spacing changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RadioGroup radioGroup)) return;

            radioGroup.Spacing = (double)newValue;
            var index = 0;
            foreach (var item in radioGroup.Children)
                if (item is CheckContent checkBox)
                    checkBox.Padding = radioGroup.SetSpacingPadding(index++);
                else
                    item.Margin = radioGroup.SetSpacingPadding(index++);
        }

        private void OnItemClicked(object sender, bool check)
        {
            if (!(sender is ICheckable selected)) return;

            if (check == false)
            {
                if (IsDeselectable)
                {
                    SelectedItem = null;
                    SelectedIndex = -1;
                }
                else
                {
                    selected.Checked = true;
                }

                return;
            }

            foreach (var item in CheckList)
                if (selected.Index.Equals(item.Index))
                {
                    SelectedItem = ItemsSource is IDictionary ? selected.Item : selected.Item.Value;
                    SelectedIndex = selected.Index;
                }
                else
                {
                    item.Checked = false;
                }
        }

        public bool Validate()
        {
            if (!(Parent is FrameInfo frameInfo)) return true;

            frameInfo.Info = false;
            frameInfo.Validators?.Invoke(this, new EventArgs());
            return !frameInfo.Info;
        }

        private Thickness SetSpacingPadding(int index)
        {
            if (index == 0)
                return Orientation == StackOrientation.Horizontal
                    ? new Thickness(0, 0, Spacing / 2, 0)
                    : new Thickness(0, 0, 0, Spacing / 2);
            if (index == Children.Count - 1)
                return Orientation == StackOrientation.Horizontal
                    ? new Thickness(Spacing / 2, 0, 0, 0)
                    : new Thickness(0, Spacing / 2, 0, 0);
            return Orientation == StackOrientation.Horizontal
                ? new Thickness(Spacing / 2, 0, Spacing / 2, 0)
                : new Thickness(0, Spacing / 2, 0, Spacing / 2);
        }
    }
}