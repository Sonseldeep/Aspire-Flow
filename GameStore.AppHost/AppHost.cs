using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// -------------------- POSTGRES --------------------
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin(pgAdmin =>
    {
        pgAdmin.WithHostPort(5050); 
    });

var database = postgres.AddDatabase("GameStore");

// -------------------- REDIS --------------------
var redis = builder.AddRedis("redis");


builder.AddProject<GameStore_Api>("gamestore-api")
    .WithHttpHealthCheck("/health")
    .WithReference(database)       
    .WithReference(redis)          
    .WaitFor(database)             
    .WaitFor(redis)
    .WithExternalHttpEndpoints();  


builder.Build().Run();