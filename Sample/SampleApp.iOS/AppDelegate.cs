using System;
using System.Collections.Generic;
using Foundation;
using Global.InputForms.iOS;
using Lottie.Forms.iOS.Renderers;
using Naxam.I18n;
using Naxam.I18n.Forms;
using Naxam.I18n.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace SampleApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            //Global Init
            InputForms.Init();

            //Init I18n
            DependencyService.Register<IDependencyGetter, DepenencyGetter>();

            //Lottie Animation
            AnimationViewRenderer.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }

    //I18N
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