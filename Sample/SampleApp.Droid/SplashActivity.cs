using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using SampleApp.Droid;

namespace MyGlobe.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            var intent = new Intent(this, typeof(MainActivity));
            if (Intent.Extras != null)
                intent.PutExtras(Intent.Extras); // copy push info from splash to main

            StartActivity(intent);
            OverridePendingTransition(0, 0);
        }

        public override void OnBackPressed()
        {
        }
    }
}