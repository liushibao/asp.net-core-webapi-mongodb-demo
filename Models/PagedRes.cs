using MongoDB.Bson.Serialization.Attributes;


namespace WebApi.Models
{
    public abstract class PagedRes<T>
    {
        [BsonId]
        [BsonElement("_id")]
        public string Query { get; set; }
        public PagedData<T> Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PagedData<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int TotalPage { get; set; }
        public T[] Data { get; set; }
    }
}
