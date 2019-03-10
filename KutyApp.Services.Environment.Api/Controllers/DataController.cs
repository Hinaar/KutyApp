using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    [Route("api/data")]
    public class DataController : BaseController
    {
        public IDataManager DataManager { get; }

        public DataController(IDataManager dataManager)
        {
            DataManager = dataManager;
        }

        [HttpPost("image")]
        public async Task<ActionResult<string>> UploadImage(IFormFile file) =>
            Result(await DataManager.SaveFileAsync(file));

        [HttpGet("image")]
        public async Task<FileContentResult> GetImage(string path) =>
            FileResult(await DataManager.GetImageAsync(path), path.Split("\\").Last());
    }
}