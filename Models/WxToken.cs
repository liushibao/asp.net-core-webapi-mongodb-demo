using MongoDB.Bson.Serialization.Attributes;


namespace WebApi.Models
{
    public class WxToken
    {
        [BsonId]
        [BsonElement("_id")]
        public string openid { get; set; }
        public string accessToken { get; set; }
        public long expiresIn { get; set; }
        public string refreshToken { get; set; }
        public string scope { get; set; }
        public long isSnapshotuser { get; set; }
        public string unionid { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
