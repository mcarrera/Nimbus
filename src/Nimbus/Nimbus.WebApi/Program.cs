using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nimbus.Business.Services;
using Nimbus.Persistance.Data;
using Nimbus.Persistance.Repositories;

namespace Nimbus.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer(); builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nimbus.WebApi", Version = "v1" });
            });

            builder.Services.AddDbContext<NimbusDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // dependency injection
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<INimbusDbRepository, NimbusDbRepository>();

            builder.Services.AddScoped<IFileService, FileService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nimbus.WebApi v1"); });
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
