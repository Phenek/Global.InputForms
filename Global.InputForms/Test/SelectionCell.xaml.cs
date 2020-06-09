using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Global.InputForms
{
    public partial class SelectionCell : ContentView
    {
        public SelectionCell()
        {
            InitializeComponent();

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
