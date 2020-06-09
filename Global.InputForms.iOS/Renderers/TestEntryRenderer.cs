using System;

using Xamarin.Forms;

namespace Global.InputForms.iOS.Renderers
{
    public class TestEntryRenderer : ContentPage
    {
        public TestEntryRenderer()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

