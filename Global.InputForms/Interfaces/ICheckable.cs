using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Global.InputForms.Interfaces
{
    public interface ICheckable
    {
        BindableProperty CheckedProperty { get; }
        BindableProperty ItemProperty { get; }

        bool DisableCheckOnClick { get; set; }
        int Index { get; set; }
        bool Checked { get; set; }
        KeyValuePair<string, string> Item { get; set; }
        string Key { get; set; }
        string Value { get; set; }

        event EventHandler<bool> CheckedChanged;
        event EventHandler<bool> Clicked;

        void SetCheckedColorsStyles();
        void OnChecked(object sender, EventArgs e);
        void OnCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue);
        void OnItemPropertyChanged(BindableObject bindable, object oldValue, object newValue);
    }
}