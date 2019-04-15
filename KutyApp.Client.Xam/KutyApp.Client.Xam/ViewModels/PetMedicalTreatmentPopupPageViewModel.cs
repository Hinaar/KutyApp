using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Enums;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Navigation;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetMedicalTreatmentPopupPageViewModel : ViewModelBase
    {
        private IKutyAppClientContext KutyAppClientContext { get; }

        public PetMedicalTreatmentPopupPageViewModel(INavigationService navigationService, IKutyAppClientContext kutyAppClientContext)
            : base(navigationService)
        {
            KutyAppClientContext = kutyAppClientContext;
        }

        private MedicalTreatmentDto originalTreatment;
        private bool inputTransparent;
        private bool allowDelete;
        private string name;
        private MedicalTreatmentType type;
        public IEnumerable<MedicalTreatmentType> MedicalTreatmentTypeValues => Enum.GetValues(typeof(MedicalTreatmentType)).Cast<MedicalTreatmentType>();
        private string description;
        private DateTime date;
        private string place;
        private string tender;
        private string price;
        private string currency;

        private ICommand editCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;

        #region Public Properties
        public bool InputTransparent
        {
            get => inputTransparent;
            set { SetProperty(ref inputTransparent, value); /*RaisePropertyChanged(nameof(SaveOrEdit)); */}
        }

        public bool AllowDelete
        {
            get => allowDelete;
            set => SetProperty(ref allowDelete, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public MedicalTreatmentType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public string Place
        {
            get => place;
            set => SetProperty(ref place, value);
        }

        public string Tender
        {
            get => tender;
            set => SetProperty(ref tender, value);
        }

        public string Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        public string Currency
        {
            get => currency;
            set => SetProperty(ref currency, value);
        }
        #endregion

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
                        originalTreatment = param.Parameter as MedicalTreatmentDto;
                        SetUIValues(originalTreatment);
                        AllowDelete = true;
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
                    if (originalTreatment == null)
                        await NavigationService.ClearPopupStackAsync(new NavigationParameters
                        {
                            {
                                nameof(NavigationHelper), new NavigationHelper
                                {
                                    Action = Common.Enums.NavigationAction.Add,
                                    Parameter = new MedicalTreatmentDto
                                        {
                                            Currency = Currency, Date = Date, Description = Description, Name = Name, Place = Place, Price = double.Parse(Price), Tender = Tender, Type = Type
                                        },
                                    ParameterTypeName = nameof(MedicalTreatmentDto)
                                }
                            }
                        });

                    else
                        await NavigationService.ClearPopupStackAsync(new NavigationParameters
                        {
                            {
                                nameof(NavigationHelper), new NavigationHelper
                                {
                                    Action = Common.Enums.NavigationAction.Edit,
                                    ParameterTypeName = nameof(MedicalTreatmentDto),
                                    Parameter = new MedicalTreatmentDto
                                    {
                                        Id = originalTreatment.Id, PetId = originalTreatment.PetId, Currency = Currency, Date = Date, Description = Description, Name = Name, Place = Place, Price = double.Parse(Price), Tender = Tender, Type = Type
                                    }
                                }
                            }
                        });
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
                            ParameterTypeName = nameof(MedicalTreatmentDto),
                            Parameter = new MedicalTreatmentDto { Id = originalTreatment.Id },
                        }
                    }
                })));

        private void SetUIValues(MedicalTreatmentDto treatment)
        {
            if (treatment == null)
                return;

            Name = treatment.Name;
            Type = treatment.Type;
            Description = treatment.Description;
            Date = treatment.Date;
            Place = treatment.Place;
            Tender = treatment.Tender;
            Price = treatment.Price.ToString();
            Currency = treatment.Currency;
        }
    }
}
