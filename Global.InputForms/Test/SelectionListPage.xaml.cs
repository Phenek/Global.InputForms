using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class SelectionListPage : ContentPage
    {
        //new INavigation Navigation => ((MasterMenuPage)Application.Current.MainPage).Navigation;
        private readonly object _model;
        private readonly string _propertyName;

        public object SelectedItem;

        public SelectionListPage(Dictionary<string, object> itemsSource, object model, string propertyName,
            string title)
        {
            InitializeComponent();
            _model = model;
            _propertyName = propertyName;
            _collection.ItemsSource = itemsSource.ToList();
            Title = title;
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
    }
}
