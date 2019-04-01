using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Global.InputForms.Droid;
using Naxam.I18n;
using Naxam.I18n.Droid;
using Naxam.I18n.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SampleApp.Droid
{
    [Activity(Label = "SampleApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Forms.Init(this, bundle);

            //GlobeForm Init
            InputForms.Init(this, bundle);

            //I18N init
            DependencyService.Register<IDependencyGetter, DepenencyGetter>();

            LoadApplication(new App());
        }
    }

    public class DepenencyGetter : IDependencyGetter
    {
        private readonly Dictionary<Type, object> _cache;

        public DepenencyGetter()
        {
            ILocalizer localizer = new Localizer();
            _cache = new Dictionary<Type, object>
            {
                {
                    typeof(ILocalizer),
                    localizer
                },
                {
                    typeof(ILocalizedResourceProvider),
                    new LocalizedResourceProvider(localizer, App.ResManager)
                }
            };
        }

        public T Get<T>()
        {
            return (T) _cache[typeof(T)];
        }
    }
}