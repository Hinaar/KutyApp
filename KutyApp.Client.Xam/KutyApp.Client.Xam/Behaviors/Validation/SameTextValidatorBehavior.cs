using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Behaviors.Validation
{
    public class SameTextValidatorBehavior : ValidatorBaseBehavior
    {
        public static readonly BindableProperty CompareToProperty = BindableProperty.Create("CompareTo", typeof(string), typeof(SameTextValidatorBehavior), string.Empty);

        public string CompareTo
        {
            get => (string)base.GetValue(CompareToProperty);
            set => base.SetValue(CompareToProperty, value);
        }

        public static readonly BindableProperty ShownCompareMessageProperty = BindableProperty.Create("ShownCompareMessage", typeof(string), typeof(ValidatorBaseBehavior), string.Empty);

        public string ShownCompareMessage
        {
            get { return (string)base.GetValue(ShownCompareMessageProperty); }
            set { base.SetValue(ShownCompareMessageProperty, value); }
        }

        protected override void Validate(object sender, TextChangedEventArgs e)
        {
            base.Validate(sender, e);

            if (IsValid)
            {
                if (ExternalIsValid.HasValue)
                    IsValid = ExternalIsValid.Value;
                else if (!string.IsNullOrEmpty(CompareTo) && !string.IsNullOrEmpty(e.NewTextValue) && !CompareTo.Equals(e.NewTextValue))
                {
                    IsValid = false;
                    ErrorMessage = ShownCompareMessage;
                }
            }
        }
    }
}
