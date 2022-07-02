using Android.App;
using Android.Text;
using Android.Views;
using Android.Content;
using Android.Views.InputMethods;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using Android.Widget;

namespace Global.InputForms.Handlers
{
    public partial class BlankPickerHandler : EntryHandler
    {
        BlankPicker _virtualView;
        AppCompatEditText _platformView;
        AlertDialog _dialog;

        //public BlankPickerHandler(IPropertyMapper mapper) : base(mapper)
        //{
        //}

        protected override AppCompatEditText CreatePlatformView()
        {
            _platformView = base.CreatePlatformView();
            _virtualView = (BlankPicker)VirtualView;

            if (!string.IsNullOrEmpty(_platformView.Text))
                _platformView.Text = _platformView.Text;

            _platformView.Focusable = true;
            _platformView.Clickable = false;
            _platformView.InputType = InputTypes.Null;
            // Disable the Keyboard on Focus
            _platformView.ShowSoftInputOnFocus = false;
            _platformView.KeyListener = null;

            _platformView.SetBackgroundColor(Colors.Transparent.ToAndroid());
            _platformView.SetPadding(0, 7, 0, 3);

            _virtualView = (BlankPicker)VirtualView;
            _virtualView.Focused += OnClick;
            _platformView.Text = _virtualView.SelectedItem?.ToString();

            return _platformView;
        }

        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(AppCompatEditText platformView)
        {
            base.DisconnectHandler(platformView);
        }

        protected override void RemoveContainer()
        {
            base.RemoveContainer();
        }

        protected override void SetupContainer()
        {
            base.SetupContainer();
        }

        private void HideKeyboard()
        {
            var imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(_platformView.WindowToken, 0);
        }

        public void OnClick(object sender, EventArgs e)
        {
            HideKeyboard();
            var model = _virtualView;
            var picker = new NumberPicker(Context);
            if (model.Items != null && model.Items.Any())
            {
                picker.MaxValue = model.Items.Count - 1;
                picker.MinValue = 0;
                picker.SetDisplayedValues(model.Items.ToArray());
                picker.WrapSelectorWheel = false;
                picker.DescendantFocusability = DescendantFocusability.BlockDescendants;
                picker.Value = model.SelectedIndex;
            }

            var layout = new LinearLayout(Context) { Orientation = Orientation.Vertical };
            layout.AddView(picker);

            var builder = new AlertDialog.Builder(Context);
            builder.SetView(layout);
            builder.SetTitle(model.Title ?? "");
            builder.SetNegativeButton(_virtualView.CancelButtonText ?? "Cancel", (s, a) =>
            {
                _virtualView.SendCancelClicked();
                if (_virtualView != null)
                    _virtualView.SetValue(VisualElement.IsFocusedPropertyKey, false);
                _dialog = null;
                _platformView.ClearFocus();
                HideKeyboard();
            });
            builder.SetPositiveButton(_virtualView.DoneButtonText ?? "OK", (s, a) =>
            {
                if (_virtualView != null)
                    _virtualView.SetValue(BlankPicker.SelectedIndexProperty, picker.Value);
                //blankPicker.SelectedItem = picker.Value;
                _virtualView.SendDoneClicked();
                // It is possible for the Content of the Page to be changed on SelectedIndexChanged. 
                // In this case, the Element & _platformView will no longer exist.
                if (_virtualView != null)
                {
                    if (model.Items.Count > 0 && _virtualView.SelectedIndex >= 0)
                        _platformView.Text = model.Items[_virtualView.SelectedIndex];
                    _virtualView.SetValue(VisualElement.IsFocusedPropertyKey, false);
                    _platformView.ClearFocus();
                    HideKeyboard();
                }

                _dialog = null;
            });

            _dialog = builder.Create();
            _dialog.DismissEvent += (s, args) =>
            {
                if (_virtualView != null)
                    _virtualView?.SetValue(VisualElement.IsFocusedPropertyKey, false);
                _platformView.ClearFocus();
                HideKeyboard();
            };
            _dialog.Show();
        }
    }
}