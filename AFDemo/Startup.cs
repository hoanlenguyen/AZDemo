using AFDemo.Data;
using AFDemo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(AFDemo.Startup))]

namespace AFDemo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //add db
            builder.Services.AddDbContext<MyDbContext>(
                options => options.UseSqlServer(Environment.GetEnvironmentVariable("DBConnectionString")));

            //add service
            builder.Services.AddScoped<IOrderService, OrderService>();
        }
    }
}