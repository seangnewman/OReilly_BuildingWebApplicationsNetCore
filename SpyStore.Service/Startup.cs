using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpyStore.DAL.EF;
using SpyStore.DAL.Initializers;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpyStore.Service
{
    public class Startup
    {
        public IWebHostEnvironment environment { get; set; }
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
           
           
        }

        public IConfiguration Configuration { get; }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
                 services.AddControllers()
                        .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
                        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);              // Specifies pascal naming

                services.AddCors(options => { options.AddPolicy("AllowAll", builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                     });

                services.AddDbContext<StoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SpyStore")));
                services.AddScoped<ICategoryRepo, CategoryRepo>();
                services.AddScoped<IProductRepo, ProductRepo>();
                services.AddScoped<ICustomerRepo, CustomerRepo>();
                services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
                services.AddScoped<IOrderRepo, OrderRepo>();
                services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            environment = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    StoreDataInitializer.InitializeData(app.ApplicationServices);
                }
                
            }

            app.UseCors("AllowAll");

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
