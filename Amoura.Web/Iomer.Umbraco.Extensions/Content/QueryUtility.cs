using System.Collections.Generic;
using System.Linq;

namespace Iomer.Umbraco.Extensions.Query
{
    using System.Web;

    using Amoura.Web.Helpers;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    public static class QueryUtility
    {
        //public static UmbracoHelper umbracoHelper = CustomUmbracoHelper.GetUmbracoHelper();
        public static UmbracoHelper _umbracoHelper;
        public static UmbracoHelper umbracoHelper
        {
            get
            {
                if (_umbracoHelper == null || HttpContext.Current == null)
                {
                    _umbracoHelper = CustomUmbracoHelper.GetUmbracoHelper();
                }
                return _umbracoHelper;
            }
        }

        //public static IEnumerable<Node> GetNodesByType(string alias)
        //{
        //    var nodeList = uQuery.GetNodesByType(alias);
        //    return nodeList;
        //}

        public static IPublishedContent GetNodeByUrl(string url)
        {
            var node = UmbracoContext.Current.ContentCache.GetByRoute(url);
            return node;
        }

        //public static IEnumerable<IPublishedContent> ToPublishedContent(this IEnumerable<Node> nodeList)
        //{
        //    return nodeList.Select(i => umbracoHelper.GetById(i.Id));
        //}

        /// <summary>
        /// This method will be used to replace uQuery, but has some performance implications
        /// Need to find the most efficient way to query all nodes using this method.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static IEnumerable<IPublishedContent> GetPublishedContentByType(string alias = "")
        {
            //todo Find an efficient way to query all nodes of a specific type without using uQuery.
            //var nodeList =  umbracoHelper.TypedContentAtRoot().DescendantsOrSelf(alias);

            //var nodeList = GetNodesByType(alias).ToPublishedContent();
            var nodeList = umbracoHelper.TypedContentAtXPath($"//{alias}");
            return nodeList;
        }
    }
}