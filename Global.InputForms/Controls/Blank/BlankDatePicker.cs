using System;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class BlankDatePicker : Entry
    {
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(BlankDatePicker), "d");

        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(BlankDatePicker), new DateTime(42, 1, 1),
            coerceValue: CoerceDate);

        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(BlankDatePicker), new DateTime(1900, 1, 1),
            validateValue: ValidateMinimumDate, coerceValue: CoerceMinimumDate);

        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(BlankDatePicker), new DateTime(2100, 12, 31),
            validateValue: ValidateMaximumDate, coerceValue: CoerceMaximumDate);

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(BlankDatePicker), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(BlankDatePicker), "Cancel");

        public event EventHandler<DateChangedEventArgs> DateSelected;
        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;
        public bool DateSet;

        private bool _dateBinded;

        public BlankDatePicker()
        {
        }

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set
            {
                DateSet = true;
                SetValue(DateProperty, value);
                //DateSelected?.Invoke(this, new NullableDateChangedEventArgs(oldValue, value));
            }
        }

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        public DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
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

        static object CoerceDate(BindableObject bindable, object value)
        {
            			var picker = (DatePicker)bindable;
			DateTime dateValue = ((DateTime)value).Date;

			if (dateValue > picker.MaximumDate)
				dateValue = picker.MaximumDate;

			if (dateValue < picker.MinimumDate)
				dateValue = picker.MinimumDate;

			return dateValue;
        }

        static object CoerceMaximumDate(BindableObject bindable, object value)
        {
            DateTime dateValue = ((DateTime)value).Date;
            var picker = (BlankDatePicker)bindable;
            if (picker.Date > dateValue)
                picker.Date = dateValue;

            return dateValue;
        }

        static object CoerceMinimumDate(BindableObject bindable, object value)
        {
            DateTime dateValue = ((DateTime)value).Date;
            var picker = (BlankDatePicker)bindable;
            if (picker.Date < dateValue)
                picker.Date = dateValue;

            return dateValue;
        }

        static bool ValidateMaximumDate(BindableObject bindable, object value)
        {
            if (bindable is BlankDatePicker picker && picker.Date is DateTime dateValue)
            {
                return dateValue >= picker.MinimumDate.Date;
            }
            return true;
        }

        static bool ValidateMinimumDate(BindableObject bindable, object value)
        {
            if (bindable is BlankDatePicker picker && picker.Date is DateTime dateValue)
            {
                return dateValue <= picker.MaximumDate.Date;
            }
            return true;
        }

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