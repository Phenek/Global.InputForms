using System;
using Xamarin.Forms;

namespace Global.InputForms
{
    public class BlankTimePicker : Entry
    {
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(BlankTimePicker), "t");

        public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(BlankTimePicker), null,
            validateValue: (bindable, value) =>
        {
            if (value is TimeSpan time)
                return time.TotalHours < 24 && time.TotalMilliseconds >= 0;
            return true;
        });
        public static readonly BindableProperty DoneButtonTextProperty =
            BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(BlankTimePicker), "Ok");

        public static readonly BindableProperty CancelButtonTextProperty =
            BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(BlankTimePicker), "Cancel");

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set
            {
                var oldValue = Time;
                TimeSet = true;
                SetValue(TimeProperty, value);
                TimeSelected?.Invoke(this, new NullableTimeChangedEventArgs(oldValue, value));
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

        public event EventHandler<NullableTimeChangedEventArgs> TimeSelected;
        public event EventHandler DoneClicked;
        public event EventHandler CancelClicked;
        public bool TimeSet;

        public void SendDoneClicked()
        {
            DoneClicked?.Invoke(this, new EventArgs());
        }

        public void SendCancelClicked()
        {
            CancelClicked?.Invoke(this, new EventArgs());
        }
    }

    public class NullableTimeChangedEventArgs : EventArgs
    {
        public NullableTimeChangedEventArgs(TimeSpan oldTime, TimeSpan newTime)
        {
            NewTime = oldTime;
            OldTime = newTime;
        }

        public TimeSpan NewTime { get; private set; }

        public TimeSpan OldTime { get; private set; }
    }
}