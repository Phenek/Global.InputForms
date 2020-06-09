using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Global.InputForms
{
    public partial class SelectionListPage : ContentPage
    {
        //new INavigation Navigation => ((MasterMenuPage)Application.Current.MainPage).Navigation;
        private readonly object _model;
        private readonly string _propertyName;

        public object SelectedItem;

        private int _firstVisibleItemIndex = 0;
        private int _lastVisibleItemIndex = 0;

        public SelectionListPage(Dictionary<string, object> itemsSource, object model, string propertyName,
            string title)
        {
            InitializeComponent();
            _model = model;
            _propertyName = propertyName;
            _carousel.ItemsSource = itemsSource.ToList();
            Title = title;

            _carousel.Scrolled += OnCarouselViewScrolled;
        }

        void OnCarouselViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            Debug.WriteLine("- - - - - - - - - - - - - - - - - - - - ");
            Debug.WriteLine("HorizontalDelta: " + e.HorizontalDelta);
            Debug.WriteLine("VerticalDelta: " + e.VerticalDelta);
            Debug.WriteLine("HorizontalOffset: " + e.HorizontalOffset);
            Debug.WriteLine("VerticalOffset: " + e.VerticalOffset);
            Debug.WriteLine("FirstVisibleItemIndex: " + e.FirstVisibleItemIndex);
            Debug.WriteLine("CenterItemIndex: " + e.CenterItemIndex);
            Debug.WriteLine("LastVisibleItemIndex: " + e.LastVisibleItemIndex);

            if (e.VerticalDelta > 0)
            {
                if (e.LastVisibleItemIndex != _lastVisibleItemIndex)
                {
                    var last = _carousel.VisibleViews.Last();
                    last.RotationX = -50;
                }
            }
            else
            {
                if (e.FirstVisibleItemIndex != _firstVisibleItemIndex)
                {
                    var first = _carousel.VisibleViews.First();
                    first.RotationX = +50;
                }
            }

            foreach (var view in _carousel.VisibleViews)
            {
                view.RotationX += e.VerticalDelta;
            }
        }

        public async void SelectItem(object selectedItem)
        {
            if (_model.GetType() is Type type)
                if (type.GetProperty(_propertyName) is PropertyInfo property)
                    property.SetValue(_model, selectedItem, null);
            await Navigation.PopAsync();
        }

        private async void Close(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine(_carousel.VisibleViews);

            var rot = 0.0;
            foreach(var view in _carousel.VisibleViews)
            {
                view.RotationX = rot;
                rot = -22.5;
            }
        }
    }
}
