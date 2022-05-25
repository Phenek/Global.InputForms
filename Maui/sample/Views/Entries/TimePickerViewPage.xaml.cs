using Sample.ViewModels;
using Microsoft.Maui.Controls;

namespace Sample.Views
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