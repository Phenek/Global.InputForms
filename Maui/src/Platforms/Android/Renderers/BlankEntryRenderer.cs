﻿//using System.ComponentModel;
//using Android.Content;
//using Android.Views;
//using Global.InputForms;
//using Global.InputForms.Droid.Renderers;
//using Microsoft.Maui.Controls;
//using Microsoft.Maui.Controls.Compatibility;
//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
//using Color = Android.Graphics.Color;
//using Microsoft.Maui.Controls.Platform;

////[assembly: ExportRenderer(typeof(BlankEntry), typeof(BlankEntryRenderer))]

//namespace Global.InputForms.Droid.Renderers
//{
//    public class BlankEntryRenderer : EntryRenderer
//    {
//        public BlankEntryRenderer(Context context) : base(context)
//        {
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
//        {
//            base.OnElementChanged(e);

//            SetAttributes();
//        }

//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);
//        }

//        private void SetAttributes()
//        {
//            Control.SetBackgroundColor(Color.Transparent);
//            Control.SetPadding(0, 7, 0, 3);
//            /*
//            if (Control.Enabled == false)
//            {
//                Control.SetHintTextColor(((BlankEntry)Element).PlaceholderColor.ToAndroid());
//                Control.SetTextColor(((BlankEntry)Element).TextColor.ToAndroid());
//            }
//            */
//        }
//    }
//}