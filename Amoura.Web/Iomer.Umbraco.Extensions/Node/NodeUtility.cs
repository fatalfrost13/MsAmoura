using System.Collections.Generic;
using umbraco.NodeFactory;
using umbraco.interfaces;
using umbraco.cms.businesslogic.media;
using Iomer.Extensions.Array;


namespace Iomer.Umbraco.Extensions
{
    using System.Linq;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web.Templates;

    using Iomer.Umbraco.Extensions.Razor;
    using System;
    using System.Configuration;

    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Services;

    //using IomerBase.U7.DataDefinition;

    /// <summary>
    /// Summary description for NodeUtility
    /// </summary>
    public static class NodeUtility
    {

        static readonly MediaService mediaService = new MediaService(new RepositoryFactory());

        public static IEnumerable<Node> AllDescendants(this Node node)
        {
            foreach (Node child in node.Children)
            {
                yield return child;

                foreach (var grandChild in child.AllDescendants())
                    yield return grandChild;
            }
        }

        public static string GetMediaPath(int nodeid)
        {
            //XPathNodeIterator xn = umbraco.library.GetMedia(nodeid, false);
            //xn.MoveNext();
            //xn.Current.MoveToFirst();
            //var path = xn.Current.Value;
            //var regstring = @"(/([A-Za-z0-9\\-_]*/)*([A-Za-z0-9-_]+.(jpg|bmp|gif)))([A-Za-z0-9]+(jpg|bmp|gif))*$";
            //var regex = new System.Text.RegularExpressions.Regex(regstring, RegexOptions.Compiled);
            //Match m = regex.Match(path);
            //if (m.Success)
            //{
            //    return m.Groups[1].Value;
            //}
          
            //var m = new Media(nodeid);
            //var path = m.getProperty("umbracoFile").Value.ToString();
            var m = mediaService.GetById(nodeid);
            var path = m.GetValue("umbracoFile").ToString();

            return path;
        }

        /// <summary>
        /// Finds the first child node with a matching node type alias
        /// </summary>
        /// <param name="node">Parent Node</param>
        /// <param name="nodeTypeAlias">node or nodes(csv)</param>
        /// <returns>first node id match</returns>
        public static int FindChildNodeId(this Node node, string nodeTypeAlias)
        {
            var nodeId = 0;
            var containerNode = node;
            bool[] blExit = { false };
            foreach (var childNode in containerNode.Children.Cast<Node>().Where(childNode => blExit[0] == false && childNode.NodeTypeAlias.ValueExistsInCsv(nodeTypeAlias)))
            {
                nodeId = childNode.Id;
                blExit[0] = true;
            }
            return nodeId;
        }

        /// <summary>
        /// Finds the first parent node with matching node type alias
        /// </summary>
        /// <param name="node">Start node</param>
        /// <param name="nodeTypeAlias">node or nodes(csv)</param>
        /// <returns>first container node id match</returns>
        public static int FindContainerNodeId(this Node node, string nodeTypeAlias)
        {
            var nodeId = 0;
            var documentNode = node;
            var blExit = false;
            while (blExit == false)
            {
                if (documentNode.NodeTypeAlias.ValueExistsInCsv(nodeTypeAlias))
                {
                    nodeId = documentNode.Id;
                    blExit = true;
                }
                if (documentNode.Parent == null)
                {
                    blExit = true;
                }
                else
                {
                    documentNode = new Node(documentNode.Parent.Id);
                }
            }

            return nodeId;
        }

        public static List<int> FindAllNodeIDs(this Node node, string nodeTypeAlias, List<int> collectionIDs = null)
        {
            if (collectionIDs == null)
            {
                collectionIDs = new List<int>();
            }

            if (node.NodeTypeAlias.ValueExistsInCsv(nodeTypeAlias) && node.Id > 0)
            {
                collectionIDs.Add(node.Id);
            }

            return node.ChildrenAsList.Cast<Node>().Aggregate(collectionIDs, (current, childNode) => childNode.FindAllNodeIDs(nodeTypeAlias, current));
        }

        /// <summary>
        /// Gets title of node
        /// if title doesn't exist, the it uses the node name
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Title as string</returns>
        public static string GetTitle(this INode node)
        {
            var strTitle = node.Name;

            var titleProperty = node.GetProperty(UmbracoFields.Title);
            var pageTitleProperty = node.GetProperty(UmbracoFields.PageTitle);
            var imageTitleProperty = node.GetProperty("imageTitle");
            if (titleProperty != null)
            {
                if (titleProperty.Value.Trim() != "")
                {
                    strTitle = titleProperty.Value;
                }
            }
            else if (pageTitleProperty != null)
            {
                if (pageTitleProperty.Value.Trim() != "")
                {
                    strTitle = pageTitleProperty.Value;
                }
            }
            else if (imageTitleProperty != null)
            {
                if (imageTitleProperty.Value.Trim() != "")
                {
                    strTitle = imageTitleProperty.Value;
                }
            }

            return strTitle;
        }

        /// <summary>
        /// Gets date field of node
        /// if title doesn't exist, the it uses the node created date
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Date as string</returns>
        public static string GetDate(this INode node, string dateFormat = "")
        {
            if (string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = ConfigurationManager.AppSettings["dateFormat"];
            }
            var date = node.CreateDate;

            var dateProperty = node.GetProperty(UmbracoFields.Date);
            if (dateProperty != null)
            {
                if (dateProperty.Value.Trim() != "")
                {
                    date = Convert.ToDateTime(dateProperty.Value);
                }
            }
            var strDate = date.ToString(dateFormat);
            return strDate;
        }

        /// <summary>
        /// Gets URL of the node. Doc types that don't have a URL need to have exception and handler
        /// </summary>
        /// <param name="node"> </param>
        /// <returns></returns>
        public static string GetUrl(this INode node, string alias = "")
        {
            string strUrl = "";
            //Node node = new Node(NodeID);
            if (node != null && node.Id > 0)
            {
                strUrl = node.Url;

                //check URL Property
                var linkAlias = alias != string.Empty ? alias : UmbracoFields.UrlPicker;
                var urlPickerProp = node.GetProperty(linkAlias);
                if (urlPickerProp != null && string.IsNullOrWhiteSpace(urlPickerProp.Value) == false)
                {
                    strUrl = RazorUtility.UrlPickerLink(urlPickerProp.Value, node, linkAlias, "Url");
                    if (strUrl.Trim() == "")
                    {
                        strUrl = node.Url;
                    }
                }
            }

            return strUrl;
        }

        public static string GetTarget(this INode node, string alias = "")
        {
            string strTarget = "_self";
            //Node node = new Node(NodeID);
            if (node != null && node.Id > 0)
            {
                //check URL Property
                var linkAlias = alias != string.Empty ? alias : UmbracoFields.UrlPicker;
                var urlPickerProp = node.GetProperty(linkAlias);
                if (urlPickerProp != null && string.IsNullOrWhiteSpace(urlPickerProp.Value) == false)
                {
                    strTarget = RazorUtility.UrlPickerLink(urlPickerProp.Value, node, linkAlias, "Target");
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

        public static bool NodeHidden(this INode node)
        {
            bool hidden = node.GetProperty(UmbracoFields.Hidden) != null && node.GetProperty(UmbracoFields.Hidden).Value == "1";

            return hidden;
        }

        public static bool AllowAlias(this string nodeTypeAlias, string aliases)
        {
            bool allow = aliases == "" || nodeTypeAlias.ValueExistsInCsv(aliases);
            return allow;
        }

        public static string GetNodeValue(this Node node, string alias)
        {
            var nodeValue = node.GetProperty(alias) != null ? node.GetProperty(alias).Value : string.Empty;
            return nodeValue;
        }

        public static string GetNodeValue(this IContent node, string alias)
        {
            var nodeValue = node.GetValue(alias) != null ? node.GetValue(alias).ToString().Trim() : string.Empty;
            return nodeValue;
        }

        public static string GetNodeValue(this IPublishedContent node, string alias)
        {
            var nodeValue = (node.GetProperty(alias) != null && node.GetProperty(alias).Value != null) ? node.GetProperty(alias).Value.ToString().Trim() : string.Empty;
            return nodeValue;
        }

        public static bool GetNodeBoolean(this Node node, string alias)
        {
            var boolValue = false;
            var nodeValue = node.GetNodeValue(alias);
            if (nodeValue.ToLower() == "1" || nodeValue.ToLower() == "true" || nodeValue.ToLower() == "yes")
            {
                boolValue = true;
            }
            return boolValue;
        }

        public static Guid GetNodeGuid(this Node node, string alias)
        {
            var guid = Guid.Empty;
            var nodeValue = node.GetNodeValue(alias);
            if (nodeValue != "")
            {
                Guid.TryParse(nodeValue, out guid);
            }
            return guid;
        }

        public static string FormatRichText(this string text)
        {
            var richText = text;
            richText = TemplateUtilities.ParseInternalLinks(richText);
            return richText;
        }

        public static List<Node> GetNodeList(this Node node, string alias)
        {
            var nodeValue = node.GetNodeValue(alias);
            var nodeList = new List<Node>();
            if (nodeValue != string.Empty)
            {
                nodeList = nodeValue.Split(',').ToList().Select(i => new Node(int.Parse(i))).ToList();
            }
            return nodeList.Where(i => i != null).ToList();
        } 
    }
}