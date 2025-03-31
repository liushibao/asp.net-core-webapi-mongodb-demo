using MongoDB.Bson.Serialization.Attributes;


namespace WebApi.Models
{
    public class GdpRes
    {
        [BsonId]
        [BsonElement("_id")]
        public string Query { get; set; }
        public Gdp[] Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
