using System;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class CheckGroupPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;
        int i = 0;

        public CheckGroupPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();

            _checkStore.Validate();
        }

        private void TOTOClicked(object sender, EventArgs e)
        {
            _viewModel.Languages.Add("Lili"+i++.ToString());

            var titi = _checkStore.GetCheckedItems();
            var toto = _checkStore.GetUnCheckedItems();

            _viewModel.Languages.RemoveAt(0);

            var tata = _checkStore.GetCheckedItems();
            var polo = _checkStore.GetUnCheckedItems();

        }
    }
}