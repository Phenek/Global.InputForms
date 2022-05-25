using Sample.ViewModels;
using Microsoft.Maui.Controls;

namespace Sample.Views
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