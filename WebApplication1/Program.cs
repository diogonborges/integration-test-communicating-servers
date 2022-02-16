using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace WebApplication1
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((ctx, options) =>
                    {
                        var port = int.Parse(ctx.Configuration.GetSection("Ports").GetSection("Main").Value);

                        options.ListenAnyIP(port, listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}