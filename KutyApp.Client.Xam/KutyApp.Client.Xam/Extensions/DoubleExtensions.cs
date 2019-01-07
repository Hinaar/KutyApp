using System;
using System.Collections.Generic;
using System.Text;

namespace KutyApp.Client.Xam.Extensions
{
    public static class DoubleExtensions
    {
        public static double Clamp(this double self, double min, double max) =>
            Math.Min(max, Math.Max(self, min));

    }
}
