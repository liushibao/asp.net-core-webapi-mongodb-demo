using MongoDB.Bson.Serialization.Attributes;


namespace WebApi.Models
{
     public class SmsCodeCache
    {
        [BsonId]
        [BsonElement("_id")]
        public string UserId { get; set; }
        public string Mob { get; set; }
        public string SmsCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
