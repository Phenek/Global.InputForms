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
        KeyValuePair<string, object> Item { get; set; }
        string Key { get; set; }
        object Value { get; set; }

        event EventHandler<bool> CheckedChanged;
        event EventHandler<bool> Clicked;

        void SetCheckedColorsStyles();
    }
}