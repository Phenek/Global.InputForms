using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Global.InputForms.Extentions;
using Global.InputForms.Interfaces;
using Global.InputForms.Models;
using Microsoft.Maui.Controls;

namespace Global.InputForms
{
    public class CheckGroup : StackLayout
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
        ///     The Icheckable List
        /// </summary>
        public ObservableCollection<Global.InputForms.Interfaces.ICheckable> CheckList;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckGroup" /> class.
        /// </summary>
        public CheckGroup()
        {
            CheckList = new ObservableCollection<Global.InputForms.Interfaces.ICheckable>();
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

        public event EventHandler<bool> CheckedChanged;
        public event EventHandler<IEnumerable> CheckedCollectionChanged;

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent is FrameInfo frameInfo) CheckedChanged += (sender, e) => frameInfo.Validate();
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is Global.InputForms.Interfaces.ICheckable checkable)
            {
                checkable.Clicked += OnCheckedChanged;
                checkable.Index = CheckList.Count;
                CheckList.Add(checkable);

                if (ItemsSource == null) return;

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
                Console.WriteLine("{CheckGroup}: Element does not implement interface Icheckable");
                throw new Exception("{CheckGroup}: Element does not implement interface Icheckable");
            }
        }

        protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        {
            base.OnChildRemoved(child, oldLogicalIndex);
            if (!(child is Global.InputForms.Interfaces.ICheckable checkable)) return;
            checkable.Clicked -= OnCheckedChanged;
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
                Console.WriteLine("{CheckGroup}: Each elements must have a unique Key/Value!");
                throw new Exception("{CheckGroup}: Each elements must have a unique Key/Value!");
            }

            if (e.OldItems != null)
            {
                var index = e.OldStartingIndex;
                foreach (var item in e.OldItems)
                {
                    string key = item is KeyValuePair<string, object> kvp ? kvp.Key : index++.ToString();
                    var view = default(Global.InputForms.Interfaces.ICheckable);
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
                        if (item is KeyValuePair<string, object> p)
                            AddItemToView(p);
                        else
                            AddItemToView(new KeyValuePair<string, object>(index++.ToString(), item));
                    
                }
            }
        }

        private void AddItemToView(KeyValuePair<string, object> item)
        {
            if (!(GenerateCheckableView(null) is View view)) return;

            ((Global.InputForms.Interfaces.ICheckable) view).Item = item;
            Children.Add(view);
        }

        private void GenerateChekableList()
        {
            foreach (var item in CheckList) item.Clicked -= OnCheckedChanged;
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
        }

        public View GenerateCheckableView(object context)
        {
            if (!(CheckTemplate is ControlTemplate template)) return null;

            var temp = template.CreateContent();
            if (temp is View view && view is Global.InputForms.Interfaces.ICheckable)
            {
                if (view != context) view.BindingContext = context;
                return view;
            }

            Console.WriteLine("{CheckGroup}: CheckTemplate must implement interface ICheckable");
            throw new Exception("{CheckGroup}: CheckTemplate must implement interface ICheckable");
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CheckGroup checkGroup)) return;

            if (oldValue is INotifyCollectionChanged oldSource)
                oldSource.CollectionChanged -= checkGroup.CollectionChanged;
            checkGroup.GenerateChekableList();
            if (newValue is INotifyCollectionChanged newSource)
                newSource.CollectionChanged += checkGroup.CollectionChanged;
        }

        private static void CheckTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckGroup checkGroup)
            {
                //CheckGroup.GenerateChekableList();
            }
        }

        /// <summary>
        /// The StackLayout Spacing changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CheckGroup checkGroup)) return;

            checkGroup.Spacing = (double)newValue;
            var index = 0;
            foreach (var item in checkGroup.Children)
                if (item is CheckContent checkBox)
                    checkBox.Padding = checkGroup.SetSpacingPadding(index++);
                else
                    ((View)item).Margin = checkGroup.SetSpacingPadding(index++);
        }

        public IEnumerable GetCheckedItems()
        {
            if (ItemsSource is IDictionary)
            {
                var result = new Dictionary<string, object>();
                for (var i = 0; i < CheckList.Count; ++i)
                    if (CheckList[i].Checked)
                        result.Add(CheckList[i].Key, CheckList[i].Value);
                return result;
            }
            else
            {
                var result = new List<object>();
                for (var i = 0; i < CheckList.Count; ++i)
                    if (CheckList[i].Checked)
                        result.Add(CheckList[i].Value);
                return result;
            }
        }

        public IEnumerable GetUnCheckedItems()
        {
            if (ItemsSource is IDictionary)
            {
                var result = new Dictionary<string, object>();
                for (var i = 0; i < CheckList.Count; ++i)
                    if (!CheckList[i].Checked)
                        result.Add(CheckList[i].Key, CheckList[i].Value);
                return result;
            }
            else
            {
                var result = new List<object>();
                for (var i = 0; i < CheckList.Count; ++i)
                    if (!CheckList[i].Checked)
                        result.Add(CheckList[i].Value);
                return result;
            }
        }

        private void OnCheckedChanged(object sender, bool check)
        {
            if (!(sender is Global.InputForms.Interfaces.ICheckable checkBox)) return;

            CheckedChanged?.Invoke(sender, check);
            CheckedCollectionChanged?.Invoke(this, GetCheckedItems());
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
