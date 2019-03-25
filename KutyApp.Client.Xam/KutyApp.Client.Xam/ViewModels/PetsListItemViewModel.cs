using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
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
        IEnvironmentApiService EnvironmentApiService { get; }

        public PetsListItemViewModel(IEnvironmentApiService environmentApiService)
        {
            EnvironmentApiService = environmentApiService;
        }

        public PetsListItemViewModel(IEnvironmentApiService environmentApiService, PetDto dto)
        {
            EnvironmentApiService = environmentApiService;
            PetDto = dto;
        }

        private ImageSource petImageSource;
        public ImageSource PetImageSource
        {
            get => petImageSource ?? ImageSource.FromUri(new Uri("https://via.placeholder.com/600x500?text=Your+Pet"));
            set => SetProperty(ref petImageSource, value);
        }
        public PetDto PetDto { get; private set; }

        private Stream InnerStream;

        public async Task LoadImageAsync()
        {
            var content = await EnvironmentApiService.GetImageAsync(PetDto.ImagePath);
            InnerStream = await content.ReadAsStreamAsync();
            PetImageSource = ImageSource.FromStream(() =>InnerStream);
        }

    }
}
