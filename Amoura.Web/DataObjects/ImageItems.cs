using System.Runtime.Serialization;
//using Iomer.Umbraco.Extensions.Transports;

namespace IomerBase.U7.DataObjects
{
    using global::Iomer.Umbraco.Extensions.Transports;

    [DataContract]
    public struct SlideshowItem
    {
        [DataMember]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        [DataMember]
        public string ImageTitle { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string ImageDescription { get; set; }
        [DataMember]
        public UrlPicker ImageLinkProperties { get; set; }
    }

    public struct PhotoGalleryItem
    {
        [DataMember]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        [DataMember]
        public string ImageTitle { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string ImageDescription { get; set; }
        //[DataMember]
        //public UrlPicker ImageLinkProperties { get; set; }
    }

}