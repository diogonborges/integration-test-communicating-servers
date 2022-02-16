using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TestProject1;

internal class CustomHttpClientFactory : IHttpClientFactory
{
    // Takes dictionary storing named HTTP clients in constructor
    public CustomHttpClientFactory(IReadOnlyDictionary<string, HttpClient> httpClients)
    {
        HttpClients = httpClients;
    }

    private IReadOnlyDictionary<string, HttpClient> HttpClients { get; }

    // Provides named HTTP client from dictionary
    public HttpClient CreateClient(string name)
    {
        return HttpClients.GetValueOrDefault(name) ?? throw new InvalidOperationException($"HTTP client is not found for client with name {name}");
    }
}