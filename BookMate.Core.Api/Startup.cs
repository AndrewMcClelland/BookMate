// -----------------------------------
// Copyright (c) Andrew McClelland.
// -----------------------------------

using System.Text.Json.Serialization;
using BookMate.Core.Api.Brokers.BookingSystems.ForeUpSoftwareBookingSystems;
using BookMate.Core.Api.Brokers.Loggings;
using BookMate.Core.Api.Services.Foundations.ForeUpSoftware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BookMate.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddLogging();
            AddBrokers(services);
            AddServices(services);

            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Title = "BookMate.Core.Api",
                        Version = "v1"
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                        name: "BookMate.Core.Api v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IForeUpSoftwareBookingSystemBroker, ForeUpSoftwareBookingSystemBroker>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IForeUpSoftwareService, ForeUpSoftwareService>();
        }
    }
}