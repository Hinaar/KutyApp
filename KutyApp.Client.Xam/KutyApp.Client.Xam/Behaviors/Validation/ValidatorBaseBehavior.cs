using System;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Behaviors.Validation
{
    public class ValidatorBaseBehavior : Behavior<Entry>
    {
        #region inner access
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(ValidatorBaseBehavior), true);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            protected set { base.SetValue(IsValidPropertyKey, value); }
        }

        static readonly BindablePropertyKey IsInvalidPropertyKey = BindableProperty.CreateReadOnly("IsInvalid", typeof(bool), typeof(ValidatorBaseBehavior), false);
        public static readonly BindableProperty IsInvalidProperty = IsInvalidPropertyKey.BindableProperty;

        public bool IsInvalid
        {
            get { return !(bool)base.GetValue(IsValidProperty); }
            protected set { base.SetValue(IsValidPropertyKey, !value); }
        }

        static readonly BindablePropertyKey ErrorMessagePropertyKey = BindableProperty.CreateReadOnly("ErrorMessage", typeof(string), typeof(ValidatorBaseBehavior), string.Empty);

        public static readonly BindableProperty ErrorMessageProperty = ErrorMessagePropertyKey.BindableProperty;

        public string ErrorMessage
        {
            get { return (string)base.GetValue(ErrorMessageProperty); }
            protected set { base.SetValue(ErrorMessagePropertyKey, value); }
        }

        #endregion

        public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create("IsRequired", typeof(bool), typeof(ValidatorBaseBehavior), false);
        public bool IsRequired
        {
            get { return (bool)base.GetValue(IsRequiredProperty); }
            set { base.SetValue(IsRequiredProperty, value); }
        }

        public static readonly BindableProperty ExternalIsValidProperty = BindableProperty.Create("ExternalIsValid", typeof(bool?), typeof(ValidatorBaseBehavior), null);

        public bool? ExternalIsValid
        {
            get { return (bool?)base.GetValue(ExternalIsValidProperty); }
            set { base.SetValue(ExternalIsValidProperty, value); }
        }

        public static readonly BindableProperty ShownMessageProperty = BindableProperty.Create("ShownMessage", typeof(string), typeof(ValidatorBaseBehavior), string.Empty);

        public string ShownMessage
        {
            get { return (string)base.GetValue(ShownMessageProperty); }
            set { base.SetValue(ShownMessageProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Focused += FocusGained;
        }

        protected virtual void FocusGained(object sender, FocusEventArgs e)
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

        protected virtual void Validate(object sender, TextChangedEventArgs e)
        {
            if (ExternalIsValid.HasValue)
                IsValid = ExternalIsValid.Value;
            else
            {
                if (IsRequired && string.IsNullOrEmpty(e.NewTextValue))
                {
                    IsValid = false;
                    ErrorMessage = ShownMessage;
                }
                else
                    IsValid = true;
            }
        }
    }
}
