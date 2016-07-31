using System;
using System.Collections.Generic;
using System.Linq;

namespace IomerBase.U7.ApiClasses
{
    using global::Iomer.Umbraco.Extensions;
    using global::Iomer.Umbraco.Extensions.Transports;

    using IomerBase.Iomer.Umbraco.Extensions.Custom;
    //using IomerBase.U7.Data;
    using IomerBase.U7.DataObjects;


    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using RJP.MultiUrlPicker.Models;

    using Umbraco.Core;
    using Umbraco.Core.Services;

    using umbraco.NodeFactory;
    using umbraco.interfaces;
    using Amoura.Web.Data;

    public class UmbracoNodeApi
    {

        //If maxLevelForSubNav = -1 then don't pull subnodes
        //If maxLevelForSubNav = 0 then pull all subnodes
        public static UmbracoNode GetNodeData(string nodeid, string subNodeAliases = "", string maxlevelforsubvav = "")
        {
            //Node currentNode = Node.GetCurrent();
            var nodeId = Int32.Parse(nodeid);
            var maxLevelForSubNav = -1;
            if (maxlevelforsubvav != "")
            {
                maxLevelForSubNav = Int32.Parse(maxlevelforsubvav);
            }

            //more intuitive to adjust the level so that primary menu is level 1
            if (maxLevelForSubNav > 0)
            {
                maxLevelForSubNav++;
            }

            var currentNode = new Node(nodeId);
            //string jSon = "";
            //var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //jSon = serializer.Serialize(currentNode.Properties);

            var umbNode = UmbracoNodeData(currentNode, subNodeAliases, maxLevelForSubNav);
            //jSon = serializer.Serialize(umbNode);
            //return jSon;

            return umbNode;
        }
        //If maxLevelForSubNav = -1 then don't pull subnodes
        //If maxLevelForSubNav = 0 then pull all subnodes
        public static List<UmbracoNode> GetSubNodeData(string parentnodeid, string subNodeAliases = "", string maxlevelforsubnav = "")
        {
            //Node currentNode = Node.GetCurrent();

            var parentNodeId = Int32.Parse(parentnodeid);
            var maxLevelForSubNav = -1;
            if (maxlevelforsubnav != "")
            {
                maxLevelForSubNav = Int32.Parse(maxlevelforsubnav);
            }

            //more intuitive to adjust the level so that primary menu is level 1
            //if maxLevelForSubNav is less than or equal to 0, it will pull all nodes.
            if (maxLevelForSubNav > 0)
            {
                maxLevelForSubNav++;
            }

            var startNode = new Node(parentNodeId);
            //string jSon = "";
            //var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //jSon = serializer.Serialize(currentNode.Properties);

            //jSon = serializer.Serialize(umbNodes);
            //return jSon;

            //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            //jSon = serializer.Serialize(umbNodes);
            //return new MemoryStream(Encoding.UTF8.GetBytes(jSon));

            return startNode.ChildrenAsList.Where(i => i.NodeHidden() == false && i.NodeTypeAlias.AllowAlias(subNodeAliases)).Select(subNode => UmbracoNodeData(subNode, subNodeAliases, maxLevelForSubNav)).ToList();
        }

        #region Protected Methods

        static UmbracoNode UmbracoNodeData(INode node, string subNodeAliases = "", int subNodeLevel = -1)
        {
            var umbNode = new UmbracoNode();
            umbNode = LoadUmbracoNodeObject(umbNode, node, subNodeAliases, subNodeLevel);
            return umbNode;
        }


        static List<UmbracoNode> GetChildren(UmbracoNode umbNode, string subNodeAliases = "", int subNodeLevel = -1)
        {
            var umbReturn = new List<UmbracoNode>();
            var node = new Node(umbNode.Id);
            foreach (var childNode in node.ChildrenAsList.Where(i => i.NodeHidden() == false && i.NodeTypeAlias.AllowAlias(subNodeAliases)))
            {
                var umbSubNode = new UmbracoNode();
                umbSubNode = LoadUmbracoNodeObject(umbSubNode, childNode, subNodeAliases, subNodeLevel);
                if ((subNodeLevel > childNode.Level || subNodeLevel == 0)
                    && childNode.ChildrenAsList.Any(i => i.NodeHidden() == false && i.NodeTypeAlias.AllowAlias(subNodeAliases)))
                {
                    umbSubNode.SubNodes = GetChildren(umbSubNode);
                }
                umbReturn.Add(umbSubNode);
            }

            return umbReturn;
        }

        static UmbracoNode LoadUmbracoNodeObject(UmbracoNode umbNode, INode node, string subNodeAliases = "", int subNodeLevel = -1)
        {
            var returnNode = umbNode;
            returnNode.Id = node.Id;
            returnNode.Name = node.Name;
            returnNode.Alias = node.NodeTypeAlias;
            returnNode.Title = node.GetTitle();
            returnNode.Description = node.GetProperty(DocumentFields.itemDescription.ToString()) != null ? node.GetProperty(DocumentFields.itemDescription.ToString()).Value : "";
            returnNode.BodyText = node.GetProperty(DocumentFields.bodyText.ToString()) != null ? node.GetProperty(DocumentFields.bodyText.ToString()).Value : "";
            returnNode.Url = node.GetUrl();

            if (subNodeLevel == 0 || subNodeLevel > node.Level)
            {
                returnNode.SubNodes = GetChildren(returnNode, subNodeAliases, subNodeLevel);
            }

            return returnNode;
        }

        public static UrlPicker LoadImageLinkProperties(INode iNode, string urlPickerAlias)
        {
            //var node = new Node(iNode.Id);
            var urlPicker = MultiUrlPicker.GetUrlPicker(iNode.Id, urlPickerAlias);
            return urlPicker;
        }

        #endregion
    }
}