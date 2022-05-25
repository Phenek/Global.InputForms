﻿using System;
using System.Collections.Generic;
using Global.InputForms;
using Sample.ViewModels;
using Microsoft.Maui.Controls;

namespace Sample.Views
{
    public partial class SimpleForms : ContentPage
    {
        private readonly SimpleFormsViewModel _viewModel;

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

        private void _RadioGenderChanged(object sender, object e)
        {
            if (sender is Global.InputForms.RadioGroup && e is KeyValuePair<string, object> kvp)
                _viewModel.Form.Gender = kvp.Key != null && kvp.Value is string str ? str : null;
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

        private void _RateSaleChanged(object sender, object e)
        {
            if (e is KeyValuePair<string, object> kvp)
                _viewModel.Form.RateEnglish = Convert.ToInt32(kvp.Key);
        }

        private void _RateStreetChanged(object sender, object e)
        {
            if (e is KeyValuePair<string, object> kvp)
                _viewModel.Form.RateSpanish = Convert.ToInt32(kvp.Key);
        }

        private void _RateBeautyChanged(object sender, object e)
        {
            if (e is KeyValuePair<string, object> kvp)
                _viewModel.Form.RateRussian = Convert.ToInt32(kvp.Key);
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