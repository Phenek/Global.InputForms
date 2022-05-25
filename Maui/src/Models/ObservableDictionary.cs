using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Global.InputForms.Models
{
    public sealed class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        public ObservableDictionary()
        {
        }

        public ObservableDictionary(int capacity) : base(capacity)
        {
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(
            dictionary, comparer)
        {
        }

        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                var exist = TryGetValue(key, out var oldValue);
                var oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
                base[key] = value;
                var newItem = new KeyValuePair<TKey, TValue>(key, value);
                if (exist)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                        newItem, oldItem, Keys.ToList().IndexOf(key)));
                }
                else
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem,
                        Keys.ToList().IndexOf(key)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
                }
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public new void Add(TKey key, TValue value)
        {
            if (ContainsKey(key)) return;

            var item = new KeyValuePair<TKey, TValue>(key, value);
            base.Add(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item,
                Keys.ToList().IndexOf(key)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        }

        public new bool Remove(TKey key)
        {
            if (!TryGetValue(key, out _)) return false;

            var item = new KeyValuePair<TKey, TValue>(key, base[key]);
            var result = base.Remove(key);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item,
                Keys.ToList().IndexOf(key)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            return result;
        }

        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}