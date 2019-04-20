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
	public partial class PetSitterPopupPage : PopupPage
	{
		public PetSitterPopupPage()
		{
			InitializeComponent ();
		}

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();

            FrameContainer.HeightRequest = -1;

            if (!IsAnimationEnabled)
            {
                //EditButton.Scale = 1;
                //EditButton.Opacity = 1;
                //SaveButton.Scale = 1;
                //SaveButton.Opacity = 1;
                //DeleteButton.Scale = 1;
                //DeleteButton.Opacity = 1;

                //HabitEntry.TranslationX = DescriptionLabel.TranslationX = DescriptionEditor.TranslationX = StartPicker.TranslationX = EndPicker.TranslationX = AmountEntry.TranslationX = UnitEntry.TranslationX = 0;
                //HabitEntry.Opacity = DescriptionLabel.Opacity = DescriptionEditor.Opacity = StartPicker.Opacity = EndPicker.Opacity = AmountEntry.Opacity = UnitEntry.Opacity = 1;

                return;
            }

            //EditButton.Scale = 0.3;
            //EditButton.Opacity = 0;
            //SaveButton.Scale = 0.3;
            //SaveButton.Opacity = 0;
            //DeleteButton.Scale = 0.3;
            //DeleteButton.Opacity = 0;

            //HabitEntry.TranslationX = DescriptionLabel.TranslationX = DescriptionEditor.TranslationX = StartPicker.TranslationX = EndPicker.TranslationX = AmountEntry.TranslationX = UnitEntry.TranslationX = -10;
            //HabitEntry.Opacity = DescriptionLabel.Opacity = DescriptionEditor.Opacity = StartPicker.Opacity = EndPicker.Opacity = AmountEntry.Opacity = UnitEntry.Opacity = 0;
        }

        protected override async Task OnAppearingAnimationEndAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var translateLength = 400u;

            await Task.WhenAll(
                //HabitEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //HabitEntry.FadeTo(1),
                //DescriptionLabel.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //DescriptionLabel.FadeTo(1),
                //DescriptionEditor.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //DescriptionEditor.FadeTo(1),
                //StartPicker.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //StartPicker.FadeTo(1),
                //EndPicker.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //EndPicker.FadeTo(1),
                //AmountEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //AmountEntry.FadeTo(1),
                //UnitEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //UnitEntry.FadeTo(1)

                //(new Func<Task>(async () =>
                //{
                //    await Task.Delay(300);
                //    await Task.WhenAll(
                //            //RememberCheckBox.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                //            //RememberCheckBox.FadeTo(1)
                //        );
                //}))()
                );

            await Task.WhenAll(
                //EditButton.ScaleTo(1),
                //EditButton.FadeTo(1),
                //SaveButton.ScaleTo(1),
                //SaveButton.FadeTo(1),
                //DeleteButton.ScaleTo(1),
                //DeleteButton.FadeTo(1)
                );

        }

        protected override async Task OnDisappearingAnimationBeginAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var taskSource = new TaskCompletionSource<bool>();

            var currentHeight = FrameContainer.Height;

            //await Task.WhenAll(
            //    HabitEntry.FadeTo(0),
            //    DescriptionLabel.FadeTo(0),
            //    DescriptionEditor.FadeTo(0),
            //    StartPicker.FadeTo(0),
            //    EndPicker.FadeTo(0),
            //    EndPicker.FadeTo(0)

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