using PsscFinalProject.Data;
using PsscFinalProject.Events;
using PsscFinalProject.Events.ServiceBus;
using PsscFinalProject.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using PsscFinalProject.Data.Models;

namespace PsscFinalProject.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PsscDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories (uncomment and adjust as needed)
            // builder.Services.AddTransient<IGradesRepository, GradesRepository>();
            // builder.Services.AddTransient<IStudentsRepository, StudentsRepository>();
            // builder.Services.AddTransient<PublishExamWorkflow>();

            // Add Service Bus event sender
            builder.Services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();

            // Add Azure Service Bus client
            builder.Services.AddAzureClients(client =>
            {
                client.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
            });

            // Add HTTP client
            builder.Services.AddHttpClient();

            // Add controllers
            builder.Services.AddControllers();

            // Add Swagger (OpenAPI)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PsscFinalProject.Api", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            app.Run();
        }
    }
}
