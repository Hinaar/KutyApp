using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetsListItemViewModel : BindableBase
    {
        private IEnvironmentApiService EnvironmentApiService { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }

        public PetsListItemViewModel(IEnvironmentApiService environmentApiService, IKutyAppClientContext kutyAppClientContext, PetDto dto)
        {
            EnvironmentApiService = environmentApiService;
            KutyAppClientContext = kutyAppClientContext;
            PetDto = dto;
        }

        private ImageSource petImageSource;
        public ImageSource PetImageSource
        {
            get => petImageSource ?? ImageSource.FromUri(new Uri("http://via.placeholder.com/600x500?text=Your+Pet"));
            set => SetProperty(ref petImageSource, value);
        }
        public PetDto PetDto { get; private set; }

        private Stream InnerStream;

        public async Task LoadImageAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(PetDto.ImagePath))
                {
                    if (KutyAppClientContext.IsLoggedIn)
                    {
                        var content = await EnvironmentApiService.GetImageAsync(PetDto.ImagePath);
                        InnerStream = await content.ReadAsStreamAsync();
                        PetImageSource = ImageSource.FromStream(() => InnerStream);
                    }
                    else
                        PetImageSource = ImageSource.FromFile(PetDto.ImagePath);
                }
            }
            catch (Exception e)
            {
                //notfound exc
            }
        }

    }
}
