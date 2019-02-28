using KutyApp.Services.Environment.Api.Extensions;
using KutyApp.Services.Environment.Bll.Interfaces.Context;
using Microsoft.AspNetCore.Mvc.Filters;
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

        public KutyAppContextFilter(IKutyAppContext context)
        {
            Context = context;
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

            await next();
        }
    }
}
