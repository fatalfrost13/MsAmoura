//using Iomer.Umbraco.Extensions.Transports;
//using Iomer.Umbraco.Extensions.uComponents;

//using uComponents.DataTypes.UrlPicker.Dto;
using umbraco.interfaces;

namespace Iomer.Umbraco.Extensions.Razor
{
    using global::Umbraco.Core.Models;
    using IomerBase.Iomer.Umbraco.Extensions.Custom;
    using System;

    public static class RazorUtility
    {
        public static string UrlPickerLink(string jSonUrlPicker, INode navNode, string urlPickerAlias, string property = "")
        {
            var strTitle = "";
            var strTarget = "";

            var navTitle = navNode.GetProperty(UmbracoFields.PageTitle);
            if (navTitle != null)
            {
                strTitle = navTitle.Value;
            }

            //Array arr = csvURLPicker.Split(',');
            var urlPicker = MultiUrlPicker.GetUrlPicker(navNode.Id, urlPickerAlias);

            //0 = Link Type
            //1 = Open new window
            //2 = node ID if applicable
            //3 = link url
            //4 = link title

            var newWindow = urlPicker.NewWindow;

            if (newWindow)
            {
                strTarget = " target=\"_blank\"";
            }

            var strUrl = urlPicker.Url;

            if (!string.IsNullOrEmpty(urlPicker.Title))
            {
                strTitle = urlPicker.Title;
            }

            var strLink = String.Format("<a href=\"{0}\"{1}>{2}</a>", strUrl, strTarget, strTitle);


            if (property != "")
            {
                switch (property)
                {
                    case "Url":
                        strLink = strUrl;
                        break;
                    case "Title":
                        strLink = strTitle;
                        break;
                    case "Target":
                        strLink = strTarget;
                        break;
                }
            }

            return strLink;
        }

        public static string UrlPickerLink(string jSonUrlPicker, IPublishedContent navContent, string urlPickerAlias, string property = "")
        {
            var strTitle = "";
            var strTarget = "";

            var navTitle = navContent.GetProperty(UmbracoFields.PageTitle);
            if (navTitle != null)
            {
                strTitle = navTitle.Value.ToString();
            }

            //Array arr = csvURLPicker.Split(',');
            var urlPicker = MultiUrlPicker.GetUrlPicker(navContent.Id, urlPickerAlias);

            //0 = Link Type
            //1 = Open new window
            //2 = node ID if applicable
            //3 = link url
            //4 = link title

            var newWindow = urlPicker.NewWindow;

            if (newWindow)
            {
                strTarget = " target=\"_blank\"";
            }

            var strUrl = urlPicker.Url;

            if (!string.IsNullOrEmpty(urlPicker.Title))
            {
                strTitle = urlPicker.Title;
            }

            var strLink = $"<a href=\"{strUrl}\"{strTarget}>{strTitle}</a>";


            if (property != "")
            {
                switch (property)
                {
                    case "Url":
                        strLink = strUrl;
                        break;
                    case "Title":
                        strLink = strTitle;
                        break;
                    case "Target":
                        strLink = strTarget;
                        break;
                }
            }

            return strLink;
        }

        public static string TruncateText(this string fullText, string replaceEnd, int maxLength = 0)
        {
            string truncatedText = fullText;
            if (maxLength != 0)
            {
                if (fullText.Length > maxLength)
                {
                    truncatedText = fullText.Substring(0, maxLength);
                    truncatedText = truncatedText.Substring(0, truncatedText.LastIndexOf(" "));
                    truncatedText += replaceEnd;
                }
            }
            return truncatedText;
        }

        public static string DisplayField(this string value, string label = "")
        {
            string returnValue = "";

            if (!String.IsNullOrEmpty(value))
            {
                if (label != "")
                {
                    returnValue = String.Format("<strong>{0}:</strong> {1}", label, value);
                }
                else
                {
                    returnValue = value;
                }
                returnValue = returnValue.Replace("\r\n", "<br />").Replace("\n", "<br />");
            }

            return returnValue;
        }

        public static string DisplayCustomField(this string value, string format = "")
        {
            string returnValue = "";

            if (!String.IsNullOrEmpty(value))
            {
                if (format != "")
                {
                    returnValue = String.Format(format, value);
                }
                returnValue = returnValue.Replace("\r\n", "<br />").Replace("\n", "<br />");
            }

            return returnValue;
        }
    }
}
