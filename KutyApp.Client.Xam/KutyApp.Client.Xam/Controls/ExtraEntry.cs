﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Controls
{
    public class ExtraEntry : Entry
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius", typeof(int), typeof(ExtraEntry), default(int));

        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty MyBackgroundColorProperty = BindableProperty.Create("MyBackgroundColor", typeof(Color), typeof(ExtraEntry), default(Color));

        public Color MyBackgroundColor
        {
            get { return (Color)GetValue(MyBackgroundColorProperty); }
            set { SetValue(MyBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(ExtraEntry), default(Color));

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create("BorderThickness", typeof(int), typeof(ExtraEntry), default(int));

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
    }
}
