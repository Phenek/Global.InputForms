using Sample.ViewModels;
using Microsoft.Maui.Controls;

namespace Sample.Views
{
    public partial class EntryViewPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public EntryViewPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
        }
    }
}