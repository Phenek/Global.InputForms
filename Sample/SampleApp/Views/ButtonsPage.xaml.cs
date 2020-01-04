using System;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class ButtonsPage : ContentPage
    {
        private readonly SimpleFormsViewModel _viewModel;

        public ButtonsPage()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();
        }

        private void SubmitClicked(object sender, EventArgs e)
        {
            _viewModel.OnSubmit();
        }

        private void ReadMoreClicked(object sender, EventArgs e)
        {
            _descriptionLabel.MaxLines = _descriptionLabel.MaxLines == 2 ? 42 : 2;
            _readBtn.Text = _descriptionLabel.MaxLines == 2 ? "Read more" : "Read less";
        }

        private void ParameterClicked(object sender, EventArgs e)
        {
            /*
            _RadioGender.ItemsSource.Add("Spanish", "3eme type");
            _RadioGender.Children.Add(new CheckButton(){
                Key = "QWRTT",
                Value = "d3qrt"
            });
            */
            //_RadioGender.ItemsSource.Remove("Spanish");
            //_RadioGender.Children.RemoveAt(0);

            //var checkeds = _checkGroup.GetCheckedDictionary().Values;
            //var uncheckeds = _checkGroup.GetUnCheckedDictionary().Values;
            //Console.WriteLine($"radio: {_radioGender.SelectedItem}");
            //Console.WriteLine($"CheckGroup Checked: {checkeds}");
            //Console.WriteLine($"CheckGroup Unchecked: {uncheckeds}");
        }
    }
}