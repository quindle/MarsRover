using MarsRoverApi.MongoDB;
using MarsRoverApi.MongoDB.Configuration;
using MarsRoverApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MarsRoverApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ServerConfig();
            Configuration.Bind(config);

            

            var marsRoverContext = new MarsRoverContext(config.MongoDB);
            var roverRepo = new RoverService(marsRoverContext);
            var planetRepo = new PlanetService(marsRoverContext);
            services.AddSingleton<IRoverService>(roverRepo);
            services.AddSingleton<IPlanetService>(planetRepo);
            services.AddSingleton<IMongoClient, MongoClient>();

            services.AddSwaggerGen(c =>
            {                
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "MarsRover API",
                    Version = "v1",
                    Description = "MarsRover API using MongoDB",
                });
            });


            services.AddMvc(); 

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });



            app.UseRouting();
                        
   

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}
