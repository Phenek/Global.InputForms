using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Global.InputForms
{
    public class BlankPicker : Entry
    {
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(BlankPicker), default(string));

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(BlankPicker), -1, BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexChanged, coerceValue: CoerceSelectedIndex);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(BlankPicker), default(IList),
                propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(BlankPicker), null, BindingMode.TwoWay,
                propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(BlankPicker), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(BlankPicker), "Cancel");

        public static readonly BindableProperty UpdateModeProperty =
            BindableProperty.Create(nameof(UpdateMode), typeof(UpdateMode), typeof(BlankPicker),
                UpdateMode.Immediately);

        private static readonly BindableProperty s_displayProperty =
            BindableProperty.Create("Display", typeof(string), typeof(Picker), default(string));

        private BindingBase _itemDisplayBinding;

        public BlankPicker()
        {
            ((INotifyCollectionChanged) Items).CollectionChanged += OnItemsCollectionChanged;
        }

        public UpdateMode UpdateMode
        {
            get => (UpdateMode) GetValue(UpdateModeProperty);
            set => SetValue(UpdateModeProperty, value);
        }

        public string DoneButtonText
        {
            get => (string) GetValue(DoneButtonTextProperty);
            set => SetValue(DoneButtonTextProperty, value);
        }

        public string CancelButtonText
        {
            get => (string) GetValue(CancelButtonTextProperty);
            set => SetValue(CancelButtonTextProperty, value);
        }

        public IList<string> Items { get; } = new LockableObservableListWrapper();

        public IList ItemsSource
        {
            get => (IList) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public BindingBase ItemDisplayBinding
        {
            get => _itemDisplayBinding;
            set
            {
                if (_itemDisplayBinding == value)
                    return;

                OnPropertyChanging();
                var oldValue = value;
                _itemDisplayBinding = value;
                OnItemDisplayBindingChanged(oldValue, _itemDisplayBinding);
                OnPropertyChanged();
            }
        }

        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;
        public event EventHandler SelectedIndexChanged;

        private string GetDisplayMember(object item)
        {
            if (ItemDisplayBinding == null)
                return item.ToString();

            //ItemDisplayBinding.Apply(item, this, s_displayProperty);
            //ItemDisplayBinding.Unapply();
            return (string) GetValue(s_displayProperty);
        }

        private static object CoerceSelectedIndex(BindableObject bindable, object value)
        {
            var picker = (BlankPicker) bindable;
            return picker.Items == null ? -1 : ((int) value).Clamp(-1, picker.Items.Count - 1);
        }

        private void OnItemDisplayBindingChanged(BindingBase oldValue, BindingBase newValue)
        {
            ResetItems();
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldIndex = SelectedIndex;
            var newIndex = SelectedIndex = SelectedIndex.Clamp(-1, Items.Count - 1);
            // If the index has not changed, still need to change the selected item
            if (newIndex == oldIndex)
                UpdateSelectedItem(newIndex);
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((BlankPicker) bindable).OnItemsSourceChanged((IList) oldValue, (IList) newValue);
        }

        private void OnItemsSourceChanged(IList oldValue, IList newValue)
        {
            var oldObservable = oldValue as INotifyCollectionChanged;
            if (oldObservable != null)
                oldObservable.CollectionChanged -= CollectionChanged;

            var newObservable = newValue as INotifyCollectionChanged;
            if (newObservable != null) newObservable.CollectionChanged += CollectionChanged;

            if (newValue != null)
            {
                ((LockableObservableListWrapper) Items).IsLocked = true;
                ResetItems();
            }
            else
            {
                ((LockableObservableListWrapper) Items).InternalClear();
                ((LockableObservableListWrapper) Items).IsLocked = false;
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddItems(e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(e);
                    break;
                default: //Move, Replace, Reset
                    ResetItems();
                    break;
            }
        }

        private void AddItems(NotifyCollectionChangedEventArgs e)
        {
            var index = e.NewStartingIndex < 0 ? Items.Count : e.NewStartingIndex;
            foreach (var newItem in e.NewItems)
                ((LockableObservableListWrapper) Items).InternalInsert(index++, GetDisplayMember(newItem));
        }

        private void RemoveItems(NotifyCollectionChangedEventArgs e)
        {
            var index = e.OldStartingIndex < Items.Count ? e.OldStartingIndex : Items.Count;
            foreach (var _ in e.OldItems)
                ((LockableObservableListWrapper) Items).InternalRemoveAt(index--);
        }

        private void ResetItems()
        {
            if (ItemsSource == null)
                return;
            ((LockableObservableListWrapper) Items).InternalClear();
            foreach (var item in ItemsSource)
                ((LockableObservableListWrapper) Items).InternalAdd(GetDisplayMember(item));
            UpdateSelectedItem(SelectedIndex);
        }

        private static void OnSelectedIndexChanged(object bindable, object oldValue, object newValue)
        {
            var picker = (BlankPicker) bindable;
            picker.UpdateSelectedItem(picker.SelectedIndex);
            picker.SelectedIndexChanged?.Invoke(bindable, EventArgs.Empty);
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BlankPicker) bindable;
            picker.UpdateSelectedIndex(newValue);
        }

        private void UpdateSelectedIndex(object selectedItem)
        {
            if (ItemsSource != null)
            {
                SelectedIndex = ItemsSource.IndexOf(selectedItem);
                return;
            }

            SelectedIndex = Items.IndexOf(selectedItem);
        }

        private void UpdateSelectedItem(int index)
        {
            if (index == -1)
            {
                SelectedItem = null;
                return;
            }

            if (ItemsSource != null)
            {
                SelectedItem = ItemsSource[index];
                return;
            }

            SelectedItem = Items[index];
        }


        public void SendDoneClicked()
        {
            DoneClicked?.Invoke(this, new EventArgs());
        }

        public void SendCancelClicked()
        {
            CancelClicked?.Invoke(this, new EventArgs());
        }
    }
}