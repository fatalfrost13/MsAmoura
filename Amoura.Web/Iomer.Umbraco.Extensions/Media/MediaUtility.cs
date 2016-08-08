using Umbraco.Web;
using Umbraco.Core;

namespace Iomer.Umbraco.Extensions.Media
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Amoura.Web.Helpers;

    using global::Umbraco.Core.Models;

    using Content;
    using Newtonsoft.Json;
    using global::Umbraco.Web.Models;
    using Amoura.Web.Helpers;
    public static class MediaUtility
    {
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
        public static string GetMediaPath(int nodeid, string propertyAlias = "umbracoFile")
        {
            var path = string.Empty;
            var mediaService = ApplicationContext.Current.Services.MediaService;
            if (nodeid > 0)
            {
                var m = mediaService.GetById(nodeid);

                if (m != null && m.Id > 0 && m.HasProperty(propertyAlias))
                {
                    path = m.GetValue(propertyAlias) != null ? m.GetValue(propertyAlias).ToString() : string.Empty;
                }
            }
            return path;
        }
        public static string GetMediaCropUrl(this IPublishedContent node, string alias, string cropAlias)
        {
            var cropUrl = string.Empty;
            var jsonValue = node.GetContentValue(alias);

            if (!string.IsNullOrEmpty(jsonValue))
            {
                var publishedContent = umbracoHelper.GetById(node.Id);
                cropUrl = publishedContent != null ? publishedContent.GetCropUrl(alias, cropAlias) : string.Empty;
            }

            return cropUrl;
        }

        public static string GetMediaCropUrl(this IMedia media, string alias, string cropAlias)
        {
            var cropUrl = string.Empty;
            var jsonValue = media.GetMediaValue(alias);

            if (!string.IsNullOrEmpty(jsonValue))
            {
                var imageCrops = JsonConvert.DeserializeObject<ImageCropDataSet>(jsonValue);
                cropUrl = imageCrops.Src;
                var crop = imageCrops.Crops.Where(i => i.Alias == cropAlias).SingleOrDefault();
                if (crop != null)
                {
                    cropUrl = $"{cropUrl}?width={crop.Width}&height={crop.Height}";
                }
            }

            return cropUrl;
        }

        public static ImageCropData GetMediaCrop(this IMedia media, string alias, string cropAlias)
        {
            var crop = new ImageCropData();
            var cropUrl = string.Empty;
            var jsonValue = media.GetMediaValue(alias);

            if (!string.IsNullOrEmpty(jsonValue))
            {
                var imageCrops = JsonConvert.DeserializeObject<ImageCropDataSet>(jsonValue);
                cropUrl = imageCrops.Src;
                crop = imageCrops.Crops.Where(i => i.Alias == cropAlias).SingleOrDefault();
            }
            return crop;
        }

        public static List<IMedia> GetMediaList(this IPublishedContent content, string alias)
        {
            var mediaService = ApplicationContext.Current.Services.MediaService;
            var nodeValue = content.GetContentValue(alias);
            var mediaList = new List<IMedia>();

            if (nodeValue != string.Empty)
            {
                mediaList = nodeValue.Split(',').ToList().Select(i => mediaService.GetById(int.Parse(i))).ToList();
            }
            return mediaList.Where(i => i != null && i.Id > 0).ToList();
        }

        public static string GetMediaValue(this IMedia media, string alias)
        {
            var mediaValue = (media.GetValue(alias) != null && media.GetValue(alias) != null) ? media.GetValue(alias).ToString().Trim() : string.Empty;
            return mediaValue;
        }
        public static IMedia GetMedia(this IPublishedContent content, string alias)
        {
            var mediaService = ApplicationContext.Current.Services.MediaService;
            var nodeValue = content.Id > 0 ? content.GetContentValue(alias) : string.Empty;
            var mediaId = !string.IsNullOrEmpty(nodeValue) ? int.Parse(nodeValue) : 0;
            var media = mediaId > 0 ? mediaService.GetById(mediaId) : null;
            return media;
        }

        public static string GetNodeMediaUrl(this IPublishedContent node, string alias)
        {
            var image = node.GetContentValue(alias);
            var imageUrl = string.Empty;
            if (!string.IsNullOrEmpty(image))
            {
                int imageId;
                int.TryParse(image, out imageId);
                if (imageId > 0)
                {
                    imageUrl = GetMediaPath(imageId);
                }
            }
            return imageUrl;
        }
    }
}