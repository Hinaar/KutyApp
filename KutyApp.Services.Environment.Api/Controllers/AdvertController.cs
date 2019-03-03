using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    [Route("api/advertisement")]
    public class AdvertController : BaseController
    {
        public IAdvertManager AdvertManager { get; }

        public AdvertController(IAdvertManager advertManager)
        {
            AdvertManager = advertManager;
        }

        [HttpPost("latest")]
        public async Task<ActionResult<List<AdvertDto>>> ListLatestAdvertisements(SearchAdvertDto dto) =>
            Result(await AdvertManager.ListLatestAdvertisementsAsync(dto));

        [HttpPost("mylatest")]
        public async Task<ActionResult<List<AdvertDto>>> ListMyLatestAdvertisements(SearchAdvertDto dto) =>
            Result(await AdvertManager.ListMyLatestAdvertisementsAsync(dto));

        //[HttpGet("nearest")]
        //public async Task<ActionResult<List<AdvertDto>>> ListNearestAdvertisements() =>
        //    Result(await AdvertManager.ListNearestAdvertisementsAsync());

        [HttpPost]
        public async Task<ActionResult<AdvertDto>> AddOrEditAdvertisement(AddOrEditAdvertDto dto) =>
            Result(await AdvertManager.AddOrEditAdvertAsync(dto));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdvert(int id) =>
             await ResultAsync(AdvertManager.DeleteAdvertAsync(id));

        [HttpGet("{id}")]
        public async Task<ActionResult<AdvertDto>> GetAdvert(int id) =>
            Result(await AdvertManager.GetAdvertAsync(id));
    }
}