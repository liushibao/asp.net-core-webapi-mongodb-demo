using Microsoft.Extensions.Options;
using MongoDB.Driver;

using System;
using WebApi.Models;

namespace WebApi.Services
{
    public class MongoDbService
    {
        public readonly IMongoCollection<User> UsersCollection;
        public readonly IMongoCollection<Gdp> GdpsCollection;
        public readonly IMongoCollection<Info> InfosCollection;
        public readonly IMongoCollection<SmsCodeCache> SmsCodeCachesCollection;
        public readonly IMongoCollection<WxToken> WxTokensCollection;
        public readonly IMongoCollection<GdpRes> GdpResCollection;
        public readonly IMongoCollection<InfoRes> InfoResCollection;
        public readonly IMongoDatabase Database;

        public MongoDbService()
        {
            var mongoClient = new MongoClient(EnvironmentConfig.Instance.MongoDbConnectionString);
            Database = mongoClient.GetDatabase("Demo");

            UsersCollection = Database.GetCollection<User>("users");
            GdpsCollection = Database.GetCollection<Gdp>("gdps");
            InfosCollection = Database.GetCollection<Info>("infos");
            SmsCodeCachesCollection = Database.GetCollection<SmsCodeCache>("smsCodeCaches");
            WxTokensCollection = Database.GetCollection<WxToken>("wxTokens");
            GdpResCollection = Database.GetCollection<GdpRes>("gdpRes");
            InfoResCollection = Database.GetCollection<InfoRes>("infoRes");

            WxTokensCollection.Indexes.CreateOne(new CreateIndexModel<WxToken>(
                new IndexKeysDefinitionBuilder<WxToken>().Ascending(t => t.CreatedAt),
                new CreateIndexOptions
                {
                    Name = "ExpireAt",
                    ExpireAfter = new TimeSpan(0, 2, 0)
                }));

            SmsCodeCachesCollection.Indexes.CreateOne(new CreateIndexModel<SmsCodeCache>(
                new IndexKeysDefinitionBuilder<SmsCodeCache>().Ascending(t => t.CreatedAt),
                new CreateIndexOptions
                {
                    Name = "ExpireAt",
                    ExpireAfter = new TimeSpan(0, 10, 0)
                }));

            GdpResCollection.Indexes.CreateOne(new CreateIndexModel<GdpRes>(
                new IndexKeysDefinitionBuilder<GdpRes>().Ascending(t => t.CreatedAt),
                new CreateIndexOptions
                {
                    Name = "ExpireAt",
                    ExpireAfter = new TimeSpan(24, 0, 0)
                }));

            InfoResCollection.Indexes.CreateOne(new CreateIndexModel<InfoRes>(
                new IndexKeysDefinitionBuilder<InfoRes>().Ascending(t => t.CreatedAt),
                new CreateIndexOptions
                {
                    Name = "ExpireAt",
                    ExpireAfter = new TimeSpan(0, 10, 0)
                }));

            UsersCollection.Indexes.CreateOne(new CreateIndexModel<User>(new IndexKeysDefinitionBuilder<User>().Ascending(t => t.WxOpenId)));
            UsersCollection.Indexes.CreateOne(new CreateIndexModel<User>(new IndexKeysDefinitionBuilder<User>().Ascending(t => t.Mob)));
        }
    }
}
