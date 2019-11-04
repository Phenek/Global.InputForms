using System;
using System.Linq;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class PickerViewPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public PickerViewPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();


            _picker.ItemsSource = _viewModel.Languages.Values.ToList();
            _picker.DoneClicked += (sender, e) => Console.WriteLine("Picker Done");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _picker.Focus();
        }
    }
}