using System;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class DatePickerViewPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public DatePickerViewPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
            _picker.MaximumDate = DateTime.Today.AddYears(2);
            _picker.MinimumDate = DateTime.Today.AddYears(-1);
        }
    }
}