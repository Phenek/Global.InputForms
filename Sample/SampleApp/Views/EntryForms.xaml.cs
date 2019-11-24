using System;
using System.Threading.Tasks;
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
            _datePicker.TextChanged += (sender, e) => Console.WriteLine("Toto");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(500);
            _entry.Focus();
        }
    }
}