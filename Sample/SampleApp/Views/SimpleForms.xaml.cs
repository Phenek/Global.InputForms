using System;
using System.Collections.Generic;
using Global.InputForms;
using Naxam.I18n;
using Naxam.I18n.Forms;
using SampleApp.ViewModels;
using Xamarin.Forms;

namespace SampleApp.Views
{
    public partial class SimpleForms : ContentPage
    {
        private readonly SimpleFormsViewModel _viewModel;

        private ILocalizedResourceProvider _i18N = DependencyService.Get<IDependencyGetter>()
            .Get<ILocalizedResourceProvider>();

        public SimpleForms()
        {
            BindingContext = _viewModel = new SimpleFormsViewModel();
            InitializeComponent();

            _rateEnglish.SelectedItemChanged += _RateSaleChanged;
            _rateSpanish.SelectedItemChanged += _RateStreetChanged;
            _rateRussian.SelectedItemChanged += _RateBeautyChanged;

            _radioGender.SelectedItemChanged += _RadioGenderChanged;
            _checkStore.CheckedChanged += _CheckStoreChanged;
        }

        private void _RadioGenderChanged(object sender, KeyValuePair<string, object> e)
        {
            if (sender is RadioGroup)
                _viewModel.Form.Gender = e.Key != null && e.Value is string str ? str : null;
        }

        private void _CheckStoreChanged(object sender, bool e)
        {
            if (!(sender is CheckLabel CheckLabel)) return;

            var storeName = CheckLabel.Text;
            var store = ((SimpleFormsViewModel) BindingContext).Form.GetType().GetProperty(storeName);
            if (store != null) store.SetValue(((SimpleFormsViewModel) BindingContext).Form, CheckLabel.Checked, null);

            switch (storeName)
            {
                case "English":
                    _rateEnglish.IsVisible = CheckLabel.Checked;
                    break;
                case "Spanish":
                    _rateSpanish.IsVisible = CheckLabel.Checked;
                    break;
                case "Russian":
                    _rateRussian.IsVisible = CheckLabel.Checked;
                    break;
            }
        }

        private void _RateSaleChanged(object sender, KeyValuePair<string, object> e)
        {
            if (sender is RateGroup rateGroup) _viewModel.Form.RateEnglish = Convert.ToInt32(e.Key);
        }

        private void _RateStreetChanged(object sender, KeyValuePair<string, object> e)
        {
            if (sender is RateGroup rateGroup) _viewModel.Form.RateSpanish = Convert.ToInt32(e.Key);
        }

        private void _RateBeautyChanged(object sender, KeyValuePair<string, object> e)
        {
            if (sender is RateGroup rateGroup) _viewModel.Form.RateRussian = Convert.ToInt32(e.Key);
        }

        private void _AlreadyHaveAccountClicked(object sender, EventArgs e)
        {
            Navigation.RemovePage(this);
        }

        private void _SubmitClicked(object sender, EventArgs e)
        {
            if (IsFormCorrect())
            {
            }
        }

        private bool IsFormCorrect()
        {
            return _firstname.Validate() && _lastname.Validate()
                                         && _radioGender.Validate() && _email.Validate()
                                         && _emailConfirm.Validate() && _birthDate.Validate() && _checkStore.Validate();
        }
    }
}