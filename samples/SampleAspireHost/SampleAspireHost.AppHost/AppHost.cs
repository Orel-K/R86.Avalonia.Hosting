var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.SampleApi>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.SampleHostedApplication>("avaloniafrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
