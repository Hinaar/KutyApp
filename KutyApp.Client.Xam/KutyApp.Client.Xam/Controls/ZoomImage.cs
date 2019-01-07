using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace KutyApp.Client.Xam.Controls
{
    public class ZoomImage : Image
    {
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 2.5;
        private const double OVERSHOOT = 0.15;
        private double StartScale, LastScale, StartX, StartY;

        public ZoomImage()
        {
            var pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinch);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            //var tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            //tap.Tapped += OnTapped;
            //GestureRecognizers.Add(tap);

            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0.5;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (Scale > MIN_SCALE)
            {
                this.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.5; //TODO tapped position
                this.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    {
                        StartX = (1 - AnchorX) * Width;
                        StartY = (1 - AnchorY) * Height;
                        break;
                    }
                case GestureStatus.Running:
                    {
                        AnchorX = (1 - (StartX + e.TotalX) / Width).Clamp(0, 1);
                        AnchorY = (1 - (StartY + e.TotalY) / Height).Clamp(0, 1);
                        break;
                    }
            }
        }
        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    {
                        LastScale = e.Scale;
                        StartScale = Scale;
                        AnchorX = e.ScaleOrigin.X;
                        AnchorY = e.ScaleOrigin.Y;
                        break;
                    }
                case GestureStatus.Running:
                    {
                        if (e.Scale < 0 || Math.Abs(LastScale - e.Scale) > (LastScale * 1.3) - LastScale)
                        {
                            // e.Scale sometimes returns wildly different values from one update to the next. This causes flickering                    
                            // By removing values that are too far off, we smooth that out.                  
                            return;
                        }
                        LastScale = e.Scale;
                        var current = Scale + (e.Scale - 1) * StartScale; Scale = current.Clamp(MIN_SCALE * (1 - OVERSHOOT), MAX_SCALE * (1 + OVERSHOOT));
                        break;
                    }
                case GestureStatus.Completed:
                    {
                        if (Scale > MAX_SCALE)
                        {
                            this.ScaleTo(MAX_SCALE, 250, Easing.SpringOut);
                        }
                        else if (Scale < MIN_SCALE)
                        {
                            this.ScaleTo(MIN_SCALE, 250, Easing.SpringOut);
                        }
                        break;
                    }

            }
        }
    }
}
