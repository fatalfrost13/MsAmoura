using System.Runtime.Serialization;
//using uComponents.DataTypes.UrlPicker;


namespace Iomer.Umbraco.Extensions.Transports
{
    using RJP.MultiUrlPicker.Models;

    [DataContract]
    public class UrlPicker
    {
        [DataMember]
        public string Title { get; set; }
        //[DataMember]
        //public UrlPickerMode Mode { get; set; }
        [DataMember]
        public LinkType LinkType { get; set; }
        [DataMember]
        public int? NodeId { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public bool NewWindow { get; set; }
    }
}
