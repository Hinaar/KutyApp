using KutyApp.Services.Environment.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseController : Controller
    {
        protected ActionResult<T> Result<T>(T result) =>
           Ok(result);

        protected ActionResult Result() =>
            new EmptyResult();

        protected async Task<ActionResult> EmptyAsync(Task task)
        {
            await task;
            return NoContent();
        }

        protected async Task<ActionResult> ResultAsync(Task task)
        {
            await task;
            return new EmptyResult();
        }

        protected ActionResult<FileContentResult> FileResult(byte[] content, string filename) =>
            File(content, filename.GetMimeType(), filename);
    }
}
