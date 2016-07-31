using System;
using System.Web;

namespace Amoura.Web.Extensions
{
    public static class WebExtensions
    {
        public static string GetDomain(string url = "")
        {
            var originalUrl = HttpContext.Current.Request.Url;
            if (!string.IsNullOrEmpty(url))
            {
                originalUrl = new Uri(url);
            }
            var domain = originalUrl.Host;
            if (!domain.StartsWith("http"))
            {
                domain = string.Format("http://{0}", domain);
            }
            return domain;
        }
    }
}