using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nimbus.Business.Common;
using Nimbus.Business.Services;
using Nimbus.Persistance.Data;
using Nimbus.Persistance.Repositories;
using Nimbus.WebApi.Middleware;

namespace Nimbus.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add configuration from appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Configure Swagger UI to use the API Key in requests
                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Enter your API key to access the API"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddDbContext<NimbusDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));



            // configuration and services
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            // dependency injection
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<INimbusDbRepository, NimbusDbRepository>();

            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IFolderService, FolderService>();

            var app = builder.Build();

            // middleware
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<ApiKeyMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nimbus.WebApi v1"); c.DefaultModelsExpandDepth(-1); });
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
