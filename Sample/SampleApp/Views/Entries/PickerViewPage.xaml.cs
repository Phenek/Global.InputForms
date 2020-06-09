using System.Linq;
using Global.InputForms;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class PickerViewPage : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public PickerViewPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();


            var button = new TestEntry()
            {
                Text = "NewPickerTest",
                ItemsSource = _viewModel.Test.Values.ToList()
            };
            _stack.Children.Add(button);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SelectionListPage(_viewModel.Test, _viewModel.Form, "LastName", "Test"));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}