var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AIDemo_ApiService>("apiservice")
    .WithUrlForEndpoint("https", url => 
        {
            url.DisplayText = "Scalar UI";
            url.Url += "/scalar";
        })
    .WithHttpHealthCheck("/health");

builder.Build().Run();
