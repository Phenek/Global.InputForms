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

            MainPage = new SplashScreen();
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
}