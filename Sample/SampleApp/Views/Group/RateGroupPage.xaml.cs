using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class RateGroupPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public RateGroupPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();

            _rateGroup.Validate();
        }
    }
}