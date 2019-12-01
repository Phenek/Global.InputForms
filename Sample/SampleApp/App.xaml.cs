using System.Reflection;
using System.Resources;
using SampleApp.Controls;
using SampleApp.Styles;
using SampleApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SampleApp
{
    public partial class App : Application
    {
        private const string ResourceId = "SampleApp.ResX.AppResources";

        public App()
        {
            InitializeComponent();

            Resources.MergedDictionaries.Add(new ButtonStyles());
            Resources.MergedDictionaries.Add(new EntryStyles());

            MainPage = new NavigationPage(new StartPage());
        }

        public static ResourceManager ResManager => new ResourceManager(ResourceId, typeof(App).GetTypeInfo().Assembly);


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public class StartPage : ContentPage
    {
        public StartPage()
        {
            var toButtonContentBtn = new ButtonInputs { Title = "Buttons" };
            toButtonContentBtn.Clicked += (sender, e) => { Navigation.PushAsync(new ButtonsPage()); };

            var toCheckFormsBtn = new ButtonInputs {Title = "Check Forms"};
            toCheckFormsBtn.Clicked += (sender, e) => { Navigation.PushAsync(new CheckForms()); };

            var toCheckGroupBtn = new ButtonInputs {Title = "Check Group"};
            toCheckGroupBtn.Clicked += (sender, e) => { Navigation.PushAsync(new CheckGroupPage()); };

            var toRadioGroupBtn = new ButtonInputs {Title = "Radio Group"};
            toRadioGroupBtn.Clicked += (sender, e) => { Navigation.PushAsync(new RadioGroupPage()); };

            var toRateGroupBtn = new ButtonInputs {Title = "Rate Group"};
            toRateGroupBtn.Clicked += (sender, e) => { Navigation.PushAsync(new RateGroupPage()); };

            var toEntryViewBtn = new ButtonInputs {Title = "EntryView"};
            toEntryViewBtn.Clicked += (sender, e) => { Navigation.PushAsync(new EntryViewPage()); };

            var toPickerViewBtn = new ButtonInputs {Title = "PickerView"};
            toPickerViewBtn.Clicked += (sender, e) => { Navigation.PushAsync(new PickerViewPage()); };

            var toDatePickerViewBtn = new ButtonInputs {Title = "DatePickerView"};
            toDatePickerViewBtn.Clicked += (sender, e) => { Navigation.PushAsync(new DatePickerViewPage()); };

            var toTimePickerViewBtn = new ButtonInputs {Title = "TimePickerView"};
            toTimePickerViewBtn.Clicked += (sender, e) => { Navigation.PushAsync(new TimePickerViewPage()); };

            var toSwitchBtn = new ButtonInputs {Title = "Switch"};
            toSwitchBtn.Clicked += (sender, e) => { Navigation.PushAsync(new SwitchPage()); };

            var toSimpleFormsBtn = new ButtonInputs { Title = "Simple Forms" };
            toSimpleFormsBtn.Clicked += (sender, e) => { Navigation.PushAsync(new SimpleForms()); };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        toCheckFormsBtn,
                        toCheckGroupBtn,
                        toRadioGroupBtn,
                        toRateGroupBtn,
                        toEntryViewBtn,
                        toPickerViewBtn,
                        toDatePickerViewBtn,
                        toTimePickerViewBtn,
                        toButtonContentBtn,
                        toSwitchBtn,
                        toSimpleFormsBtn,
                        //toTestBtn
                    }
                }
            };
        }
    }
}