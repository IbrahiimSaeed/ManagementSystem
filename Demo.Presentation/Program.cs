using Demo.BusinessLogic.Mappings;
using Demo.BusinessLogic.Services.Classes;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.DataAccess.Data.Contexts;
using Demo.DataAccess.Data.Repositories.Classes;
using Demo.DataAccess.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); //Client Side Validation
            });
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //DI for DbContext
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
                options.UseLazyLoadingProxies();
            });
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();//DI
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();//DI
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();//DI
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();//DI
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();//DI
            builder.Services.AddAutoMapper(Mapping => Mapping.AddProfile(new MappingProfile()));//Auto Mapper

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


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
