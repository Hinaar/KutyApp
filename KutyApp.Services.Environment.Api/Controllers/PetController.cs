using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    [Route("api/pet")]
    public class PetController : BaseController
    {
        private IPetManager PetManager { get; }

        public PetController(IPetManager petManager)
        {
            PetManager = petManager;
        }

        [HttpPost]
        public async Task<ActionResult<PetDto>> AddOrEditPoi(AddOrEditPetDto dto) =>
            Result(await PetManager.AddOrEditPetAsync(dto));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePet(int id) =>
            await ResultAsync(PetManager.DeleteDogAsync(id));

        [HttpPost("addPetSitter")]
        public async Task<ActionResult> AddPetSitter(AddOrRemovePetSitterDto dto) =>
            await ResultAsync(PetManager.AddPetSitter(dto));

        [HttpPost("removePetSitter")]
        public async Task<ActionResult> RemovePetSitter(AddOrRemovePetSitterDto dto) =>
            await ResultAsync(PetManager.RemovePetSitter(dto));

        [HttpGet("{id}")]
        public async Task<ActionResult<PetDto>> GetPet(int id) =>
            Result(await PetManager.GetPetAsync(id));

        [HttpGet("myPets")]
        public async Task<ActionResult<List<PetDto>>> ListMyPets() =>
            Result(await PetManager.ListMyPetsAsync());

        [HttpGet("mySittedPets")]
        public async Task<ActionResult<List<PetDto>>> ListMySittedPets() =>
            Result(await PetManager.ListMySittedPetsAsync());
    }
}
