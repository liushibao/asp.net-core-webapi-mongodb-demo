using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;
using System.Xml.Linq;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("/api/auth/reg")]
public class AuthRegController : ControllerBase
{
    private readonly MongoDbService _db;
    private readonly IMobVerifier _smsClient;

    public AuthRegController(MongoDbService db, IMobVerifier smsClient)
    {
        _smsClient = smsClient;
        _db = db;
    }

    [HttpPost]
    [Route("SendSmsCode")]
    public async Task<IResult> SendSmsCode([FromBody] SendSmsCodeRequest req)
    {
        string userId = this.HttpContext?.User?.FindFirstValue("id");
        var builer = Builders<User>.Filter;
        var filter = builer.And(builer.Eq(t => t.Mob, req.Mob), builer.Ne(t => t.Id, userId));
        var count = await _db.UsersCollection.CountDocumentsAsync(filter);
        if (count > 0)
            return Results.BadRequest(new ProblemDetails() { Title = "手机号已绑定其他用户。" });
        var smsCode = new Random().Next(1000000, 9999999).ToString().Substring(0, 6);
        var result = await this._smsClient.SendSmsCode(req.Mob, [smsCode]);
        if (result == true)
        {
            await _db.SmsCodeCachesCollection.InsertOneAsync(new SmsCodeCache() { Mob = req.Mob, SmsCode = smsCode, UserId = userId, CreatedAt = DateTime.Now });
            return Results.Ok(new { isSuccess = true, expireSeconds = 600 });
        }
        else
        {
            return Results.Problem(new ProblemDetails() { Title = "短信服务异常" });
        }
    }

    [HttpPost]
    [Route("VerifySmsCode")]
    public async Task<IResult> VerifySmsCode(VerifySmsCodeRequest req)
    {
        string userId = this.HttpContext?.User?.FindFirstValue("id");
        var smsCodeCache = await _db.SmsCodeCachesCollection.Find(t => t.UserId == userId).FirstOrDefaultAsync();
        var isSuccess = smsCodeCache?.Mob == req.Mob && smsCodeCache?.SmsCode == req.SmsCode;
        if (isSuccess == true)
        {
            await _db.UsersCollection.UpdateOneAsync(Builders<User>.Filter.Eq(t => t.Id, userId), Builders<User>.Update.Set("Mob", smsCodeCache.Mob));
        }
        return Results.Ok(new { isSuccess });
    }

    [HttpPost]
    [Route("Reg")]
    public async Task<IResult> Reg(UserRegRequest req)
    {
        string userId = this.HttpContext?.User?.FindFirstValue("id");
        var builer = Builders<User>.Filter;
        var filter = builer.And(builer.Eq(t => t.Mob, req.Mob), builer.Eq(t => t.Id, userId));
        _db.UsersCollection.UpdateOne(filter, Builders<User>.Update.Set("Name", req.Name).Set("IdCardNumber", req.IdCardNumber).Set("Birthday", req.Birthday));
        var user = await this._db.UsersCollection.Find(t => t.Id == userId).FirstOrDefaultAsync();
        return Results.Ok(new { isSuccess = true, user });
    }

}
