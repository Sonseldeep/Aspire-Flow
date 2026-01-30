using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin(pgAdmin =>
    {
        pgAdmin.WithHostPort(5050);
    });

var database = postgres.AddDatabase("GameStore");

var redis = builder.AddRedis("redis");

// ===== COPY THIS KEYCLOAK SECTION =====
var keycloakPassword = builder.AddParameter("KeycloakPassword", secret: true, value: "admin");
var keycloak = builder.AddKeycloak("keycloak", adminPassword: keycloakPassword)
    .WithLifetime(ContainerLifetime.Persistent);

if (builder.ExecutionContext.IsRunMode)
{
    keycloak.WithDataVolume()
           .WithRealmImport("./realms");
}

var keycloakAuthority = ReferenceExpression.Create(
    $"{keycloak.GetEndpoint("http").Property(EndpointProperty.Url)}/realms/gamestore"
);
// ===== END KEYCLOAK SECTION =====

builder.AddProject<GameStore_Api>("gamestore-api")
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WithReference(redis)
    // ===== COPY THESE AUTH LINES =====
    .WithEnvironment("Auth__Authority", keycloakAuthority)
    .WithEnvironment("SWAGGERUI_CLIENTID", "gamestore-api")
    .WaitFor(database)
    .WaitFor(redis)
    .WaitFor(keycloak)
    .WithExternalHttpEndpoints();

builder.Build().Run();
