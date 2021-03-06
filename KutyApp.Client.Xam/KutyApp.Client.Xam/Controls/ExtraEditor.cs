﻿using Xamarin.Forms;

namespace KutyApp.Client.Xam.Controls
{
    public class ExtraEditor : Editor
    {
        public static readonly BindableProperty MyBackgroundColorProperty = BindableProperty.Create("MyBackgroundColor", typeof(Color), typeof(ExtraEditor), default(Color));

        public Color MyBackgroundColor
        {
            get { return (Color)GetValue(MyBackgroundColorProperty); }
            set { SetValue(MyBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(ExtraEditor), default(Color));

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create("BorderThickness", typeof(int), typeof(ExtraEditor), default(int));

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius", typeof(int), typeof(ExtraEditor), default(int));

        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}
