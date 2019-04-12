using Android.Content;
using Android.Graphics.Drawables;
using KutyApp.Client.Xam.Controls;
using KutyApp.Client.Xam.Droid.Renderers;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtraEditor), typeof(ExtraEditorRendererDroid))]
namespace KutyApp.Client.Xam.Droid.Renderers
{
    public class ExtraEditorRendererDroid : EditorRenderer
    {
        private Android.Graphics.Color? backColor;

        public ExtraEditorRendererDroid(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;

            var customEntry = e.NewElement as ExtraEditor;
            if (customEntry == null) return;

            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(customEntry.CornerRadius);
            gd.SetStroke(customEntry.BorderThickness, customEntry.BorderColor.ToAndroid());
            gd.SetColor(customEntry.MyBackgroundColor.ToAndroid());
            backColor = customEntry.MyBackgroundColor.ToAndroid();
            Control.SetBackground(gd);
        }

        private string[] customRenderValues = { nameof(ExtraEditor.MyBackgroundColor), nameof(ExtraEditor.CornerRadius), nameof(ExtraEditor.BorderThickness), nameof(ExtraEditor.BorderColor) };

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var customEntry = sender as ExtraEditor;

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