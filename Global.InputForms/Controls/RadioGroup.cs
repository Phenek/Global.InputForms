using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Global.InputForms.Interfaces;
using Global.InputForms.Models;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class RadioGroup : StackLayout
    {
        /// <summary>
        ///     The Items Source property.
        /// </summary>
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IDictionary<string, string>), typeof(RadioGroup), null, propertyChanged: OnItemsSourceChanged);

        /// <summary>
        ///     Icon Template Property.
        /// </summary>
        private static readonly BindableProperty CheckTemplateProperty = BindableProperty.Create(nameof(CheckTemplate),
            typeof(ControlTemplate), typeof(CheckGroup), null, propertyChanged: CheckTemplateChanged);

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
            typeof(KeyValuePair<string, string>), typeof(RadioGroup), new KeyValuePair<string, string>(null, null),
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
            ItemsSource = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Gets or sets the radio group Items Source.
        /// </summary>
        /// <value>The radio group Items Source.</value>
        public IDictionary<string, string> ItemsSource
        {
            get => (IDictionary<string, string>) GetValue(ItemsSourceProperty);
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
        public KeyValuePair<string, string> SelectedItem
        {
            get => (KeyValuePair<string, string>) GetValue(SelectedItemProperty);
            private set => SetValue(SelectedItemProperty, value);
        }

        public event EventHandler<int> SelectedIndexChanged;
        public event EventHandler<KeyValuePair<string, string>> SelectedItemChanged;

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

            foreach (var item in ItemsSource) AddItemToView(item);

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
                if (!string.IsNullOrEmpty(checkable.Key) && CheckList.All(c => c.Key != checkable.Key))
                {
                    checkable.Clicked += OnItemClicked;
                    checkable.Index = CheckList.Count;
                    CheckList.Add(checkable);
                    if (!ItemsSource.ContainsKey(checkable.Key)) ItemsSource.Add(checkable.Key, checkable.Value);

                    if (DefaultIndex == checkable.Index) checkable.Checked = true;
                }
                else
                {
                    Console.WriteLine("{RadioGroup}: Each elements must have a unique Key!");
                    throw new Exception("{RadioGroup}: Each elements must have a unique Key!");
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
            ItemsSource.Remove(checkable.Key);

            var index = 0;
            foreach (var check in CheckList) check.Index = index++;
        }

        private void ItemSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (KeyValuePair<string, string> item in e.NewItems)
                    if (CheckList.All(c => c.Key != item.Key))
                        AddItemToView(item);
            if (e.OldItems != null)
                foreach (KeyValuePair<string, string> item in e.OldItems)
                {
                    var view = default(ICheckable);
                    foreach (var checkable in CheckList)
                        if (checkable.Item.Key == item.Key)
                            view = checkable;
                    if (view != null) Children.Remove((View) view);
                }
        }

        private void AddItemToView(KeyValuePair<string, string> item)
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
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RadioGroup radioGroup) || radioGroup.ItemsSource == null) return;

            if (oldValue is INotifyCollectionChanged oldSource)
                oldSource.CollectionChanged -= radioGroup.ItemSource_CollectionChanged;
            radioGroup.GenerateChekableList();
            if (newValue is INotifyCollectionChanged newSource)
                newSource.CollectionChanged += radioGroup.ItemSource_CollectionChanged;
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
                radioGroup.SelectedItem = new KeyValuePair<string, string>(null, null);
                return;
            }

            foreach (var button in radioGroup.CheckList)
                if (button.Index == radioGroup.SelectedIndex)
                {
                    radioGroup.SelectedItem = button.Item;
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
                radioGroup.SelectedItemChanged?.Invoke(radioGroup, (KeyValuePair<string, string>) newValue);
        }

        private void OnItemClicked(object sender, bool check)
        {
            if (!(sender is ICheckable selected)) return;
            
            if (check == false)
            {
                if (IsDeselectable)
                {
                    SelectedItem = new KeyValuePair<string, string>(null, null);
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
                    SelectedItem = selected.Item;
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
    }
}