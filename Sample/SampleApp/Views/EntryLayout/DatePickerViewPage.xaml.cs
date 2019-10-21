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
        }
    }
}