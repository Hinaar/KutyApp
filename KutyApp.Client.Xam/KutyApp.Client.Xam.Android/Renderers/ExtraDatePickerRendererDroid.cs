﻿using Android.Content;
using Android.Graphics.Drawables;
using KutyApp.Client.Xam.Controls;
using KutyApp.Client.Xam.Droid.Renderers;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtraDatePicker), typeof(ExtraDatePickerRendererDroid))]
namespace KutyApp.Client.Xam.Droid.Renderers
{
    public class ExtraDatePickerRendererDroid : DatePickerRenderer
    {
        public ExtraDatePickerRendererDroid(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;
            
            var customEntry = e.NewElement as ExtraDatePicker;
            if (customEntry == null) return;

            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(customEntry.CornerRadius);
            gd.SetStroke(customEntry.BorderThickness, customEntry.BorderColor.ToAndroid());
            gd.SetColor(customEntry.MyBackgroundColor.ToAndroid());
            Control.SetBackground(gd);
        }

        private string[] customRenderValues = { nameof(ExtraDatePicker.MyBackgroundColor), nameof(ExtraDatePicker.CornerRadius), nameof(ExtraDatePicker.BorderThickness), nameof(ExtraDatePicker.BorderColor) };

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var customEntry = sender as ExtraDatePicker;

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