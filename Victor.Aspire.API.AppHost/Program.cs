using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Victor_Aspire_API_Service>("webapi");

builder.Build().Run();
