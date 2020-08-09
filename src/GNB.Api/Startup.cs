using System;
using System.IO;
using System.Reflection;
using System.Text;
using GNB.Infrastructure.Capabilities;
using GNB.Data;
using GNB.Services;
using GNB.Services.Mappings;
using GNB.Services.QuietStone;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GNB.Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GNB API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
                
            MapsterConfig.Configure();

            services.AddServices(cfg => Configuration.GetSection("QuietStoneConfig").Bind(cfg));

            services.AddData();

            services.AddDbContext<GNBDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly(typeof(GNBDbContext).Assembly.FullName)));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(app => app.Use(async (context, next) =>
            {
                var ex = context.Features.Get<IExceptionHandlerFeature>();

                context.Response.ContentType = "application/json";

                if (ex.Error is GNBException exception)
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        exception.Code,
                        exception.Message
                    }), Encoding.UTF8).ConfigureAwait(false);
                }
                else
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        Code = ErrorCode.UnexpectedError,
                        Message = "Ups, something went wrong. It's not you, it's us."
                    }), Encoding.UTF8).ConfigureAwait(false);
            }));

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GNB API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
