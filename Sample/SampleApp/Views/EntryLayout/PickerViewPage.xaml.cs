using System;
using System.Collections.Generic;
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
        }
    }
}
