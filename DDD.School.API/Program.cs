using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DDD.School.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var hostingEnvironment = context.HostingEnvironment;
                    builder.SetBasePath(hostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile("appsettings." + hostingEnvironment.EnvironmentName + ".json", true, true)
                        .AddEnvironmentVariables();
                }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
