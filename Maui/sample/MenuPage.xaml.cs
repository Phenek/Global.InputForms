using Sample.Views;

namespace Sample
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();

            var info = DeviceDisplay.MainDisplayInfo;

            var top = info.Height / info.Density / 3;
            _stackLayout.Margin = new Thickness(0, top, 0, 0);

            _logo.TranslationY = -info.Height / info.Density / 3;

            btn.Clicked += (sender, e) =>
            {
                Navigation.PushAsync(new ButtonsPage());
            };

            toButtonContentBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new ButtonsPage()); };
            toCheckFormsBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new CheckForms()); };
            toCheckGroupBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new CheckGroupPage()); };
            toRadioGroupBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new RadioGroupPage()); };
            toRateGroupBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new RateGroupPage()); };
            toEntryViewBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new EntryViewPage()); };
            toPickerViewBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new PickerViewPage()); };
            toDatePickerViewBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new DatePickerViewPage()); };
            toTimePickerViewBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new TimePickerViewPage()); };
            toSwitchBtn.Clicked += (sender, e) =>
            { Navigation.PushAsync(new SwitchPage()); };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            var smoothAnimation = new Animation();

            //foreach (var)
            //{
            //    {0, 1, new Animation(f => _label.TranslationY = f, _label.TranslationY, translateY, Easing.Linear)},
            //    {0, 1, new Animation(f => _label.TranslationX = f, _label.TranslationX, translateX, Easing.Linear)},
            //    {0, 1, new Animation(f => _label.FontSize = f, _label.FontSize, EntryFontSize, Easing.Linear)}
            //};

            //if (EntryLayoutType == EntryLayoutType.Besieged)
            //    smoothAnimation.Add(0, 1,
            //        new Animation(f => Input.TranslationY = f, Input.TranslationY, translateY, Easing.Linear));

            //Device.BeginInvokeOnMainThread(() =>
            //    smoothAnimation.Commit(this, "EntryAnimation", 16, 200, Easing.Linear));
        }
    }
}
