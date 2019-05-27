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
    public class CheckGroup : StackLayout
    {
        /// <summary>
        ///     The Items Source property.
        /// </summary>
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IDictionary<string, string>), typeof(CheckGroup), null, propertyChanged: OnItemsSourceChanged);

        /// <summary>
        ///     Icon Template Property.
        /// </summary>
        private static readonly BindableProperty CheckTemplateProperty = BindableProperty.Create(nameof(CheckTemplate),
            typeof(ControlTemplate), typeof(CheckGroup), null, propertyChanged: CheckTemplateChanged);

        /*
        /// <summary>
        /// The Orientation property.
        /// </summary>
        public static new BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(CheckGroup), StackOrientation.Vertical, propertyChanged: OrientationChanged);

        /// <summary>
        /// The Spacing property.
        /// </summary>
        public static readonly new BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(CheckGroup), 10.0, propertyChanged: SpacingChanged);
        */

        /// <summary>
        ///     The Icheckable List
        /// </summary>
        public ObservableCollection<ICheckable> CheckList;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckGroup" /> class.
        /// </summary>
        public CheckGroup()
        {
            CheckList = new ObservableCollection<ICheckable>();
            ItemsSource = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Gets or sets the Check group Items Source.
        /// </summary>
        /// <value>The Check group Items Source.</value>
        public IDictionary<string, string> ItemsSource
        {
            get => (IDictionary<string, string>) GetValue(ItemsSourceProperty);
            set
            {
                if (value != null) SetValue(ItemsSourceProperty, value);
            }
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
        public event EventHandler<Dictionary<string, string>> CheckedCollectionChanged;

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent is FrameInfo frameInfo) CheckedChanged += (sender, e) => frameInfo.Validate();
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is ICheckable checkable)
            {
                if (!string.IsNullOrEmpty(checkable.Key) && CheckList.All(c => c.Key != checkable.Key))
                {
                    checkable.Clicked += OnCheckedChanged;
                    checkable.Index = CheckList.Count;
                    CheckList.Add(checkable);
                    if (!ItemsSource.ContainsKey(checkable.Key)) ItemsSource.Add(checkable.Key, checkable.Value);
                    /*
                    //Spacing
                    if (checkable is CheckBox checkBox)
                    {
                        checkBox.Padding = SetSpacingPadding(checkable.Index);
                    }
                    else
                    {
                        ((View)checkable).Margin = SetSpacingPadding(checkable.Index);
                    }
                    */
                }
                else
                {
                    Console.WriteLine("{CheckGroup}: Each elements must have a unique Key!");
                    throw new Exception("{CheckGroup}: Each elements must have a unique Key!");
                }
            }
            else
            {
                Console.WriteLine("{CheckGroup}: Element does not implement interface Icheckable");
                throw new Exception("{CheckGroup}: Element does not implement interface Icheckable");
            }
        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);
            if (!(child is ICheckable checkable)) return;
            checkable.Clicked -= OnCheckedChanged;
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

        private void GenerateChekableList()
        {
            foreach (var item in CheckList) item.Clicked -= OnCheckedChanged;
            Children.Clear();
            CheckList.Clear();

            foreach (var item in ItemsSource) AddItemToView(item);
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

            Console.WriteLine("{CheckGroup}: CheckTemplate must implement interface ICheckable");
            throw new Exception("{CheckGroup}: CheckTemplate must implement interface ICheckable");

        }

        /// <summary>
        ///     The Check group Items Source changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CheckGroup checkGroup) || checkGroup.ItemsSource == null) return;

            if (oldValue is INotifyCollectionChanged oldSource)
                oldSource.CollectionChanged -= checkGroup.ItemSource_CollectionChanged;
            checkGroup.GenerateChekableList();
            if (newValue is INotifyCollectionChanged newSource)
                newSource.CollectionChanged += checkGroup.ItemSource_CollectionChanged;
        }

        private static void CheckTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckGroup checkGroup && checkGroup.ItemsSource != null)
            {
                //CheckGroup.GenerateChekableList();
            }
        }

        public Dictionary<string, string> GetCheckedDictionary()
        {
            var result = new Dictionary<string, string>();
            for (var i = 0; i < CheckList.Count; ++i)
                if (CheckList[i].Checked)
                    result.Add(CheckList[i].Key, CheckList[i].Value);
            return result;
        }

        public Dictionary<string, string> GetUnCheckedDictionary()
        {
            var result = new Dictionary<string, string>();
            for (var i = 0; i < CheckList.Count; ++i)
                if (!CheckList[i].Checked)
                    result.Add(CheckList[i].Key, CheckList[i].Value);
            return result;
        }

        private void OnCheckedChanged(object sender, bool check)
        {
            if (!(sender is ICheckable checkBox)) return;
            
            CheckedChanged?.Invoke(sender, check);
            CheckedCollectionChanged?.Invoke(this, GetCheckedDictionary());
        }

        public bool Validate()
        {
            if (!(Parent is FrameInfo frameInfo)) return true;
            
            frameInfo.Info = false;
            frameInfo.Validators?.Invoke(this, new EventArgs());
            return !frameInfo.Info;

        }

        /*
        private Thickness SetSpacingPadding(int index)
        {
            if (index == 0)
            {
                return (Orientation == StackOrientation.Horizontal)
                    ? new Thickness(0, 0, Spacing / 2, 0)
                    : new Thickness(0, 0, 0, Spacing / 2);
            }
            else if (index == ItemsSource.Count - 1)
            {
                return (Orientation == StackOrientation.Horizontal)
                    ? new Thickness(Spacing / 2, 0, 0, 0)
                    : new Thickness(0, Spacing / 2, 0, 0);
            }
            else
            {
                return (Orientation == StackOrientation.Horizontal)
                    ? new Thickness(Spacing / 2, 0, Spacing / 2, 0)
                    : new Thickness(0, Spacing / 2, 0, Spacing / 2);
            }
        }
        */
    }
}