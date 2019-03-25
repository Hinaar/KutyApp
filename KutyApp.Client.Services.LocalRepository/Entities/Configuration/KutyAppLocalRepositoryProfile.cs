using AutoMapper;
using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.LocalRepository.Entities.Models;
using System;

namespace KutyApp.Client.Services.LocalRepository.Entities.Configuration
{
    public class KutyAppLocalRepositoryProfile : Profile
    {
        public KutyAppLocalRepositoryProfile()
        {
            CreateMap<Habit, HabitDto>().ForMember(d => d.StartTime, m => m.MapFrom(h => TimeSpan.FromSeconds(h.StartTime ?? 0)))
                                        .ForMember(d => d.EndTime, m => m.MapFrom(h => TimeSpan.FromSeconds(h.EndTime ?? 0)));
            CreateMap<HabitDto, Habit>().ForMember(h => h.StartTime, m => m.MapFrom(d => d.StartTime.HasValue? d.StartTime.Value.TotalSeconds : 0))
                                        .ForMember(h => h.EndTime, m => m.MapFrom(d => d.EndTime.HasValue? d.EndTime.Value.TotalSeconds : 0));

            CreateMap<MedicalTreatment, MedicalTreatmentDto>();
            CreateMap<MedicalTreatmentDto, MedicalTreatment>();

            CreateMap<PetDto, Pet>();
            CreateMap<Pet, PetDto>();
        }
    }
}
