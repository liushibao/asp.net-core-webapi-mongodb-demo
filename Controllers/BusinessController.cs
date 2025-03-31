using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/[controller]/public")]
public class BusinessController : ControllerBase
{
    private readonly MongoDbService _db;

    public BusinessController(MongoDbService db)
    {
        _db = db;
    }

    [HttpGet]
    [Route("GdpData")]
    public async Task<IResult> GdpData([FromQuery] GdpDataRequest req)
    {
        int yearStart = (int)req.YearStart;
        int yearEnd = (int)req.YearEnd;
        string query = $"{yearStart}-{yearEnd}";
        GdpRes cached = await _db.GdpResCollection.Find(t => t.Query == query).FirstOrDefaultAsync();
        if (cached == null)
        {
            Console.WriteLine("gdp cach not found");
            var data = await _db.GdpsCollection.Find(t => t.Year >= yearStart && t.Year <= yearEnd).ToListAsync();
            cached = new GdpRes() { Query = query, Data = data.ToArray(), CreatedAt = DateTime.Now };
            await _db.GdpResCollection.InsertOneAsync(cached);
        }
        return Results.Ok(cached.Data);
    }

    [HttpGet]
    [Route("Info")]
    public async Task<IResult> Info([FromQuery] InfoRequest req)
    {
        int pageNumber = (int)req.PageNumber;
        int pageSize = (int)req.PageSize;
        int skip = (pageNumber - 1) * pageSize;
        long totalCount = 0;
        string query = $"{pageNumber}-{pageSize}";
        InfoRes cached = await _db.InfoResCollection.Find(t => t.Query == query).FirstOrDefaultAsync();
        if (cached == null)
        {
            Console.WriteLine("info cach not found");
            totalCount = await _db.InfosCollection.EstimatedDocumentCountAsync();
            var data = await _db.InfosCollection.Find(t => true).Skip(skip).Limit(pageSize).ToListAsync();
            cached = new InfoRes()
            {
                Query = query,
                Data = new PagedData<Info>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPage = (int)Math.Ceiling((double)totalCount / pageSize),
                    Data = data.ToArray()
                },
                CreatedAt = DateTime.Now
            };
            await _db.InfoResCollection.InsertOneAsync(cached);
        }
        return Results.Ok(cached.Data);
    }

}
