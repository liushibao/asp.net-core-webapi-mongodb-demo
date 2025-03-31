
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models
{
    public class Gdp
    {
        [BsonId]
        [BsonElement("_id")]
        public int Year { get; set; }
        public int Amount { get; set; }
        public double GrowthRate { get; set; }
    }
}
