using Application.Shared.Helps;
using Application.UseCases.Orders.CreateOrder;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.SAP_Helps;
using Persistence.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAPI.SAP_Helps;

namespace Persistence.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigurePersistenceServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var connectionString = configuration.GetConnectionString("DevDB");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            });


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["AppSettings:JWTSecret"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            

            services.Configure<SapSettings>(configuration.GetSection("SapSettings"));

            services.AddTransient<SapSessionHandler>();
            services.AddMemoryCache();
            services.AddHttpClient("SAP", (sp, client) =>
            {
                var sap = sp.GetRequiredService<IOptions<SapSettings>>().Value;

                if (string.IsNullOrWhiteSpace(sap.BaseUrl))
                    throw new InvalidOperationException("SapSettings:BaseUrl is not configured.");

                // BaseAddress must end with "/" or the last segment (v1) gets dropped
                client.BaseAddress = new Uri(sap.BaseUrl.TrimEnd('/') + "/");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            })
            .AddHttpMessageHandler<SapSessionHandler>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(20));


            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(3));

            services.AddScoped<SapUserImporter>();
            services.AddHostedService<SapSqlQueryUserSeeder>();


            services.AddScoped<ISapClient, SapB1ServiceLayerClient>();
            services.AddScoped<IB1DashboardService, B1DashboardService>();
            services.AddMediatR(cfg =>
                                cfg.RegisterServicesFromAssembly(typeof(CreateB1OrderHandler).Assembly));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBaseUserRepository, AppUserRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            
            services.AddScoped<IOrdersCartRepository, OrdersCartRepository>();
            services.AddScoped<IOrderLineCartRepository, OrderLineCartRepository>();
            services.AddScoped<IItemExtensionRepository, ItemExtensionRepository>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAppSettingRepository, AppSettingRepository>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<SeedSettings>(configuration.GetSection("Seed"));




            services.AddScoped<IEmailService, EmailService>();

            services.AddHostedService<SapSqlQuerySeeder>();
        }
    }
}
