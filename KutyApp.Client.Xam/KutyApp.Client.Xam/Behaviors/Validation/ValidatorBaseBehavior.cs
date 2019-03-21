using System;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Behaviors.Validation
{
    public class ValidatorBaseBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create("IsRequired", typeof(bool), typeof(ValidatorBaseBehavior), false);

        public static readonly BindableProperty ExternalIsValidProperty = BindableProperty.Create("ExternalIsValid", typeof(bool?), typeof(ValidatorBaseBehavior), null);

        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(ValidatorBaseBehavior), true);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }


        static readonly BindablePropertyKey IsInvalidPropertyKey = BindableProperty.CreateReadOnly("IsInvalid", typeof(bool), typeof(ValidatorBaseBehavior), false);
        public static readonly BindableProperty IsInvalidProperty = IsInvalidPropertyKey.BindableProperty;

        public bool IsInvalid
        {
            get { return !(bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        public bool? ExternalIsValid
        {
            get { return (bool?)base.GetValue(ExternalIsValidProperty); }
            set { base.SetValue(ExternalIsValidProperty, value); }
        }

        public bool IsRequired
        {
            get { return (bool)base.GetValue(IsRequiredProperty); }
            set { base.SetValue(IsRequiredProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Focused += FocusGained;
        }

        private void FocusGained(object sender, FocusEventArgs e)
        {
           if(sender is Entry entry)
            {
                entry.TextChanged -= Validate;
                entry.TextChanged += Validate;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Unfocused -= FocusGained;
            bindable.TextChanged -= Validate;
        }

        private void Validate(object sender, TextChangedEventArgs e)
        {
            if (ExternalIsValid.HasValue)
                IsValid = ExternalIsValid.Value;
            else
            {
                if (IsRequired && string.IsNullOrEmpty(e.NewTextValue))
                    IsValid = false;
                else
                    IsValid = true;
            }
        }
    }
}
