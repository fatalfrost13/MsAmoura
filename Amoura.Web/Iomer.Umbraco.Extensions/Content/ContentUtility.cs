using Iomer.Extensions.Array;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Iomer.Umbraco.Extensions.Content
{
    using global::Umbraco.Web;
    using System.Configuration;
    using System.Web;

    using Amoura.Web.Helpers;
    using global::Umbraco.Web.Templates;
    using Iomer.Umbraco.Extensions.Razor;

    using Amoura.Web.Data;

    public static class ContentUtility
    {
        public static IContentService contentService = ApplicationContext.Current.Services.ContentService;
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

        public static TValue GetValue<TValue>(this IContent content, string propertyAlias, TValue defaultValue)
        {
            TValue result;

            try
            {
                result = content.GetValue<TValue>(propertyAlias);
            }
            catch (InvalidCastException)
            {
                var rawValue = content.GetValue(propertyAlias);
                if (rawValue is IConvertible)
                {
                    try
                    {
                        result = (TValue)Convert.ChangeType(rawValue, typeof(TValue));
                    }
                    catch
                    {
                        result = defaultValue;
                    }
                }
                else
                {
                    result = defaultValue;
                }
            }

            return result;
        }

        public static string GetContentValue(this IPublishedContent content, string propertyAlias, string defaultValue = "")
        {
            var result = defaultValue;
            if (content != null && content.Id > 0)
            {
                var property = content.GetProperty(propertyAlias);
                if (property != null && property.HasValue && !string.IsNullOrEmpty(property.DataValue.ToString()))
                {
                    result = property.DataValue.ToString();
                }
                result = TemplateUtilities.ParseInternalLinks(result);
            }
            return result;
        }

        public static string GetContentValue(this IContent node, string alias)
        {
            var nodeValue = (node != null && node.HasProperty(alias) && node.GetValue(alias) != null) ? node.GetValue(alias).ToString().Trim() : string.Empty;
            return nodeValue;
        }
        public static string RenderBodyText(this IPublishedContent content, string propertyAlias)
        {
            var bodyText = string.Empty;
            if (content != null)
            {
                bodyText = GetContentValue(content, propertyAlias);
            }
            return bodyText; ;
        }

        public static List<IContent> GetByContentType(string alias = "")
        {
            var contentList = new List<IContent>();

            if (!string.IsNullOrEmpty(alias))
            {
                var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
                var contentTypeId = contentTypeService.GetContentType(alias).Id;
                contentList = contentTypeId > 0 ? contentService.GetContentOfContentType(contentTypeId).ToList() : new List<IContent>();
            }
            return contentList;
        }

        public static string GetTitle(this IContent iContent)
        {
            var strTitle = iContent.Name;

            var title = iContent.GetContentValue(UmbracoFields.Title);
            var pageTitle = iContent.GetContentValue(UmbracoFields.PageTitle);
            if (!string.IsNullOrEmpty(title))
            {

                strTitle = title;

            }
            else if (!string.IsNullOrEmpty(pageTitle))
            {
                strTitle = pageTitle;
            }

            return strTitle;
        }

        public static string GetTitle(this IPublishedContent iContent)
        {
            var strTitle = string.Empty;
            if (iContent != null)
            {
                strTitle = iContent.Name;
                var title = iContent.GetContentValue(UmbracoFields.Title);
                if (!string.IsNullOrEmpty(title))
                {
                    strTitle = title;
                }
                else
                {
                    var pageTitle = iContent.GetContentValue(UmbracoFields.PageTitle);
                    if (!string.IsNullOrEmpty(pageTitle))
                    {
                        strTitle = pageTitle;
                    }
                    else
                    {
                        var pageName = iContent.GetContentValue(DocumentFields.name.ToString());
                        if (!string.IsNullOrEmpty(pageName))
                        {
                            strTitle = pageName;
                        }
                    }
                }
            }
            return strTitle;
        }

        public static string GetDescription(this IPublishedContent content)
        {
            var strDesc = string.Empty;
            var desc = content?.GetContentValue(UmbracoFields.ItemDescription);
            if (!string.IsNullOrEmpty(desc))
            {

                strDesc = desc;

            }
            return strDesc;
        }

        public static bool NodeHidden(this IPublishedContent content)
        {
            var hidden = !string.IsNullOrEmpty(content.GetContentValue(UmbracoFields.Hidden)) && content.GetContentValue(UmbracoFields.Hidden) == "1";

            return hidden;
        }

        public static bool ParentHidden(this IPublishedContent content)
        {
            var parentHidden = false;
            var parentIds = content.Path.Split(',').Select(int.Parse).Where(i => i > 0).ToList();
            if (parentIds.Any())
            {
                var parentIdsReverse = parentIds.AsEnumerable().Reverse();
                foreach (var parentId in parentIdsReverse)
                {
                    var parentNode = umbracoHelper.GetById(parentId);
                    if (parentNode != null && parentNode.NodeHidden())
                    {
                        parentHidden = true;
                        return parentHidden;
                    }
                }
            }
            return parentHidden;
        }

        public static int FindChildNodeId(this IPublishedContent content, string nodeTypeAlias)
        {
            var nodeId = 0;
            var containerNode = content;
            bool[] blExit = { false };
            foreach (var childNode in containerNode.Children.Where(childNode => blExit[0] == false && childNode.DocumentTypeAlias.ValueExistsInCsv(nodeTypeAlias)))
            {
                nodeId = childNode.Id;
                blExit[0] = true;
            }
            return nodeId;
        }

        public static int FindContainerNodeId(this IPublishedContent content, string nodeTypeAlias = "")
        {
            var containerNodeId = 0;
            var aliases = nodeTypeAlias != string.Empty ? nodeTypeAlias.Split(',').ToList() : new List<string>();
            if (content != null && content.Id > 0)
            {
                if (content.DocumentTypeAlias != nodeTypeAlias)
                {
                    var parentIds = content.Path.Split(',').Select(int.Parse).Where(i => i > 0).ToList();
                    if (parentIds.Any())
                    {
                        var parentIdsReverse = parentIds.AsEnumerable().Reverse();
                        foreach (var parentId in parentIdsReverse)
                        {
                            var parentNode = umbracoHelper.GetById(parentId);
                            if (parentNode != null && (!aliases.Any() || aliases.Contains(parentNode.DocumentTypeAlias)))
                            {
                                containerNodeId = parentNode.Id;
                                return containerNodeId;
                            }
                        }
                    }
                }
                else
                {
                    containerNodeId = content.Id;
                }
            }
            return containerNodeId;
        }

        public static int FindContainerNodeId(this IContent content, string nodeTypeAlias = "")
        {
            var containerNodeId = 0;
            var aliases = nodeTypeAlias != string.Empty ? nodeTypeAlias.Split(',').ToList() : new List<string>();
            if (content != null && content.Id > 0)
            {
                if (content.ContentType.Alias != nodeTypeAlias)
                {
                    var parentIds = content.Path.Split(',').Select(int.Parse).Where(i => i > 0).ToList();
                    if (parentIds.Any())
                    {
                        var parentIdsReverse = parentIds.AsEnumerable().Reverse();
                        foreach (var parentId in parentIdsReverse)
                        {
                            var parentNode = contentService.GetById(parentId);
                            if (parentNode != null && (!aliases.Any() || aliases.Contains(parentNode.ContentType.Alias)))
                            {
                                containerNodeId = parentNode.Id;
                                return containerNodeId;
                            }
                        }
                    }
                }
                else
                {
                    containerNodeId = content.Id;
                }
            }
            return containerNodeId;
        }

        public static bool ContainsNodeWithAlias(this IPublishedContent content, string nodeTypeAlias, bool hasNode = false)
        {
            var containsNode = hasNode;
            if (!containsNode)
            {
                var firstNodeId = content.FindChildNodeId(nodeTypeAlias);
                if (firstNodeId > 0)
                {
                    containsNode = true;
                }
                else
                {
                    //recursiveLookup
                    foreach (var childNode in content.Children())
                    {
                        if (!containsNode)
                        {
                            containsNode = childNode.ContainsNodeWithAlias(nodeTypeAlias);
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            return containsNode;
        }

        public static List<int> FindAllNodeIDs(this IPublishedContent content, string nodeTypeAlias, List<int> collectionIDs = null)
        {
            if (collectionIDs == null)
            {
                collectionIDs = new List<int>();
            }

            if (content != null && content.Id > 0 && content.DocumentTypeAlias.ValueExistsInCsv(nodeTypeAlias) && !content.NodeHidden())
            {
                collectionIDs.Add(content.Id);
            }

            return content.Children().Aggregate(collectionIDs, (current, childNode) => childNode.FindAllNodeIDs(nodeTypeAlias, current));
        }

        public static string GetDate(this IPublishedContent content, string dateFormat = "")
        {
            if (string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = ConfigurationManager.AppSettings["dateFormat"];
            }
            var date = content.CreateDate;
            var dateValue = content.GetContentValue(UmbracoFields.Date);
            if (!string.IsNullOrEmpty(dateValue))
            {
                date = Convert.ToDateTime(dateValue);
            }
            var strDate = date.ToString(dateFormat);
            return strDate;
        }

        public static string GetDate(this IPublishedContent content, string nodeTypeAlias, string dateFormat = "", DateTime? defaultDate = null)
        {
            if (string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = ConfigurationManager.AppSettings["dateFormat"];
            }
            var date = defaultDate ?? content.CreateDate;
            var dateValue = content.GetContentValue(nodeTypeAlias);
            if (!string.IsNullOrEmpty(dateValue))
            {

                date = Convert.ToDateTime(dateValue);
            }

            var strDate = date != DateTime.MinValue ? date.ToString(dateFormat) : string.Empty;
            return strDate;
        }

        public static string GetUrl(this IPublishedContent content)
        {
            var strUrl = "";
            if (content != null && content.Id > 0)
            {
                strUrl = content.Url;

                //check URL Property
                var urlPickerValue = content.GetContentValue(UmbracoFields.UrlPicker);
                if (!string.IsNullOrWhiteSpace(urlPickerValue))
                {
                    strUrl = RazorUtility.UrlPickerLink(urlPickerValue, content, UmbracoFields.UrlPicker, "Url");
                    if (strUrl.Trim() == "")
                    {
                        strUrl = content.Url;
                    }
                }
            }

            return strUrl;
        }

        public static string GetTarget(this IPublishedContent content)
        {
            string strTarget = "_self";
            if (content != null && content.Id > 0)
            {
                //check URL Property
                var urlPickerValue = content.GetContentValue(UmbracoFields.UrlPicker);
                if (!string.IsNullOrWhiteSpace(urlPickerValue))
                {
                    strTarget = RazorUtility.UrlPickerLink(urlPickerValue, content, UmbracoFields.UrlPicker, "Target");
                    if (strTarget.Trim().Contains("_blank"))
                    {
                        strTarget = "_blank";
                    }
                    else if (strTarget.Trim() == "")
                    {
                        strTarget = "_self";
                    }
                }
            }

            return strTarget;
        }

        public static bool AliasExists(this string nodeTypeAlias, string aliases)
        {
            var allow = aliases == "" || nodeTypeAlias.ValueExistsInCsv(aliases);
            return allow;
        }

        public static bool GetNodeBoolean(this IPublishedContent content, string alias)
        {
            var boolValue = false;
            var contentValue = content.GetContentValue(alias);
            if (contentValue.ToLower() == "1" || contentValue.ToLower() == "true" || contentValue.ToLower() == "yes")
            {
                boolValue = true;
            }
            return boolValue;
        }

        public static bool GetNodeBoolean(this IContent node, string alias)
        {
            var boolValue = false;
            var nodeValue = node.GetContentValue(alias);
            if (nodeValue.ToLower() == "1" || nodeValue.ToLower() == "true" || nodeValue.ToLower() == "yes")
            {
                boolValue = true;
            }
            return boolValue;
        }

        public static int GetNodeInt(this IPublishedContent content, string alias)
        {
            var intValue = 0;
            var contentValue = content.GetContentValue(alias);

            if (!string.IsNullOrEmpty(contentValue))
            {
                int.TryParse(contentValue, out intValue);
            }
            return intValue;
        }

        public static List<IPublishedContent> GetNodeList(this IPublishedContent content, string alias)
        {
            var contentValue = content.GetContentValue(alias);
            var nodeList = new List<IPublishedContent>();
            if (contentValue != string.Empty)
            {
                nodeList = contentValue.Split(',').ToList().Select(i => umbracoHelper.GetById(int.Parse(i))).ToList();
            }
            return nodeList.Where(i => i != null && i.Id > 0).ToList();
        }

        public static string GetNodeValueInherit(this IPublishedContent content, string alias)
        {
            var contentValue = string.Empty;
            if (content != null && content.Id > 0)
            {
                contentValue = content.GetContentValue(alias);
                if (string.IsNullOrEmpty(contentValue))
                {
                    var parentIds = content.Path.Split(',').Select(int.Parse).Where(i => i > 0 && i != content.Id).Reverse().ToList();
                    foreach (var contentId in parentIds)
                    {
                        var parentNode = umbracoHelper.GetById(contentId);
                        contentValue = parentNode.GetContentValue(alias);
                        if (!string.IsNullOrEmpty(contentValue))
                        {
                            return contentValue;
                        }
                    }
                }
            }
            return contentValue;
        }

        public static string GetNodeValueInherit(this IContent content, string alias)
        {
            var node = umbracoHelper.GetById(content.Id);
            if (!content.Published || (node == null || node.Id == 0))
            {
                var parentId = content.ParentId;
                node = umbracoHelper.GetById(parentId);
            }

            return node.GetNodeValueInherit(alias);
        }

        public static List<int> GetNodeIdsInherit(this IPublishedContent content, string alias, IEnumerable<int> nodeIdList = null)
        {

            if (nodeIdList == null)
            {
                nodeIdList = new List<int>();
            }

            var nodeValue = content.GetContentValue(alias);
            var nodeListIds = new List<int>();

            if (nodeValue != string.Empty)
            {
                nodeListIds = nodeValue.Split(',').ToList().Select(int.Parse).ToList();
            }
            nodeListIds.AddRange(nodeIdList);

            if (content != null && content.Id > 0)
            {
                var parentIds = content.Path.Split(',').Select(int.Parse).Where(i => i > 0 && i != content.Id).Reverse().ToList();
                foreach (var nodeId in parentIds)
                {
                    var parentNode = umbracoHelper.GetById(nodeId);
                    nodeValue = parentNode.GetContentValue(alias);
                    if (!string.IsNullOrEmpty(nodeValue))
                    {
                        var parentListIds = nodeValue.Split(',').ToList().Select(int.Parse).Where(i => !nodeListIds.Contains(i)).ToList();
                        nodeListIds.AddRange(parentListIds);
                    }
                }
            }

            nodeListIds = nodeListIds.Distinct().Where(i => i > 0).ToList();
            return nodeListIds;
        }

        public static List<IPublishedContent> GetNodeListInherit(this IPublishedContent content, string alias)
        {
            var nodeList = new List<IPublishedContent>();
            var nodeListIds = content.GetNodeIdsInherit(alias);
            if (nodeListIds.Any())
            {
                nodeList.AddRange(nodeListIds.Select(i => umbracoHelper.GetById(i)));
            }
            return nodeList;
        }



        public static List<string> GetNodeStringsInherit(this IPublishedContent content, string alias)
        {
            var strings = new List<string>();
            if (content != null && content.Id > 0)
            {
                var parentIds = content.Path.Split(',').Select(int.Parse).Where(i => i > 0 && i != content.Id).Reverse().ToList();
                foreach (var nodeId in parentIds)
                {
                    var parentNode = umbracoHelper.GetById(nodeId);
                    var keywords = parentNode.GetContentValue(DocumentFields.metaKeywords.ToString());
                    if (!string.IsNullOrEmpty(keywords))
                    {
                        strings.Add(keywords);
                    }
                }
            }

            return strings;
        }

        public static List<string> GetNodeStringListInherit(this IPublishedContent content, string alias, bool distinctWords = false)
        {

            var valueList = new List<string>();

            if (content != null && content.Id > 0)
            {
                var parentIds = content.Path.Split(',').Select(int.Parse).Where(i => i > 0 && i != content.Id).ToList();
                foreach (var nodeId in parentIds)
                {
                    var parentNode = umbracoHelper.GetById(nodeId);
                    var parentNodeValue = parentNode.GetContentValue(alias);
                    if (!string.IsNullOrEmpty(parentNodeValue))
                    {
                        var parentListValues = parentNodeValue.Split(',').ToList().Select(i => i.Trim()).ToList();
                        valueList.AddRange(parentListValues);
                    }
                }

                //add the current node last
                var contentValue = content.GetContentValue(alias);
                var nodeLocalValues = new List<string>();

                if (contentValue != string.Empty)
                {
                    nodeLocalValues = contentValue.Split(',').ToList().Select(i => i.Trim()).ToList();
                }

                valueList.AddRange(nodeLocalValues);
            }

            if (distinctWords)
            {
                var distinctValues = new List<string>();
                foreach (var word in valueList)
                {
                    if (!distinctValues.Select(i => i.ToLower().Trim()).Contains(word.ToLower().Trim()))
                    {
                        distinctValues.Add(word);
                    }
                }
                valueList = distinctValues;
            }
            return valueList;
        }
        public static bool AllowAlias(this string nodeTypeAlias, string aliases)
        {
            var allow = aliases == "" || nodeTypeAlias.ValueExistsInCsv(aliases);
            return allow;
        }
        public static int FirstPublishedParentNodeId(int startNodeId)
        {
            var publishedNodeId = 0;

            var nodeId = startNodeId;
            var exit = false;

            while (!exit)
            {
                var content = contentService.GetById(nodeId);
                if (content.Published)
                {
                    exit = true;
                    publishedNodeId = nodeId;
                }
                else
                {
                    if (content.ParentId > 0)
                    {
                        nodeId = content.ParentId;
                    }
                    else
                    {
                        exit = true;
                    }
                }
            }
            return publishedNodeId;
        }
    }
}
