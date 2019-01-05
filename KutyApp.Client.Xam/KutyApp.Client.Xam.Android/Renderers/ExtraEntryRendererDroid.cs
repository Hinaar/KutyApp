using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KutyApp.Client.Xam.Controls;
using KutyApp.Client.Xam.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtraEntry), typeof(ExtraEntryRendererDroid))]
namespace KutyApp.Client.Xam.Droid.Renderers
{
    public class ExtraEntryRendererDroid : EntryRenderer
    {
        public ExtraEntryRendererDroid(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;

            var customEntry = e.NewElement as ExtraEntry;
            if (customEntry == null) return;

            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(customEntry.CornerRadius);
            gd.SetStroke(customEntry.BorderThickness, customEntry.BorderColor.ToAndroid());
            gd.SetColor(customEntry.MyBackgroundColor.ToAndroid());
            Control.SetBackground(gd);
        }

        private string[] customRenderValues = { nameof(ExtraEntry.MyBackgroundColor), nameof(ExtraEntry.CornerRadius), nameof(ExtraEntry.BorderThickness), nameof(ExtraEntry.BorderColor) };

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var customEntry = sender as ExtraEntry;

            if (customRenderValues.Contains(e.PropertyName))
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(customEntry.CornerRadius);
                gd.SetStroke(customEntry.BorderThickness, customEntry.BorderColor.ToAndroid());
                gd.SetColor(customEntry.MyBackgroundColor.ToAndroid());
                Control.SetBackground(gd);
            }
        }
    }
}