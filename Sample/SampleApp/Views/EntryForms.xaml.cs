using System;
using Global.InputForms.Models;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class EntryForms : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public EntryForms()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
        }
    }
}