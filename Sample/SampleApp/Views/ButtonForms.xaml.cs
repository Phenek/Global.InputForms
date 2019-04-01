using System;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class ButtonForms : ContentPage
    {
        private SimpleFormsViewModel _viewModel;

        public ButtonForms()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();

            var button = new Button
            {
                Text = "test"
            };
            _stack.Children.Add(button);

            button.Clicked += Button_Clicked;
        }


        private void Button_Clicked(object sender, EventArgs e)
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

            var checkeds = _checkGroup.GetCheckedDictionary().Values;
            var uncheckeds = _checkGroup.GetUnCheckedDictionary().Values;
            Console.WriteLine($"radio: {_radioGender.SelectedItem}");
            Console.WriteLine($"CheckGroup Checked: {checkeds}");
            Console.WriteLine($"CheckGroup Unchecked: {uncheckeds}");
        }
    }
}