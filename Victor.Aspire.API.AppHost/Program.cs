using Projects;

var builder = DistributedApplication.CreateBuilder(args);


var cache = builder
    .AddRedisContainer("redis");

builder
    .AddProject<Victor_Aspire_API_Service>("webapi")
    .WithReference(cache);

builder.Build().Run();
