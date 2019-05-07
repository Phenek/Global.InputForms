using System;
using System.Collections.Generic;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class CheckGroupPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public CheckGroupPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();

            _checkStore.Validate();
        }
    }
}
