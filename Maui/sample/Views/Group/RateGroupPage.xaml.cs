using Sample.ViewModels;
using Microsoft.Maui.Controls;

namespace Sample.Views
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