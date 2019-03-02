using AutoMapper;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;

namespace KutyApp.Services.Environment.Bll.Mapping
{
    public class PetOwnershipResolver : IValueResolver<Pet, PetDto, bool>
    {
        private IKutyAppContext KutyAppContext { get; }

        public PetOwnershipResolver(IKutyAppContext kutyAppContext)
        {
            KutyAppContext = kutyAppContext;
        }

        public bool Resolve(Pet source, PetDto destination, bool destMember, ResolutionContext context) =>
            source.OwnerId == KutyAppContext.CurrentUser?.Id;
    }
}
