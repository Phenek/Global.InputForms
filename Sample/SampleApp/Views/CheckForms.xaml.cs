using System.Collections.Generic;
using Global.InputForms;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class CheckForms : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public CheckForms()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
        }

        private void _CheckStore_CheckedCollectionChanged(object sender, Dictionary<string, string> e)
        {
        }

        private void _CheckStoreChanged(object sender, bool e)
        {
            if (!(sender is CheckBox checkBox)) return;
            
            var storeName = checkBox.Text;
            var store = ((SimpleFormsViewModel) BindingContext).Form.GetType().GetProperty(storeName);
            store.SetValue(((SimpleFormsViewModel) BindingContext).Form, checkBox.Checked, null);

            switch (storeName)
            {
                case "English":
                    break;
                case "Spanish":
                    break;
                case "Russian":
                    break;
            }
        }
    }
}