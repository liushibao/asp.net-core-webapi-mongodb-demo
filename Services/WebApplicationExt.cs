
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public static class WebApplicationExt
    {
        public static async Task SeedDb(this WebApplication app)
        {
            var db = app.Services.GetService<MongoDbService>();

            if (db.InfosCollection.EstimatedDocumentCount() == 0)
            {
                var now = DateTime.Now;
                for (int i = 1; i <= 100; i++)
                  await  db.InfosCollection.InsertOneAsync(new Info
                    {
                        Name = $"信息{i}",
                        Detail = i % 3 == 0 ? $"这是一条比较长的信息{i}内容，用作展示两列不对称效果" : $"信息{i}内容",
                        Contact = $"某某{i}",
                        CreatedAt = now.AddMinutes(i)
                    });
            }

            // 插入gdp模拟数据
            if (db.GdpsCollection.EstimatedDocumentCount() == 0)
            {
                for (int i = 2001; i <= 2024; i++)
                 await   db.GdpsCollection.InsertOneAsync(new Gdp
                    {
                        Year = i,
                        Amount = 10000 + RandomNumberGenerator.GetInt32(10, 1000),
                        GrowthRate = RandomNumberGenerator.GetInt32(-10, 10) * 0.01,
                    });
            }
        }
    }

}
