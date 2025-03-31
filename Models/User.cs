
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Models
{
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string WxOpenId { get; set; }
        public string? Name { get; set; }
        public string? Mob { get; set; }
        public string? IdCardNumber { get; set; }
        public string? Birthday { get; set; }
    }


}
