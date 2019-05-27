using System;
using System.Collections.Generic;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class EntryViewPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public EntryViewPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
        }
    }
}
