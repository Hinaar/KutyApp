using KutyApp.Services.Environment.Api.Extensions;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Filter
{
    public class KutyAppContextFilter : IAsyncActionFilter
    {
        private IKutyAppContext Context { get; }
        private Serilog.ILogger Logger { get; }

        public KutyAppContextFilter(IKutyAppContext context, Serilog.ILogger logger)
        {
            Context = context;
            Logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                Claim claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (claim != null)
                    await Context.LoadCurrentUserAsync(claim.Value);
            }

            Context.IpAddress = context.HttpContext.GetClientIpAddress();
            Logger.Information($"{Context.CurrentUser?.Name} from {Context.IpAddress} - {context.HttpContext.Request.Method}: {context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
            await next();
        }
    }
}
