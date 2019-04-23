using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Navigation;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class AdvertisementPopupPageViewModel : ViewModelBase
    {
        private IKutyAppClientContext KutyAppClientContext { get; }
        private IEnvironmentApiService EnvironmentApiService { get; }

        public AdvertisementPopupPageViewModel(INavigationService navigationService, IKutyAppClientContext kutyAppClientContext, IEnvironmentApiService environmentApiService)
            : base(navigationService)
        {
            KutyAppClientContext = kutyAppClientContext;
            EnvironmentApiService = environmentApiService;
        }

        private AdvertDto originalAdvert;
        private string advertTitle;
        private string description;
        private DateTime createdDate;
        private string advertiserName;
        private string advertiserPhone;

        private bool inputTransparent;
        private bool allowDelete;

        private ICommand editCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;

        public bool AllowDelete
        {
            get => allowDelete;
            set => SetProperty(ref allowDelete, value);
        }
        public string AdvertTitle
        {
            get => advertTitle;
            set => SetProperty(ref advertTitle, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public DateTime CreatedDate { get => createdDate; set => SetProperty(ref createdDate, value); }

        public string AdvertiserName { get => advertiserName; set => SetProperty(ref advertiserName, value); }

        public string AdvertiserPhone { get => advertiserPhone; set => SetProperty(ref advertiserPhone, value); }

        public bool InputTransparent
        {
            get => inputTransparent;
            set { SetProperty(ref inputTransparent, value); /*RaisePropertyChanged(nameof(SaveOrEdit)); */}
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(nameof(NavigationHelper)))
            {
                var param = (NavigationHelper)parameters[nameof(NavigationHelper)];

                switch (param.Action)
                {
                    case Common.Enums.NavigationAction.Add:
                        InputTransparent = false;
                        AllowDelete = false;
                        break;
                    case Common.Enums.NavigationAction.Edit:
                        originalAdvert = param.Parameter as AdvertDto;

                        if(originalAdvert.Advertiser.Email == KutyAppClientContext.CurrentUserEmail)
                            AllowDelete = true;

                        SetUIValues(originalAdvert);
                        InputTransparent = true;
                        break;
                    default:
                        InputTransparent = true;
                        break;
                }
            }

            base.OnNavigatedTo(parameters);
        }

        public ICommand EditCommand =>
            editCommand ?? (editCommand = new Command(
                () => InputTransparent = !InputTransparent));

        public ICommand SaveCommand =>
            saveCommand ?? (saveCommand = new Command(
                async () =>
                {
                    try
                    {
                        if (originalAdvert == null)
                            await EnvironmentApiService.AddOrEditAdvertisementAsync(new AddOrEditAdvertDto { Title = AdvertTitle, Description = Description });
                        else
                            await EnvironmentApiService.AddOrEditAdvertisementAsync(new AddOrEditAdvertDto { Id = originalAdvert.Id, Title = AdvertTitle, Description = description });

                        await NavigationService.ClearPopupStackAsync();
                    }
                    catch (Exception e)
                    {

                    }
                }
                ));

        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command(
                async () => await NavigationService.ClearPopupStackAsync(new NavigationParameters
                {
                    {
                        nameof(NavigationHelper), new NavigationHelper
                        {
                            Action = Common.Enums.NavigationAction.Delete,
                            ParameterTypeName = nameof(AdvertDto),
                            Parameter = new AdvertDto { Id = originalAdvert.Id },
                        }
                    }
                })));

        private void SetUIValues(AdvertDto advertisement)
        {
            if (advertisement == null)
                return;

            AdvertTitle = advertisement.Title;
            Description = advertisement.Description;
            CreatedDate = advertisement.CreateDate;

            AdvertiserName = advertisement.Advertiser.UserName;
            AdvertiserPhone = advertisement.Advertiser.PhoneNumber;
        }
    }
}
