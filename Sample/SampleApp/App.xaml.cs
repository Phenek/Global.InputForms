using System.Reflection;
using System.Resources;
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
            var toCheckFormsBtn = new Button { Text = "Check Forms" };
            toCheckFormsBtn.Clicked += (sender, e) => { Navigation.PushAsync(new CheckForms()); };

            var toCheckGroupBtn = new Button { Text = "Check Group" };
            toCheckGroupBtn.Clicked += (sender, e) => { Navigation.PushAsync(new CheckGroupPage()); };

            var toRadioGroupBtn = new Button { Text = "Radio Group" };
            toRadioGroupBtn.Clicked += (sender, e) => { Navigation.PushAsync(new RadioGroupPage()); };

            var toRateGroupBtn = new Button { Text = "Rate Group" };
            toRateGroupBtn.Clicked += (sender, e) => { Navigation.PushAsync(new RateGroupPage()); };

            var toSimpleFormsBtn = new Button {Text = "Simple Forms"};
            toSimpleFormsBtn.Clicked += (sender, e) => { Navigation.PushAsync(new SimpleForms()); };

            var toEntriesBtn = new Button {Text = "Entries"};
            toEntriesBtn.Clicked += (sender, e) => { Navigation.PushAsync(new EntryForms()); };

            var toButtonContentBtn = new Button {Text = "Buttons"};
            toButtonContentBtn.Clicked += (sender, e) => { Navigation.PushAsync(new ButtonsPage()); };

            var toTestBtn = new Button { Text = "Test" };
            toTestBtn.Clicked += (sender, e) => { Navigation.PushAsync(new TestPage()); };

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
                        toSimpleFormsBtn,
                        toEntriesBtn,
                        toButtonContentBtn,
                        //toTestBtn
                    }
                }
            };
        }
    }
}