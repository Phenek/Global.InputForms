using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class RadioGroupPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public RadioGroupPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();

            _radioGroup.Validate();
        }
    }
}