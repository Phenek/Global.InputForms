using Android.App;
using Android.Text;
using Android.Content;
using Android.Views.InputMethods;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using static Android.App.DatePickerDialog;

namespace Global.InputForms.Handlers
{
    public partial class BlankDatePickerHandler : EntryHandler
    {
        BlankDatePicker _virtualView;
        AppCompatEditText _platformView;
        DatePickerDialog _dialog;
        EntryDateSetListener OnDateSetListener { get; } = new EntryDateSetListener();

        //public BlankDatePickerHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override AppCompatEditText CreatePlatformView()
        {
            _platformView = base.CreatePlatformView();
            _virtualView = (BlankDatePicker)VirtualView;

            if (!string.IsNullOrEmpty(_platformView.Text))
                VirtualView.Text = string.Empty;

            _platformView.Focusable = true;
            _platformView.Clickable = false;
            _platformView.InputType = InputTypes.Null;
            // Disable the Keyboard on Focus
            _platformView.ShowSoftInputOnFocus = false;
            _platformView.SetBackgroundColor(Colors.Transparent.ToAndroid());
            _platformView.SetPadding(0, 7, 0, 3);

            _virtualView.Focused += OnClick;

            UpdateDate();

            return _platformView;
        }

        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            base.ConnectHandler(platformView);

            OnDateSetListener.Handler = this;
        }

        protected override void DisconnectHandler(AppCompatEditText platformView)
        {
            base.DisconnectHandler(platformView);

            OnDateSetListener.Handler = null;
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }

        private void UpdateDate()
        {
            if (_virtualView.DateSet)
                _platformView.Text = _virtualView.Date.Date.ToString(_virtualView.Format);
            else
                _platformView.Text = string.Empty;
        }

        public void OnClick(object sender, EventArgs e)
        {
            HideKeyboard();
            _dialog = new DatePickerDialog(Context, OnDateSetListener, _virtualView.Date.Year, _virtualView.Date.Month - 1,
                _virtualView.Date.Day);
            _dialog.DatePicker.MaxDate = UnixTimestampFromDateTime(_virtualView.MaximumDate);
            _dialog.DatePicker.MinDate = UnixTimestampFromDateTime(_virtualView.MinimumDate);

            _dialog.SetButton(_virtualView.DoneButtonText, (k, p) =>
            {
                _virtualView.Text = _dialog.DatePicker.DateTime.ToString(_virtualView.Format);
                _virtualView.Date = _dialog.DatePicker.DateTime;
                _virtualView.SendDoneClicked();
                if (_virtualView != null)
                    _virtualView.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                _platformView.ClearFocus();
                HideKeyboard();
            });
            _dialog.SetButton2(_virtualView.CancelButtonText, (s, el) =>
            {
                _virtualView.SendCancelClicked();
                if (_virtualView != null)
                    _virtualView.SetValue(VisualElement.IsFocusedPropertyKey, false);
                _platformView.ClearFocus();
                HideKeyboard();
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

        private void HideKeyboard()
        {
            var imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(_platformView.WindowToken, 0);
        }

        public long UnixTimestampFromDateTime(DateTime date)
        {
            var unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public void OnDateSet(Android.Widget.DatePicker view, int year, int month, int dayOfMonth)
        {
            _virtualView.Text = _dialog.DatePicker.DateTime.ToString(_virtualView.Format);
            _virtualView.Date = new DateTime(year, month, dayOfMonth);
            if (_virtualView != null)
                _virtualView.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            _platformView.ClearFocus();
            HideKeyboard();

            _dialog = null;
        }

        class EntryDateSetListener : Java.Lang.Object, IOnDateSetListener
        {
            public BlankDatePickerHandler Handler { get; set; }

            public void OnDateSet(Android.Widget.DatePicker view, int year, int month, int dayOfMonth)
                => Handler.OnDateSet(view, year, month, dayOfMonth);
        }
    }
}