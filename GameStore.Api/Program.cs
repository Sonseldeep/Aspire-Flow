using GameStore.Api.Database;
using GameStore.Api.Endpoints;
using GameStore.Api.Extensions;
using GameStore.Api.Middleware;
using GameStore.Api.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<GameStoreContext>("GameStore");
builder.Services.AddScoped<IGameRepository,GameRepository>();

// builder.Services.AddPresistence(builder.Configuration);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrationsAsync();
}

app.MapDefaultEndpoints();
app.MapGameEndpoints();
app.UseHttpsRedirection();
app.UseExceptionHandler();



app.Run();

