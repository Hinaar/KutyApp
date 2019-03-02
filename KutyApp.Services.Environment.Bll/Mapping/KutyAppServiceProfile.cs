﻿using AutoMapper;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities.Model;
using System.Linq;

namespace KutyApp.Services.Environment.Bll.Mapping
{
    public class KutyAppServiceProfile : Profile
    {
        public KutyAppServiceProfile()
        {
            //Poi
            CreateMap<AddOrEditPoiDto, Poi>().ForMember(p => p.Location, m => m.MapFrom<LocationResolver>());
            CreateMap<Poi, AddOrEditPoiDto>().ForMember(d => d.Latitude, m => m.MapFrom(p => p.Location.Coordinate.Y))
                                             .ForMember(d => d.Longitude, m => m.MapFrom(p => p.Location.Coordinate.X));
            CreateMap<Poi, PoiDto>().ForMember(d => d.Latitude, m => m.MapFrom(p => p.Location.Coordinate.Y))
                                    .ForMember(d => d.Longitude, m => m.MapFrom(p => p.Location.Coordinate.X));
            //DbVersion
            CreateMap<DbVersion, DbVersionDto>();


            //User
            CreateMap<User, UserDto>();
            //Pet
            CreateMap<Habit, HabitDto>();
            CreateMap<HabitDto, Habit>();
            CreateMap<AddOrEditHabitDto, Habit>();
            CreateMap<MedicalTreatment, MedicalTreatmentDto>();
            CreateMap<AddOrEditMedicalTreatmentDto, MedicalTreatment>();
            CreateMap<Pet, PetDto>().ForMember(p => p.Sitters, m => m.MapFrom(p => p.PetSittings.Select(ps => ps.Sitter).ToList() ?? Enumerable.Empty<User>()))
                                    .ForMember(p => p.IsMyPet, m => m.MapFrom<PetOwnershipResolver>());
            
        }
    }
}
