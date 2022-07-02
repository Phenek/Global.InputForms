using Android.App;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Content;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using static Android.App.TimePickerDialog;

namespace Global.InputForms.Handlers
{
    public partial class BlankTimePickerHandler : EntryHandler
    {
        BlankTimePicker _virtualView;
        AppCompatEditText _platformView;
        TimePickerDialog _dialog;
        EntryTimeSetListener OnTimeSetListener { get; } = new EntryTimeSetListener();

        //public BlankTimePickerHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override AppCompatEditText CreatePlatformView()
        {
            _platformView = base.CreatePlatformView();
            _virtualView = (BlankTimePicker)VirtualView;

            if (!string.IsNullOrEmpty(_platformView.Text))
                _virtualView.Text = _platformView.Text;

            _platformView.Focusable = true;
            _platformView.Clickable = false;
            _platformView.InputType = InputTypes.Null;
            // Disable the Keyboard on Focus
            _platformView.ShowSoftInputOnFocus = false;

            _platformView.SetBackgroundColor(Colors.Transparent.ToAndroid());
            _platformView.SetPadding(0, 7, 0, 3);

            _virtualView.Focused += OnClick;

            UpdateTime();
            return _platformView;
        }

        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            base.ConnectHandler(platformView);

            OnTimeSetListener.Handler = this;
        }

        protected override void DisconnectHandler(AppCompatEditText platformView)
        {
            base.DisconnectHandler(platformView);

            OnTimeSetListener.Handler = null;
            OnTimeSetListener.Dispose();
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }

        private void UpdateTime()
        {
            if (_virtualView.TimeSet)
            {
                var time = new TimeSpan(_virtualView.Time.Hours, _virtualView.Time.Minutes, 0);
                _virtualView.Text = _platformView.Text = new DateTime(time.Ticks).ToString(_virtualView.Format);
            }
            else
            {
                _platformView.Text = string.Empty;
            }
        }

        private void HideKeyboard()
        {
            var imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(_platformView.WindowToken, 0);
        }

        public void OnClick(object sender, EventArgs e)
        {
            HideKeyboard();
            _dialog = new TimePickerDialog(Context, OnTimeSetListener, _virtualView.Time.Hours, _virtualView.Time.Minutes, true);

            _dialog.SetButton(_virtualView.DoneButtonText, (k, p) => { });
            _dialog.SetButton2(_virtualView.CancelButtonText, (k, p) =>
            {
                if (_virtualView != null)
                    _virtualView.SetValue(VisualElement.IsFocusedProperty, false);
                _platformView.ClearFocus();
                HideKeyboard();
                _virtualView.SendCancelClicked();
            });

            _dialog.CancelEvent += _dialog_DismissEvent;

            _dialog.Show();
        }

        private void _dialog_DismissEvent(object sender, EventArgs e)
        {
            _virtualView.Unfocus();
            if (_virtualView != null)
                _virtualView.SetValue(VisualElement.IsFocusedProperty, false);
            _platformView.ClearFocus();
            HideKeyboard();
        }

        public void OnTimeSet(Android.Widget.TimePicker view, int hoursOfDay, int minute)
        {
            var time = _virtualView.Time = new TimeSpan(hoursOfDay, minute, 0);
            _platformView.Text = new DateTime(time.Ticks).ToString(_virtualView.Format);
            if (_virtualView != null)
                _virtualView.SetValue(VisualElement.IsFocusedProperty, false);
            _platformView.ClearFocus();
            HideKeyboard();
            _virtualView.SendDoneClicked();
            _dialog = null;
        }

        class EntryTimeSetListener : Java.Lang.Object, IOnTimeSetListener
        {
            public BlankTimePickerHandler Handler { get; set; }

            public void OnTimeSet(Android.Widget.TimePicker view, int hourOfDay, int minute)
                => Handler.OnTimeSet(view, hourOfDay, minute);
        }
    }
}