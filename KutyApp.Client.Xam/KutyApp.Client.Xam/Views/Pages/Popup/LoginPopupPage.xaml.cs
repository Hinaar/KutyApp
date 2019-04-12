using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KutyApp.Client.Xam.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPopupPage : PopupPage
	{
		public LoginPopupPage()
		{
			InitializeComponent ();
		}

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();

            FrameContainer.HeightRequest = -1;

            if (!IsAnimationEnabled)
            {
                //CloseImage.Rotation = 0;
                //CloseImage.Scale = 1;
                //CloseImage.Opacity = 1;

                LoginButton.Scale = 1;
                LoginButton.Opacity = 1;
                RegisterButton.Scale = 1;
                RegisterButton.Opacity = 1;
                OrEntry.Scale = 1;
                OrEntry.Opacity = 1;
                
                UsernameEntry.TranslationX = PasswordEntry.TranslationX = RememberCheckBox.TranslationX = 0;
                UsernameEntry.Opacity = PasswordEntry.Opacity = RememberCheckBox.Opacity = 1;

                return;
            }

            //CloseImage.Rotation = 30;
            //CloseImage.Scale = 0.3;
            //CloseImage.Opacity = 0;

            LoginButton.Scale = 0.3;
            LoginButton.Opacity = 0;
            RegisterButton.Scale = 0.3;
            RegisterButton.Opacity = 0;
            OrEntry.Scale = 0.3;
            OrEntry.Opacity = 0;

            UsernameEntry.TranslationX = PasswordEntry.TranslationX = RememberCheckBox.TranslationX = -10;
            UsernameEntry.Opacity = PasswordEntry.Opacity = RememberCheckBox.Opacity = 0;
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
                    await Task.Delay(150);
                    await Task.WhenAll(
                            PasswordEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                            PasswordEntry.FadeTo(1)
                        );
                }))(),
                (new Func<Task>(async () =>
                {
                    await Task.Delay(300);
                    await Task.WhenAll(
                            RememberCheckBox.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                            RememberCheckBox.FadeTo(1)
                        );
                }))()
                );

            await Task.WhenAll(
                //CloseImage.FadeTo(1),
                //CloseImage.ScaleTo(1, easing: Easing.SpringOut),
                //CloseImage.RotateTo(0),
                LoginButton.ScaleTo(1),
                LoginButton.FadeTo(1),
                RegisterButton.ScaleTo(1),
                RegisterButton.FadeTo(1),
                OrEntry.ScaleTo(1),
                OrEntry.FadeTo(1)
                );

            LoginButton.IsEnabled = false;
        }

        protected override async Task OnDisappearingAnimationBeginAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var taskSource = new TaskCompletionSource<bool>();

            var currentHeight = FrameContainer.Height;

            //await Task.WhenAll(
            //    UsernameEntry.FadeTo(0),
            //    PasswordEntry.FadeTo(0),
            //    RememberCheckBox.FadeTo(0),
            //    LoginButton.FadeTo(0),
            //    OrEntry.FadeTo(0),
            //    RegisterButton.FadeTo(0),
            //    ErrorLabel.FadeTo(0),
            //    LoadingActivityIndicator.FadeTo(0)
            //    );

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

        //private async void OnLogin(object sender, EventArgs e)
        //{
            //var loadingPage = new LoadingPopupPage();
            //await Navigation.PushPopupAsync(loadingPage);
            //await Task.Delay(2000);
            //await Navigation.RemovePopupPageAsync(loadingPage);
            //await Navigation.PushPopupAsync(new LoginSuccessPopupPage());
        //}

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