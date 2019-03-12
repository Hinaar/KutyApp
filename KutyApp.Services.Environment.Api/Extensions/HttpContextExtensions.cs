using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace KutyApp.Services.Environment.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIpAddress(this HttpContext context)
        {
            string ip = null;


            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues value))
                ip = value.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ip))
                ip = context.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                if (context.Request.Headers.TryGetValue("REMOTE_ADDR", out value))
                    ip = value.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(ip) && ip.Count(c => c == ':') == 1)
                return ip.Split(':').First();
            else
                return ip;
        }
    }
}
