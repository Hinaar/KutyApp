using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KutyApp.Client.Xam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPopupPage : PopupPage
	{
		public RegisterPopupPage()
		{
			InitializeComponent ();
		}

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();

            FrameContainer.HeightRequest = -1;

            if (!IsAnimationEnabled)
            {
                RegisterButton.Scale = 1;
                RegisterButton.Opacity = 1;
                
                UsernameEntry.TranslationX = PasswordEntry.TranslationX = EmailEntry.TranslationX = PasswordConfirmEntry.TranslationX = PhoneNumberEntry.TranslationX = 0;
                UsernameEntry.Opacity = PasswordEntry.Opacity = EmailEntry.Opacity = PasswordConfirmEntry.Opacity = PhoneNumberEntry.Opacity = 1;

                return;
            }

            RegisterButton.Scale = 0.3;
            RegisterButton.Opacity = 0;

            UsernameEntry.TranslationX = PasswordEntry.TranslationX = EmailEntry.TranslationX = PasswordConfirmEntry.TranslationX = PhoneNumberEntry.TranslationX = - 10;
            UsernameEntry.Opacity = PasswordEntry.Opacity = EmailEntry.Opacity = PasswordConfirmEntry.Opacity = PhoneNumberEntry.Opacity = 0;
        }

        protected override async Task OnAppearingAnimationEndAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var translateLength = 400u;

            await Task.WhenAll(
                UsernameEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                UsernameEntry.FadeTo(1),
                (new Func<Task>(async () =>
                {
                    await Task.Delay(100);
                    await Task.WhenAll(
                            EmailEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                            EmailEntry.FadeTo(1)
                        );
                }))(),
                (new Func<Task>(async () =>
                {
                    await Task.Delay(100);
                    await Task.WhenAll(
                            PasswordEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                            PasswordEntry.FadeTo(1)
                        );
                }))(),
                (new Func<Task>(async () =>
                {
                    await Task.Delay(100);
                    await Task.WhenAll(
                            PasswordConfirmEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                            PasswordConfirmEntry.FadeTo(1)
                        );
                }))(),
                (new Func<Task>(async () =>
                {
                    await Task.Delay(100);
                    await Task.WhenAll(
                            PhoneNumberEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                            PhoneNumberEntry.FadeTo(1)
                        );
                }))()
                );

            await Task.WhenAll(
                RegisterButton.ScaleTo(1),
                RegisterButton.FadeTo(1)
                );

            RegisterButton.IsEnabled = false;
        }

        protected override async Task OnDisappearingAnimationBeginAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var taskSource = new TaskCompletionSource<bool>();

            var currentHeight = FrameContainer.Height;

            await Task.Run(() => Parallel.ForEach(PageStack.Children, (c) => c.FadeTo(0)));

            FrameContainer.Animate("HideAnimation", d =>
            {
                FrameContainer.HeightRequest = d;
            },
            start: currentHeight,
            end: currentHeight + 20,
            finished: async (d, b) =>
            {
                await Task.Delay(100);
                taskSource.TrySetResult(true);
            });

            await taskSource.Task;
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();

            return false;
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}