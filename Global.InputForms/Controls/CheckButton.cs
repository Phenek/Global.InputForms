using System;
using System.Collections.Generic;
using Global.InputForms.Interfaces;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class CheckButton : Button, ICheckable
    {
        public static readonly BindableProperty CheckedProperty = BindableProperty.Create(nameof(Checked), typeof(bool),
            typeof(CheckButton), false, propertyChanged: OnCheckedPropertyChanged);

        public static readonly BindableProperty ItemProperty = BindableProperty.Create(nameof(Item),
            typeof(KeyValuePair<string, object>), typeof(CheckButton), new KeyValuePair<string, object>(),
            propertyChanged: OnItemPropertyChanged);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public new static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CheckButton), Color.White,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Label Text Color property.
        /// </summary>
        public new static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
            typeof(Color), typeof(CheckButton), Color.RoyalBlue, propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Border Color Property.
        /// </summary>
        public new static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
            typeof(Color), typeof(CheckButton), Color.LightGray, propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Checked Background Color property.
        /// </summary>
        public static readonly BindableProperty CheckedBackgroundColorProperty =
            BindableProperty.Create(nameof(CheckedBackgroundColor), typeof(Color), typeof(CheckButton), Color.Gray,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Checked Label Text Color property.
        /// </summary>
        public static readonly BindableProperty CheckedTextColorProperty =
            BindableProperty.Create(nameof(CheckedTextColor), typeof(Color), typeof(CheckButton), Color.White,
                propertyChanged: ColorsChanged);

        /// <summary>
        ///     The Checked Border Color Property.
        /// </summary>
        public static readonly BindableProperty CheckedBorderColorProperty =
            BindableProperty.Create(nameof(CheckedBorderColor), typeof(Color), typeof(CheckButton), Color.Gray,
                propertyChanged: ColorsChanged);

        public CheckButton()
        {
            Padding = new Thickness(0, 0, 0, 0);

            if (Item.Value is string str)
                Text = str;

            SetButtonUnchecked();
            base.Clicked += OnChecked;
            base.Clicked += Animation;
        }

        public KeyValuePair<string, object> Item
        {
            get => (KeyValuePair<string, object>) GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public string Key
        {
            get => Item.Key;
            set => Item = new KeyValuePair<string, object>(value, Item.Value);
        }

        public object Value
        {
            get => Item.Value;
            set => Item = new KeyValuePair<string, object>(Item.Key, value);
        }

        /// <summary>
        ///     Gets or sets the unchecked Background color.
        /// </summary>
        /// <value>The Background color.</value>
        public new Color BackgroundColor
        {
            get => (Color) GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the unchecked label text color.
        /// </summary>
        /// <value>The label text color.</value>
        public new Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the unchecked border color value.
        /// </summary>
        /// <value>The border color.</value>
        public new Color BorderColor
        {
            get => (Color) GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets Checked Background color value.
        /// </summary>
        /// <value>The Checked Background color.</value>
        public Color CheckedBackgroundColor
        {
            get => (Color) GetValue(CheckedBackgroundColorProperty);
            set => SetValue(CheckedBackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets Checked Label Text color value.
        /// </summary>
        /// <value>The Checked Label Text color.</value>
        public Color CheckedTextColor
        {
            get => (Color) GetValue(CheckedTextColorProperty);
            set => SetValue(CheckedTextColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets Checked Border color value.
        /// </summary>
        /// <value>The Checked Border color.</value>
        public Color CheckedBorderColor
        {
            get => (Color) GetValue(CheckedBorderColorProperty);
            set => SetValue(CheckedBorderColorProperty, value);
        }

        /// <summary>
        ///     The Checked property.
        /// </summary>
        BindableProperty ICheckable.CheckedProperty => CheckedProperty;

        /// <summary>
        ///     The Item property.
        /// </summary>
        BindableProperty ICheckable.ItemProperty => ItemProperty;

        public bool DisableCheckOnClick { get; set; }
        public int Index { get; set; }

        event EventHandler<bool> ICheckable.CheckedChanged
        {
            add => CheckedChanged += value;
            remove => CheckedChanged += value;
        }

        event EventHandler<bool> ICheckable.Clicked
        {
            add => Clicked += value;
            remove => Clicked += value;
        }

        public bool Checked
        {
            get => (bool) GetValue(CheckedProperty);
            set
            {
                if (Checked != value) SetValue(CheckedProperty, value);
            }
        }

        KeyValuePair<string, object> ICheckable.Item
        {
            get => Item;
            set => Item = value;
        }

        string ICheckable.Key
        {
            get => Key;
            set => Key = value;
        }

        object ICheckable.Value
        {
            get => Value;
            set => Value = value;
        }

        public void SetCheckedColorsStyles()
        {
            if (Checked)
                SetButtonChecked();
            else
                SetButtonUnchecked();
        }

        public event EventHandler<bool> CheckedChanged;
        private new event EventHandler<bool> Clicked;

        private static void OnCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CheckButton checkButton)) return;
            
            checkButton.SetCheckedColorsStyles();
            checkButton.CheckedChanged?.Invoke(bindable, (bool) newValue);
        }

        private static void OnItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckButton checkButton && checkButton.Item.Value is string str)
                checkButton.Text = str;
        }

        /// <summary>
        ///     The Colors changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ColorsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CheckButton button) button.SetCheckedColorsStyles();
        }

        private void OnChecked(object sender, EventArgs e)
        {
            if (!DisableCheckOnClick)
                Checked = !Checked;
            Clicked?.Invoke(this, Checked);
        }

        private void Animation(object sender, EventArgs e)
        {
            Scale = .9;
            this.ScaleTo(1, easing: Easing.SpringOut);
        }

        private void SetButtonUnchecked()
        {
            base.BackgroundColor = BackgroundColor;
            base.BorderColor = BorderColor;
            base.TextColor = TextColor;
        }

        private void SetButtonChecked()
        {
            base.BackgroundColor = CheckedBackgroundColor;
            base.BorderColor = CheckedBorderColor;
            base.TextColor = CheckedTextColor;
        }
    }
}