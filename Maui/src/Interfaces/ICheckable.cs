using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace Global.InputForms.Interfaces
{
    public interface ICheckable
    {
        public static BindableProperty CheckedProperty { get; }
        public static BindableProperty ItemProperty { get; }

        public bool DisableCheckOnClick { get; set; }
        public int Index { get; set; }
        public bool Checked { get; set; }
        public KeyValuePair<string, object> Item { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }

        public event EventHandler<bool> CheckedChanged;
        public event EventHandler<bool> Clicked;

        public void SetCheckedColorsStyles();
    }
}