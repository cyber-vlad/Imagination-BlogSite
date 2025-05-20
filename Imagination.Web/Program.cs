using FluentValidation;
using FluentValidation.AspNetCore;
using Imagination.Application.DTOs;
using Imagination.Application.Features.Account.Commands.Login;
using Imagination.Application.Interfaces;
using Imagination.Application.Validation;
using Imagination.Infrastructure.Services;
using Imagination.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Imagination.Application.Features.Account.Commands.Register;
using Microsoft.Extensions.DependencyInjection;
using Imagination.Application.Extensions;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Hosting;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using Imagination.Infrastructure.Extensions;
using Imagination.Persistence.Extensions;
using Imagination.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ReflectionIT.Mvc.Paging;

namespace Imagination.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer();
            builder.Services.AddPersistenceLayer(builder.Configuration);
            
            builder.Services.AddAuthentication("Cookies")
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; 
                    options.AccessDeniedPath = "/Account/Login"; 
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            using (var scope = app.Services.CreateScope() )
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "postDetails",
                pattern: "Post/{id}",
                defaults: new { controller = "Post", action = "Details" });

            app.Run();
        }
    }
}
