using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class BlankTimePicker : Entry
    {
        public static readonly BindableProperty FormatProperty =
            BindableProperty.Create(nameof(Format), typeof(string), typeof(BlankTimePicker), @"H\:mm");

        public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan),
            typeof(BlankTimePicker), TimeSpan.FromDays(42),
            defaultValueCreator: bindable => TimeSpan.FromDays(42), propertyChanged: TimeChanged);

        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(BlankTimePicker), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(BlankTimePicker), "Cancel");

        public static readonly BindableProperty UpdateModeProperty =
            BindableProperty.Create(nameof(UpdateMode), typeof(UpdateMode), typeof(BlankTimePicker),
                UpdateMode.Immediately);

        public bool TimeSet;

        public UpdateMode UpdateMode
        {
            get => (UpdateMode) GetValue(UpdateModeProperty);
            set => SetValue(UpdateModeProperty, value);
        }

        public string Format
        {
            get => (string) GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }

        public TimeSpan Time
        {
            get => (TimeSpan) GetValue(TimeProperty);
            set
            {
                TimeSet = true;
                SetValue(TimeProperty, value);
            }
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

        private static void TimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BlankTimePicker picker)
                if ((TimeSpan) newValue != TimeSpan.FromDays(42))
                    picker.TimeSelected?.Invoke(picker,
                        new TimeChangedEventArgs((TimeSpan) oldValue, (TimeSpan) newValue));
        }

        public event EventHandler<TimeChangedEventArgs> TimeSelected;
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

    public class TimeChangedEventArgs : EventArgs
    {
        public TimeChangedEventArgs(TimeSpan oldTime, TimeSpan newTime)
        {
            NewTime = newTime;
            OldTime = oldTime;
        }

        public TimeSpan NewTime { get; }

        public TimeSpan OldTime { get; }
    }
}