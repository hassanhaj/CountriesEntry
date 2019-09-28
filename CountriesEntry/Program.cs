using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Serilog;
using System;
using System.IO;

namespace CountriesEntry
{
    public class Program
    {
        public Program()
        {

        }

        public static IConfiguration Configuration;

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  //  AppContext.BaseDirectory
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .Build();

                Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                     // .WriteTo.Console()              
                     //  .WriteTo.MSSqlServer("Server=.;Database=CountriesDB;integrated security=true", "serilog")
                   .CreateLogger();


            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               .UseSerilog()
               .UseStartup<Startup>();
    }
}
