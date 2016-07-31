using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IomerBase.U7.DataObjects
{
    [DataContract]
    public struct UmbracoNode
    {
        [DataMember]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        public string BodyText { get; set; }
        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public List<UmbracoNode> SubNodes { get; set; }

    }
}