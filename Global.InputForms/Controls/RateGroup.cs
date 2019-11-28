using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Global.InputForms.Interfaces;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class RateGroup : StackLayout
    {
        /// <summary>
        ///     The Items Source property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IDictionary<string, object>), typeof(RateGroup), null, propertyChanged: OnItemsSourceChanged);

        /// <summary>
        ///     Icon Template Property.
        /// </summary>
        private static readonly BindableProperty CheckTemplateProperty = BindableProperty.Create(nameof(CheckTemplate),
            typeof(ControlTemplate), typeof(CheckGroup), null, propertyChanged: CheckTemplateChanged);

        /// <summary>
        ///     The Default Index property.
        /// </summary>
        public static readonly BindableProperty DefaultIndexProperty = BindableProperty.Create(nameof(DefaultIndex),
            typeof(int), typeof(RateGroup), -1, propertyChanged: DefaultIndexChanged);

        /// <summary>
        ///     The Selected Index property.
        /// </summary>
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex),
            typeof(int), typeof(RateGroup), -1, propertyChanged: OnSelectedIndexChanged);

        /// <summary>
        ///     The Selected Item property.
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
            typeof(KeyValuePair<string, object>), typeof(RateGroup), new KeyValuePair<string, object>(null, null),
            propertyChanged: OnSelectedItemChanged);

        /// <summary>
        ///     The Orientation property.
        /// </summary>
        public new static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation),
            typeof(StackOrientation), typeof(RateGroup), StackOrientation.Horizontal,
            propertyChanged: OrientationChanged);

        /// <summary>
        ///     The Spacing property.
        /// </summary>
        public new static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing),
            typeof(double), typeof(RateGroup), 10.0, propertyChanged: SpacingChanged);

        /// <summary>
        ///     The Label Font Attributes property.
        /// </summary>
        public static readonly BindableProperty LabelFontAttributesProperty =
            BindableProperty.Create(nameof(LabelFontAttributes), typeof(FontAttributes), typeof(RateGroup),
                FontAttributes.None, propertyChanged: LabelFontAttributesChanged);

        /// <summary>
        ///     The Label Font Family property.
        /// </summary>
        public static readonly BindableProperty LabelFontFamilyProperty =
            BindableProperty.Create(nameof(LabelFontFamily), typeof(string), typeof(RateGroup), string.Empty,
                propertyChanged: LabelFontFamilyChanged);

        /// <summary>
        ///     The Label Font Size property.
        /// </summary>
        public static readonly BindableProperty LabelFontSizeProperty = BindableProperty.Create(nameof(LabelFontSize),
            typeof(double), typeof(RateGroup), Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
            propertyChanged: LabelFontSizeChanged);

        /// <summary>
        ///     The Label Horizontal Text Alignment property.
        /// </summary>
        public static readonly BindableProperty LabelHorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelHorizontalTextAlignment), typeof(TextAlignment), typeof(RateGroup),
                TextAlignment.Center, propertyChanged: LabelHorizontalTextAlignmentChanged);

        /// <summary>
        ///     The Label Vertical Text Alignment property.
        /// </summary>
        public static readonly BindableProperty LabelVerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(LabelVerticalTextAlignment), typeof(TextAlignment), typeof(RateGroup),
                TextAlignment.Center, propertyChanged: LabelVerticalTextAlignmentChanged);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public static readonly BindableProperty LabelTextColorProperty = BindableProperty.Create(nameof(LabelTextColor),
            typeof(Color), typeof(RateGroup), Color.Black, propertyChanged: LabelTextColorChanged);

        /// <summary>
        ///     The Label is Visible property.
        /// </summary>
        public static readonly BindableProperty LabelIsVisibleProperty = BindableProperty.Create(nameof(LabelIsVisible),
            typeof(bool), typeof(RateGroup), true, propertyChanged: LabelIsVisibleChanged);

        private readonly Label _rateLabel;
        private readonly StackLayout _rateLayout;
        private bool _isBusy;

        /// <summary>
        ///     The items
        /// </summary>
        public ObservableCollection<ICheckable> CheckList;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RateGroup" /> class.
        /// </summary>
        public RateGroup()
        {
            base.Orientation = StackOrientation.Vertical;
            base.Spacing = 0;
            Padding = 0;

            _rateLayout = new StackLayout
            {
                Orientation = Orientation,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0
            };

            _rateLabel = new Label
            {
                Text = "-",
                FontAttributes = LabelFontAttributes,
                FontFamily = LabelFontFamily,
                FontSize = LabelFontSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = LabelHorizontalTextAlignment,
                VerticalTextAlignment = LabelVerticalTextAlignment,
                TextColor = LabelTextColor
            };

            base.Children.Add(_rateLabel);
            base.Children.Add(_rateLayout);

            CheckList = new ObservableCollection<ICheckable>();
            ItemsSource = new Dictionary<string, object>();

            _rateLayout.ChildAdded += ChildCheckAdded;
            _rateLayout.ChildRemoved += ChildCheckRemoved;

            /*
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);
            */
        }

        public new IList<View> Children => _rateLayout.Children;

        /// <summary>
        ///     Gets or sets the Rate group Items Source.
        /// </summary>
        /// <value>The Rate group Items Source.</value>
        public IDictionary<string, object> ItemsSource
        {
            get => (IDictionary<string, object>)GetValue(ItemsSourceProperty);
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
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set
            {
                if (!_isBusy) SetValue(SelectedIndexProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Rate group selected index.
        /// </summary>
        /// <value>The Rate group selected index.</value>
        public KeyValuePair<string, object> SelectedItem
        {
            get => (KeyValuePair<string, object>)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Rate group selected index.
        /// </summary>
        /// <value>The Rate group selected index.</value>
        public int DefaultIndex
        {
            get => (int)GetValue(DefaultIndexProperty);
            set => SetValue(DefaultIndexProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Rate group orientation.
        /// </summary>
        /// <value>The Orientation of Rate buttons.</value>
        public new StackOrientation Orientation
        {
            get => (StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Check group Spacing.
        /// </summary>
        /// <value>The Spacing of Check buttons.</value>
        public new double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font attributes.
        /// </summary>
        /// <value>The label font attributes.</value>
        public FontAttributes LabelFontAttributes
        {
            get => (FontAttributes)GetValue(LabelFontAttributesProperty);
            set => SetValue(LabelFontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font family.
        /// </summary>
        /// <value>The label font family.</value>
        public string LabelFontFamily
        {
            get => (string)GetValue(LabelFontFamilyProperty);
            set => SetValue(LabelFontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label font size.
        /// </summary>
        /// <value>The label font size.</value>
        public double LabelFontSize
        {
            get => (double)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label horizontal text alignment.
        /// </summary>
        /// <value>The label horizontal text alignment.</value>
        public TextAlignment LabelHorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(LabelHorizontalTextAlignmentProperty);
            set => SetValue(LabelHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label vertical text alignment.
        /// </summary>
        /// <value>The label vertical text alignment.</value>
        public TextAlignment LabelVerticalTextAlignment
        {
            get => (TextAlignment)GetValue(LabelVerticalTextAlignmentProperty);
            set => SetValue(LabelVerticalTextAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public Color LabelTextColor
        {
            get => (Color)GetValue(LabelTextColorProperty);
            set => SetValue(LabelTextColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the label is Visible property.
        /// </summary>
        /// <value>The label is Visible.</value>
        public bool LabelIsVisible
        {
            get => (bool)GetValue(LabelIsVisibleProperty);
            set => SetValue(LabelIsVisibleProperty, value);
        }

        public event EventHandler<int> SelectedIndexChanged;
        public event EventHandler<KeyValuePair<string, object>> SelectedItemChanged;

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

            // Check the default index & set the label
            if (CheckList.Any() && DefaultIndex >= 0)
            {
                if (CheckList[DefaultIndex].Item.Value is string str)
                    _rateLabel.Text = str;
                CheckList[DefaultIndex].Checked = true;
            }
            else
            {
                _rateLabel.Text = "-";
            }
        }

        public View GenerateCheckableView(object context)
        {
            if (!(CheckTemplate is ControlTemplate template)) return null;

            var temp = template.CreateContent();
            if (temp is CheckContent view)
            {
                if (view != context) view.BindingContext = context;
                return view;
            }

            Console.WriteLine("{RateGroup}: CheckTemplate must implement interface ICheckable");
            throw new Exception("{RateGroup}: CheckTemplate must implement interface ICheckable");
        }

        private void ChildCheckAdded(object sender, ElementEventArgs e)
        {
            if (e.Element is ICheckable checkable)
            {
                if (!string.IsNullOrEmpty(checkable.Key) && CheckList.All(c => c.Key != checkable.Key))
                {
                    checkable.DisableCheckOnClick = true;
                    checkable.Clicked += OnItemClicked;
                    checkable.Index = CheckList.Count;
                    CheckList.Add(checkable);
                    if (!ItemsSource.ContainsKey(checkable.Key)) ItemsSource.Add(checkable.Key, checkable.Value);

                    if (DefaultIndex == checkable.Index) checkable.Checked = true;
                }
                else
                {
                    Console.WriteLine("{RateGroup}: Each elements must have a unique Key!");
                    throw new Exception("{RateGroup}: Each elements must have a unique Key!");
                }
            }
            else
            {
                Console.WriteLine("{RateGroup}: Element does not implement interface Icheckable");
                throw new Exception("{RateGroup}: Element does not implement interface Icheckable");
            }
        }

        private void ChildCheckRemoved(object sender, ElementEventArgs e)
        {
            if (!(e.Element is ICheckable checkable)) return;

            checkable.Clicked -= OnItemClicked;
            CheckList.Remove(checkable);
            ItemsSource.Remove(checkable.Key);

            var index = 0;
            foreach (var check in CheckList) check.Index = index++;
        }

        private void ItemSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (KeyValuePair<string, object> item in e.NewItems)
                    if (CheckList.All(c => c.Key != item.Key))
                        AddItemToView(item);
            if (e.OldItems != null)
                foreach (KeyValuePair<string, object> item in e.OldItems)
                {
                    var view = default(ICheckable);
                    foreach (var checkable in CheckList)
                        if (checkable.Item.Key == item.Key)
                            view = checkable;
                    if (view != null) Children.Remove((View)view);
                }
        }

        private void AddItemToView(KeyValuePair<string, object> item)
        {
            if (!(GenerateCheckableView(null) is View view)) return;

            ((ICheckable)view).Item = item;
            Children.Add(view);
        }

        /// <summary>
        ///     The Rate group Items Source changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RateGroup rateGroup) || rateGroup.ItemsSource == null) return;

            if (oldValue is INotifyCollectionChanged oldSource)
                oldSource.CollectionChanged -= rateGroup.ItemSource_CollectionChanged;
            rateGroup.GenerateChekableList();
            if (newValue is INotifyCollectionChanged newSource)
                newSource.CollectionChanged += rateGroup.ItemSource_CollectionChanged;
        }

        private static void CheckTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup && rateGroup.ItemsSource != null) rateGroup.GenerateChekableList();
        }

        /// <summary>
        ///     The Rate group selected index changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static async void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RateGroup rateGroup)) return;

            rateGroup._isBusy = true;
            var animationDuration = 200 / (rateGroup.CheckList.Count() + 1);

            rateGroup.SelectedIndexChanged?.Invoke(rateGroup, (int)newValue);
            if (rateGroup.SelectedIndex >= 0 && rateGroup.CheckList.Any() &&
                rateGroup.CheckList[rateGroup.SelectedIndex].Item.Value is string str)
                rateGroup._rateLabel.Text = str;
            else
                rateGroup._rateLabel.Text = "-";

            var index = 0;
            var list = (int)oldValue < (int)newValue ? rateGroup.CheckList : rateGroup.CheckList.Reverse();
            foreach (var item in list)
            {
                if (index < rateGroup.CheckList.Count() && item.Checked != item.Index <= rateGroup.SelectedIndex)
                {
                    await Task.Delay(animationDuration);
                    item.Checked = item.Index <= rateGroup.SelectedIndex;
                }

                ++index;
            }

            rateGroup._isBusy = false;
        }

        /// <summary>
        ///     The Rate group selected index changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup.SelectedItemChanged?.Invoke(rateGroup, (KeyValuePair<string, object>)newValue);
        }

        /// <summary>
        ///     The Rate group default index changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void DefaultIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup.SelectedIndex = rateGroup.DefaultIndex;
        }

        /// <summary>
        ///     The Rate StackLayout Orientation changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup && rateGroup._rateLayout != null)
                rateGroup._rateLayout.Orientation = (StackOrientation)newValue;
        }

        /// <summary>
        ///     The Check StackLayout Spacing changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RateGroup rateGroup) || rateGroup._rateLayout == null) return;

            rateGroup.Spacing = (double)newValue;
            var index = 0;
            foreach (var item in rateGroup._rateLayout.Children)
                if (item is CheckContent checkBox)
                    checkBox.Padding = rateGroup.SetSpacingPadding(index++);
                else
                    item.Margin = rateGroup.SetSpacingPadding(index++);
        }

        /// <summary>
        ///     The Label Font Attributes property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelFontAttributesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup._rateLabel.FontAttributes = (FontAttributes)newValue;
        }

        /// <summary>
        ///     The Label Font Family property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup._rateLabel.FontFamily = (string)newValue;
        }

        /// <summary>
        ///     The Label Font Size property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup._rateLabel.FontSize = (double)newValue;
        }

        /// <summary>
        ///     The Label Horizontal TextAlignment property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelHorizontalTextAlignmentChanged(BindableObject bindable, object oldValue,
            object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup._rateLabel.HorizontalTextAlignment = (TextAlignment)newValue;
        }

        /// <summary>
        ///     The Label Vertical Text Alignment property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelVerticalTextAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup._rateLabel.VerticalTextAlignment = (TextAlignment)newValue;
        }

        /// <summary>
        ///     The Label Text Color property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup._rateLabel.TextColor = (Color)newValue;
        }

        /// <summary>
        ///     The Label is Visible property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void LabelIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RateGroup rateGroup)
                rateGroup._rateLabel.IsVisible = (bool)newValue;
        }

        private void OnItemClicked(object sender, bool check)
        {
            if (!(sender is CheckContent selectedCheckBox)) return;

            switch (check)
            {
                case false when selectedCheckBox.Index == SelectedIndex && selectedCheckBox.Index == DefaultIndex:
                    selectedCheckBox.Checked = true;
                    return;
                case false when selectedCheckBox.Index == SelectedIndex && SelectedIndex > DefaultIndex:
                    SelectedIndex = SelectedIndex - 1;
                    SelectedItem = selectedCheckBox.Item;
                    return;
                default:
                    SelectedIndex = selectedCheckBox.Index;
                    SelectedItem = selectedCheckBox.Item;
                    break;
            }
        }

        private Thickness SetSpacingPadding(int index)
        {
            if (index == 0)
                return Orientation == StackOrientation.Horizontal
                    ? new Thickness(0, 0, Spacing / 2, 0)
                    : new Thickness(0, 0, 0, Spacing / 2);
            if (index == ItemsSource.Count - 1)
                return Orientation == StackOrientation.Horizontal
                    ? new Thickness(Spacing / 2, 0, 0, 0)
                    : new Thickness(0, Spacing / 2, 0, 0);
            return Orientation == StackOrientation.Horizontal
                ? new Thickness(Spacing / 2, 0, Spacing / 2, 0)
                : new Thickness(0, Spacing / 2, 0, Spacing / 2);
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