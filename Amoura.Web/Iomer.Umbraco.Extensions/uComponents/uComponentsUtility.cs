using Iomer.Umbraco.Extensions.Transports;

using System;
using System.Collections.Generic;

using uComponents.DataTypes.MultiUrlPicker.Dto;
using uComponents.DataTypes.UrlPicker;
using uComponents.DataTypes.UrlPicker.Dto;
using umbraco.NodeFactory;

namespace Iomer.Umbraco.Extensions.uComponents
{
    using System.Linq;

    public static class uComponentsUtility
    {
        public static List<UrlPicker> DeserializeMultiUrlPicker(this string jSon)
        {
            var items = MultiUrlPickerState.Deserialize(jSon);

            return items.Items.Select(item => new UrlPicker
                {
                    Title = item.Title, 
                    Mode = item.Mode, 
                    NewWindow = item.NewWindow, 
                    NodeId = item.NodeId, 
                    Url = GetUrlFromUrlPicker(item)
                }).ToList();
        }

        public static string GetUrlFromUrlPicker(UrlPickerState item)
        {
            var url = "";
            if (item.Mode == UrlPickerMode.Content && item.NodeId != null)
            {
                var itemNode = new Node(Int32.Parse(item.NodeId.ToString()));
                url = itemNode.Url;
            }
            else if (item.Mode == UrlPickerMode.Media && item.NodeId != null)
            {
                url = NodeUtility.GetMediaPath(Int32.Parse(item.NodeId.ToString()));
            }
            else switch (item.Mode)
            {
                case UrlPickerMode.Upload:
                    url = item.Url;
                    break;
                case UrlPickerMode.URL:
                    url = item.Url;
                    break;
            }
            if (!string.IsNullOrEmpty(url) && url.IndexOf("http://") == -1 && url.IndexOf("https://") == -1 && item.Mode == UrlPickerMode.URL)
            {
                //if it's a manually typed URL, ensure that it starts with http://
                url = string.Format("{0}{1}","http://",url);
            }
            return url;
        }
    }
}
