using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    private readonly IServer _feature;
    private readonly IOptions<Ports> _ports;
    private readonly HttpClient _client;

    public HelloController(IServer server, IHttpClientFactory httpClientFactory, IOptions<Ports> ports)
    {
        _feature = server;
        _ports = ports;
        _client = httpClientFactory.CreateClient(Constants.Fallback);
    }

    [Route("")]
    public string hello()
    {
        var host = _feature.Features.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault() ?? "";
        host = host.Replace("[::]", "localhost");

        return "hello from " + host;
    }

    [Route(Constants.Fallback)]
    public async Task<string> fallback()
    {
        var httpResponseMessage = await _client.GetAsync($"http://localhost:{_ports.Value.Fallback}/hello");
        return await httpResponseMessage.Content.ReadAsStringAsync();
    }
}