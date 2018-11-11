using Microsoft.AspNetCore.StaticFiles;

namespace KutyApp.Services.Environment.Api.Extensions
{
    public static class StringExtensions
    {
        public static string GetMimeType(this string value)
        {
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(value, out string contentType);

            return contentType;
        }
    }
}
