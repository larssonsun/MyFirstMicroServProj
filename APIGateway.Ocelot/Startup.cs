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
            // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJhcGlnYXRld2F5LmF1ZCIsImlzcyI6ImFwaWdhdGV3YXkuaXNzIiwiZXhwIjoxNjM2NTMxNTczLCJJc0Z1a2luVXNlciI6InllcyBpJ20gZnVja2luIHVzZXIifQ.W5mCBrPUYc1gx1L-HiUqxH2h0eJBoS5uEB5EQIXVO-0
            // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJHcmF2ZWwuQXBwU2VjcmV0QXVkaWVuY2UiLCJpc3MiOiJHcmF2ZWwuQXBwSXNzdWVyIiwiZXhwIjoxNjM2NTMxNTczLCJTdXBwbGllclVuaXRJZCI6IkVFN0IzREFBLTE3RDItNDMzMS05RjAwLTY5NzEwMTgwRjhGNSJ9.Tqu1bw_rxyfCdwq9ZErb-AQm4Z9SbG36HDpY9iH_W1U
            Console.WriteLine(Convert.ToBase64String((new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenGravelApp:Secret"]))).Key));

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
            })
             .AddJwtBearer("Gravel.App", jbo =>
            {
                jbo.RequireHttpsMetadata = false;
                jbo.SaveToken = true;
                jbo.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenGravelApp:Secret"])),
                    ValidIssuer = Configuration["TokenGravelApp:Issuer"],
                    ValidAudience = Configuration["TokenGravelApp:Audience"],

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
