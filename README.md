
Demonstration of Net6 Integration tests with multiple servers (WebApplicationFactory) that communicate to each other via a fallback mechanism

This Web API has one controller with two methods:
 - **hello** - replies with a message that contains the address where it is running from
 - **fallback** - uses an HttpClient to do an HttpRequest to _hello_ endpoint in the fallback server

(Credit for https://github.com/GennadyGS for providing the basis for what I was going for)
