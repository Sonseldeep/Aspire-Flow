using GameStore.Api.Database;
using GameStore.Api.Endpoints;
using GameStore.Api.Extensions;
using GameStore.Api.Middleware;
using GameStore.Api.Repositories;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;



var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<GameStoreContext>("GameStore");
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.AddRedisDistributedCache("redis");

builder.Services.AddValidation();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.Authority = "http://localhost:8080/realms/gamestore";
//         options.Audience = "gamestore-api";
//         options.RequireHttpsMetadata = false; 
//         
//     });

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrationsAsync();
}


app.UseExceptionHandler();           
app.UseHttpsRedirection(); 



app.MapDefaultEndpoints();
app.MapGameEndpoints();

app.Run();





