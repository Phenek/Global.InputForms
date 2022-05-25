using Sample.ViewModels;
using Microsoft.Maui.Controls;

namespace Sample.Views
{
    public partial class PickerViewPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public PickerViewPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}