using ExpenseManagementMVC.Configuration;
using ExpenseManagementMVC.DTO;
using ExpenseManagementMVC.Models;
using ExpenseManagementMVC.Repository;
using ExpenseManagementMVC.Services;
using ExpenseManagementMVC.Validators;
using ExpensesManagementMVC.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExpenseManagementMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ExpensesDbContext>(options => options.UseSqlServer(connString));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IValidator<UserSignUpDTO>, UserSignUpValidator>();
            builder.Services.AddScoped<IValidator<ExpenseInsertDTO>, ExpenseInsertValidator>();
            builder.Services.AddScoped<IValidator<ExpenseUpdateDTO>, ExpenseUpdateValidator>();
            builder.Services.AddScoped<IValidator<UserUpdatePasswordDTO>, UserUpdatePasswordValidator>();
            builder.Services.AddAutoMapper(typeof(MapperConfig));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/User/Login";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
