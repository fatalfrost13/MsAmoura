
namespace IomerBase.Iomer.Umbraco.Extensions.Custom
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Iomer.Umbraco.Extensions;
    using global::Iomer.Umbraco.Extensions.Transports;

    using global::Umbraco.Core.Services;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using RJP.MultiUrlPicker.Models;

    using umbraco.NodeFactory;

    public static class MultiUrlPicker
    {

        public static UrlPicker GetUrlPicker(int nodeId, string alias)
        {
            var contentService = new ContentService();
            var node = contentService.GetById(nodeId);
            var urlPicker = new UrlPicker();
            var stringData = node.GetNodeValue(alias);
            var links = JsonConvert.DeserializeObject<JArray>(stringData);
            var firstLink = links != null ? links.FirstOrDefault() : null;

            if (firstLink != null)
            {
                var item = new Link(firstLink);
                var url = string.Empty;
                if (item.Type == LinkType.Content && item.Id != null && item.Id != 0)
                {
                    url = new Node(Int32.Parse(item.Id.ToString())).Url;
                }
                else if (item.Type == LinkType.Media && item.Id != null && item.Id != 0)
                {
                    url = NodeUtility.GetMediaPath(Int32.Parse(item.Id.ToString()));
                }
                else if (item.Type == LinkType.External)
                {
                    url = item.Url;
                }


                urlPicker = new UrlPicker
                {
                    Title = item.Name,
                    LinkType = item.Type,
                    NewWindow = item.Target == "_blank",
                    NodeId = item.Id,
                    //Url = uComponentsUtility.GetUrlFromUrlPicker(item),
                    Url = url
                };
            }
            return urlPicker;
        }

        public static List<UrlPicker> GetUrlPickerList(int nodeId, string alias)
        {
            var contentService = new ContentService();
            var node = contentService.GetById(nodeId);
            var urlPickerList = new List<UrlPicker>();
            var stringData = node.GetValue(alias).ToString();
            var links = JsonConvert.DeserializeObject<JArray>(stringData);

            if (links.Any())
            {
                //var multiUrls = JsonConvert.DeserializeObject<JArray>(multiImageLinkProp.Value).Cast<MultiUrls>().ToList();
                //var item = multiImageLinks.SingleOrDefault();
                //var item = UrlPickerState.Deserialize(jSonUrlPicker);
                foreach (var link in links)
                {
                    var item = new Link(link);
                    var url = string.Empty;
                    if (item.Id != null && item.Id != 0)
                    {
                        url = new Node(Int32.Parse(item.Id.ToString())).Url;
                    }
                    var urlPicker = new UrlPicker
                    {
                        Title = item.Name,
                        LinkType = item.Type,
                        NewWindow = item.Target == "_blank",
                        NodeId = item.Id,
                        //Url = uComponentsUtility.GetUrlFromUrlPicker(item),
                        Url = url
                    };
                    urlPickerList.Add(urlPicker);
                }
            }
            return urlPickerList;
        }
    }
}
