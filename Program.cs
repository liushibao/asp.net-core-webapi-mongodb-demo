﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Text;
using WebApi.Models;
using WebApi.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure the cert and the key
builder.Configuration["Kestrel:Certificates:Default:Path"] = EnvironmentConfig.Instance.SslCertPath;
builder.Configuration["Kestrel:Certificates:Default:KeyPath"] = EnvironmentConfig.Instance.SslKeyPath;

// Add services to the container.
if (EnvironmentConfig.Instance.TencentcloudSecretKey != null)
    builder.Services.AddSingleton<IMobVerifier, TencentCloudMobVerifier>();
else
    builder.Services.AddSingleton<IMobVerifier, FakeMobVerifier>();

builder.Services.AddSingleton<IValidator<UserRegRequest>, UserRegRequestValidator>();

builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = EnvironmentConfig.Instance.Issuer,
         ValidAudience = EnvironmentConfig.Instance.Audience,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentConfig.Instance.JwtKey))
     };
 });

builder.Services.AddAuthorization();

builder.Services.AddControllers(options =>
{
    options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider(JsonNamingPolicy.CamelCase));
}).AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    opts.JsonSerializerOptions.Converters.Add(new ValidationProblemDetailsJsonConverter());
});

//builder.Services.Configure<JsonOptions>(opts =>
//{
//    opts.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
//    opts.SerializerOptions.Converters.Add(new ValidationProblemDetailsJsonConverter());
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async httpContext =>
    {
        var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
        if (pds == null
            || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
        {
            Console.WriteLine("未捕获的服务器错误");
            await httpContext.Response.WriteAsJsonAsync(new { isSuccess = false, message = "服务器错误" });
        }
    });
});

if (EnvironmentConfig.Instance.SslCertPath != null)
    app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.SeedDb();

app.MapControllers();


app.Run();

