using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GlobeInventory.Views
{
    public partial class SelectionCell : ContentView
    {
        public SelectionCell()
        {
            InitializeComponent();
            _btn.Clicked += PointCell_Clicked;

            TextChanged();
            BindingContextChanged += TextChanged;
        }

        private void TextChanged(object sender = null, EventArgs e = null)
        {
            if(BindingContext is Type type)
            {

            }
            if (BindingContext is KeyValuePair<string, object> kvp)
            {
                _btn.Text = (string)kvp.Value;
            }
            else if (BindingContext is string str)
            {
                _btn.Text = str;
            }
        }

        private async void PointCell_Clicked(object sender, EventArgs e)
        {
            if (!(Parent.Parent is SelectionListPage page)) return;


            if (BindingContext is KeyValuePair<string, object> kvp)
            {
                page.SelectItem((string)kvp.Value);
            }
            else if (BindingContext is string str)
            {
                page.SelectItem(str);
            }
        }
    }
}
