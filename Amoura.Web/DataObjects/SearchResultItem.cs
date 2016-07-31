using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IomerBase.U7.DataObjects
{
    using global::Iomer.Extensions.Pagination;

    [DataContract]
    public struct SearchResultItemPager
    {
        [DataMember]
        public List<SearchResultItem> SearchResultItems { get; set; }
        [DataMember]
        public PaginationModel Pagination { get; set; }
    }

    [DataContract]
    public struct SearchResultItem
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Score { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}