using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server.Features;
using NUnit.Framework;
using WebApplication1;

namespace TestProject1;

public class Tests
{
    private HttpClient _port80Client;
    private HttpClient _port82Client;

    [SetUp]
    public void Setup()
    {
        // Create application factories for master and utility services and corresponding HTTP clients
        var port80Factory = new CustomWebApplicationFactory(80, 82);
        _port80Client = port80Factory.CreateClient();
        port80Factory.Server.Features.Set<IServerAddressesFeature>(new ServerAddressesFeature {Addresses = {"http://localhost:80"}});

        var port82Factory = new CustomWebApplicationFactory(82, 80);
        _port82Client = port82Factory.CreateClient();
        port82Factory.Server.Features.Set<IServerAddressesFeature>(new ServerAddressesFeature {Addresses = {"http://localhost:82"}});

        // Mock dependency on utility service by replacing named HTTP client
        port80Factory.AddHttpClient(Constants.Fallback, _port82Client);
        port82Factory.AddHttpClient(Constants.Fallback, _port80Client);
    }

    [Test]
    public async Task Port80_says_hello()
    {
        var response = await _port80Client.GetAsync("hello");

        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("hello from http://localhost:80", content);
    }
    
    [Test]
    public async Task Port80_falls_back_to_82()
    {
        var response = await _port80Client.GetAsync("hello/fallback");

        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("hello from http://localhost:82", content);
    }
    
    [Test]
    public async Task Port82_says_hello()
    {
        var response = await _port82Client.GetAsync("hello");

        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("hello from http://localhost:82", content);
    }


    [Test]
    public async Task Port82_falls_back_to_80()
    {
        var response = await _port82Client.GetAsync("hello/fallback");

        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("hello from http://localhost:80", content);
    }
}