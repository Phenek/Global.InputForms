using System;
using System.Collections;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class TestEntry : Entry
    {
        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(BlankPicker), -1, BindingMode.TwoWay);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(BlankPicker), default(IList));

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(BlankPicker), null, BindingMode.TwoWay);

        public ContentView KeyboardInput;

        public TestEntry()
        {
            KeyboardInput = new ContentView()
            {
                BackgroundColor = Color.Red,
                HeightRequest = 200
            };
            //new CollectionKeyboard(ItemsSource, this);

        }

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
    }
}
