using AFDemo.Data;
using AFDemo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(AFDemo.Startup))]
namespace AFDemo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                         .AddJsonFile("host.json", optional: true, reloadOnChange: true)
                         .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables()
                         .Build();

            //add db
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlite(/*config["ConnectionStrings:DefaultConnection"]*/"Data Source=Data\\Database.db"));                    

            //add service
            builder.Services.AddScoped<IOrderService, OrderService>();
        }
    }
}