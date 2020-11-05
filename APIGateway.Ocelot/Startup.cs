using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace APIGateway.Ocelot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // IssuerSigningKey
            Console.WriteLine("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJhcGlnYXRld2F5LmF1ZCIsImlzcyI6ImFwaWdhdGV3YXkuaXNzIiwiZXhwIjoxNjM2MDkzODYwLCJJc0Z1a2luVXNlciI6InllcyBpJ20gZnVja2luIHVzZXIifQ." +
            Convert.ToBase64String((new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenManagement:Secret"]))).Key));

            services.AddAuthentication()
            .AddJwtBearer("scetiJwtAuth", jbo =>
            {
                jbo.RequireHttpsMetadata = false;
                jbo.SaveToken = true;
                jbo.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenManagement:Secret"])),
                    ValidIssuer = Configuration["TokenManagement:Issuer"],
                    ValidAudience = Configuration["TokenManagement:Audience"],

                };
            });

            services.AddOcelot()
            .AddConsul() // 服务发现（结合consul）
            .AddCacheManager(x => x.WithDictionaryHandle()); // 服务治理 缓存
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot().Wait();
        }
    }
}
