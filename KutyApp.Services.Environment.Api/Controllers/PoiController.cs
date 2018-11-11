using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/poi")]
    public class PoiController : BaseController
    {
        private IPoiManager PoiManager { get; }

        public PoiController(IPoiManager poiManager)
        {
            PoiManager = poiManager;
        }

        [HttpPost]
        public async Task<ActionResult<PoiDto>> AddOrEditPoi(AddOrEditPoiDto dto) =>
            Result(await PoiManager.AddOrEditPoiAsync(dto));

        [HttpGet("list")]
        public async Task<ActionResult<List<PoiDto>>> ListPoisAsync() =>
            Result(await PoiManager.ListPoisAsync());

        [HttpDelete("{poiId}")]
        public async Task<ActionResult> DeletePoiAsync(int poiId) =>
            await ResultAsync(PoiManager.DeletePoiAsync(poiId));

        [HttpPost("listclosest")]
        public async Task<ActionResult<List<PoiDto>>> ListClosestPoisAsync(SearchPoiDto search) =>
           Result(await PoiManager.ListClosestPoisAsync(search));
    }
}
