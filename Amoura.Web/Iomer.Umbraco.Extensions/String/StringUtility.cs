using Iomer.Extensions.String;
using Iomer.Umbraco.Extensions.Razor;

using System.Linq;

using umbraco.NodeFactory;

namespace Iomer.Umbraco.Extensions.String
{
    public static class StringUtility
    {
        public static string FormatTitle(this int nodeId)
        {
            var node = new Node(nodeId);
            var title = node.GetTitle();
            var target = string.Empty;
            var strLink = string.Empty;

            //var urlPickerProperty = node.GetProperty(UmbracoFields.UrlPicker);
            //if (urlPickerProperty != null)
            //{
            //    var navNode = new Node(node.Id);
            //    var urlPicker = urlPickerProperty.Value;
            //    if (string.IsNullOrWhiteSpace(urlPicker) == false)
            //    {
            //        strLink = RazorUtility.UrlPickerLink(urlPicker, navNode);
            //    }
            //}
            if (string.IsNullOrWhiteSpace(strLink))
            {
                var path = node.Url;
                strLink = string.Format("<a href=\"{0}\"{1}>{2}</a>", path, target, title);
            }
            return strLink;
        }


        public static string SearchDescription(this int nodeId, string descriptionFields, int maxCharLength)
        {
            var sNode = new Node(nodeId);
            string strDescription = string.Empty;
            descriptionFields = descriptionFields.Replace(", ", ",");

            if (!string.IsNullOrWhiteSpace(descriptionFields))
            {
                var descFields = descriptionFields.Split(',').ToArray();
                foreach (var descProp in from item in descFields 
                                         where !string.IsNullOrWhiteSpace(item) 
                                         select sNode.GetProperty(item) into descProp where descProp != null where !string.IsNullOrWhiteSpace(descProp.Value.RemoveHTML()) select descProp)
                {
                    strDescription = descProp.Value.Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(strDescription))
            {
                var bodyText = sNode.GetProperty(UmbracoFields.BodyText);
                if (bodyText != null)
                {
                    if (!string.IsNullOrWhiteSpace(bodyText.Value.Trim()))
                    {
                        strDescription = bodyText.Value;
                    }
                }
            }

            strDescription = strDescription.RemoveHTML();
            if (!string.IsNullOrWhiteSpace(strDescription) && strDescription.Length >= maxCharLength && maxCharLength > 0)
            {
                strDescription = strDescription.Substring(0, maxCharLength);
                strDescription += " ...";
            }

            return strDescription;
        }

    }
}
