using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private RequestDelegate Next { get; }
        private Serilog.ILogger Logger { get; }

        public ErrorHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                //if (e is ArgumentNullException)
                //{

                //}
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { ErrorCode = context.Response.StatusCode, Message = e.Message}, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            }
        }
    }
}
