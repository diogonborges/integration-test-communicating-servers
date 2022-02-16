using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1;

namespace TestProject1;

internal sealed class CustomWebApplicationFactory : WebApplicationFactory<Startup>
{
    private readonly int _mainPort;
    private readonly int _fallbackPort;

    public CustomWebApplicationFactory(int mainPort, int fallbackPort)
    {
        _mainPort = mainPort;
        _fallbackPort = fallbackPort;
    }

    // Contains replaced named HTTP clients
    private ConcurrentDictionary<string, HttpClient> HttpClients { get; } = new();

    // Add replaced named HTTP client
    public void AddHttpClient(string clientName, HttpClient client)
    {
        if (!HttpClients.TryAdd(clientName, client))
        {
            throw new InvalidOperationException($"HttpClient with name {clientName} is already added");
        }
    }

    // Replaces implementation of standard IHttpClientFactory interface with
    // custom one providing replaced HTTP clients from HttpClients dictionary 
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IHttpClientFactory>(new CustomHttpClientFactory(HttpClients));
        });
        
        builder.ConfigureAppConfiguration(_ =>
        {
            Environment.SetEnvironmentVariable("Ports__Main", _mainPort.ToString());
            Environment.SetEnvironmentVariable("Ports__Fallback", _fallbackPort.ToString());
        });
    }
}