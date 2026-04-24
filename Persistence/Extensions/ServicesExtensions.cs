using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Orders.CreateOrder;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Persistence.Repositories;

namespace Persistence.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigurePersistenceServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>()
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

            services.AddHttpClient("SAP", client =>
            {
                client.BaseAddress = new Uri("https://pmachanguane.ebs.local:50000/b1s/v1/");
                client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = new CookieContainer(),
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });
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

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
