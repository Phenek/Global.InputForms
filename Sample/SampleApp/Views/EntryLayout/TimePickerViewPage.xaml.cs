using System;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class TimePickerViewPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public TimePickerViewPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
        }
    }
}