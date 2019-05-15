using System;
using System.Threading.Tasks;
using Global.InputForms;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(500);
            ((EntryLayout)_entry).Focus();
        }
    }
}