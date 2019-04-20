using KutyApp.Services.Environment.Api.ModelBinders;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<PetDto>> AddOrEditPet(AddOrEditPetDto dto) =>
            Result(await PetManager.AddOrEditPetAsync(dto));

        [HttpPost("complex")]
        public async Task<ActionResult<PetDto>> AddorEditComplexPet([ModelBinder(typeof(JsonModelBinder))]AddOrEditPetDto dto, IFormFile file) =>
            Result(await PetManager.AddOrEditComplexPetAsync(dto, file));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePet(int id) =>
            await ResultAsync(PetManager.DeleteDogAsync(id));

        [HttpPost("addPetSitter")]
        public async Task<ActionResult> AddPetSitter(AddOrRemovePetSitterDto dto) =>
            await ResultAsync(PetManager.AddPetSitter(dto));

        [HttpPost("removePetSitter")]
        public async Task<ActionResult> RemovePetSitter(AddOrRemovePetSitterDto dto) =>
            await ResultAsync(PetManager.RemovePetSitter(dto));

        [HttpGet("availableSitters")]
        public async Task<ActionResult<List<UserDto>>> ListAvailableSitters(string username) =>
            Result(await PetManager.ListAvailableSittersAsync(username));

        [HttpGet("{id}")]
        public async Task<ActionResult<PetDto>> GetPet(int id) =>
            Result(await PetManager.GetPetAsync(id));

        [HttpGet("myPets")]
        public async Task<ActionResult<List<PetDto>>> ListMyPets() =>
            Result(await PetManager.ListMyPetsAsync());

        [HttpGet("mySittedPets")]
        public async Task<ActionResult<List<PetDto>>> ListMySittedPets() =>
            Result(await PetManager.ListMySittedPetsAsync());

        [HttpGet("myPetSitters")]
        public async Task<ActionResult<List<UserDto>>> ListMyPetSitters() =>
            Result(await PetManager.ListMyPetSittersAsync());
    }
}
