using System.Runtime.Serialization;
using Iomer.Extensions.Pagination;

namespace IomerBase.U7.DataObjects
{
    using System.Collections.Generic;

    [DataContract]
    public struct ListPager
    {
        [DataMember]
        public List<ListItem> ListItems { get; set; }
        [DataMember]
        public PaginationModel Pagination {get; set;}
    }

    [DataContract]
    public struct ListItem
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
        public string Date { get; set; }
        public string AllowComments { get; set; }
        public string RequiresApproval { get; set; }
    }

    [DataContract]
    public struct EventItem
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
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        public string Categories { get; set; }
        public string AllowComments { get; set; }
        public string RequiresApproval { get; set; }
    }
}