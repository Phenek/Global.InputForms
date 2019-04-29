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
            var toSimpleFormsBtn = new Button {Text = "Simple Forms"};
            toSimpleFormsBtn.Clicked += (sender, e) => { Navigation.PushAsync(new SimpleForms()); };

            var toEntriesBtn = new Button {Text = "Entries"};
            toEntriesBtn.Clicked += (sender, e) => { Navigation.PushAsync(new EntryForms()); };

            var toCheckBoxsBtnBtn = new Button {Text = "Check Boxs"};
            toCheckBoxsBtnBtn.Clicked += (sender, e) => { Navigation.PushAsync(new CheckForms()); };

            var toFormsBtn = new Button {Text = "Buttons"};
            toFormsBtn.Clicked += (sender, e) => { Navigation.PushAsync(new ButtonForms()); };

            var toTestBtn = new Button { Text = "Test" };
            toTestBtn.Clicked += (sender, e) => { Navigation.PushAsync(new TestPage()); };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        toSimpleFormsBtn,
                        toEntriesBtn,
                        toCheckBoxsBtnBtn,
                        toFormsBtn,
                        toTestBtn
                    }
                }
            };
        }
    }
}