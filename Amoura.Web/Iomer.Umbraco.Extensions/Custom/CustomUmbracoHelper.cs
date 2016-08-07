
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Amoura.Web.Helpers
{
    public static class CustomUmbracoHelper
    {
        public static UmbracoHelper GetUmbracoHelper()
        {
            ContextHelper.EnsureUmbracoContext();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            return umbracoHelper;
        }

        public static IPublishedContent GetById(this UmbracoHelper umbracoHelper, int id)
        {
            var content = UmbracoContext.Current == null ? umbracoHelper.GetById(id) : UmbracoContext.Current.ContentCache.GetById(id);
            return content;
        }

        public static IPublishedContent GetCurrentPage()
        {
            ContextHelper.EnsureUmbracoContext();
            var currentPage = UmbracoContext.Current.PublishedContentRequest.PublishedContent;
            return currentPage;
        }
    }
}