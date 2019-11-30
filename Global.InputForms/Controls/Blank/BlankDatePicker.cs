using System;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class BlankDatePicker : Entry
    {
        public static readonly BindableProperty FormatProperty =
            BindableProperty.Create(nameof(Format), typeof(string), typeof(BlankDatePicker), "d");

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(BlankDatePicker), new DateTime(42, 1, 1),
                BindingMode.TwoWay,
                defaultValueCreator: bindable => new DateTime(42, 1, 1));

        public static readonly BindableProperty MinimumDateProperty =
            BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(BlankDatePicker),
                new DateTime(1900, 1, 1));

        public static readonly BindableProperty MaximumDateProperty =
            BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(BlankDatePicker),
                new DateTime(2100, 12, 31));

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(BlankDatePicker), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(BlankDatePicker), "Cancel");

        public static readonly BindableProperty UpdateModeProperty =
            BindableProperty.Create(nameof(UpdateMode), typeof(UpdateMode), typeof(BlankDatePicker),
                UpdateMode.Immediately);

        private bool _dateBinded;
        public bool DateSet;

        public DateTime Date
        {
            get => (DateTime) GetValue(DateProperty);
            set
            {
                DateSet = true;
                var oldValue = Date;
                SetValue(DateProperty, value);
                DateSelected?.Invoke(this, new DateChangedEventArgs(oldValue, value));
            }
        }

        public string Format
        {
            get => (string) GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }

        public DateTime MaximumDate
        {
            get => (DateTime) GetValue(MaximumDateProperty);
            set => SetValue(MaximumDateProperty, value);
        }

        public DateTime MinimumDate
        {
            get => (DateTime) GetValue(MinimumDateProperty);
            set => SetValue(MinimumDateProperty, value);
        }

        public string DoneButtonText
        {
            get => (string) GetValue(DoneButtonTextProperty);
            set => SetValue(DoneButtonTextProperty, value);
        }

        public string CancelButtonText
        {
            get => (string) GetValue(CancelButtonTextProperty);
            set => SetValue(CancelButtonTextProperty, value);
        }

        public UpdateMode UpdateMode
        {
            get => (UpdateMode) GetValue(UpdateModeProperty);
            set => SetValue(UpdateModeProperty, value);
        }

        public event EventHandler<DateChangedEventArgs> DateSelected;
        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;

        public void SendDoneClicked()
        {
            DoneClicked?.Invoke(this, new EventArgs());
        }

        public void SendCancelClicked()
        {
            CancelClicked?.Invoke(this, new EventArgs());
        }
    }
}