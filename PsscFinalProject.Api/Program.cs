using PsscFinalProject.Data;
using PsscFinalProject.Events.ServiceBus;
using PsscFinalProject.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Data.Repositories;
using PsscFinalProject.Domain.Repositories;


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

            // Register the repositories with the DI container
            //builder.Services.AddTransient<IOrderRepository, OrderRepository>(); // Register IOrderRepository
            //builder.Services.AddTransient<IClientRepository, ClientRepository>(); // Register IClientRepository
            //builder.Services.AddTransient<TakeOrderWorkflow>(); // Register TakeOrderWorkflow

            // Add Service Bus event sender
            builder.Services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();

            // Add Azure Service Bus client
            builder.Services.AddAzureClients(client =>
            {
                client.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
            });

            // Add HTTP client
            builder.Services.AddHttpClient();

            // Add controllers with JSON settings
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Enable circular reference handling using ReferenceHandler.Preserve
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.WriteIndented = true; // Optional, if you want pretty-printed JSON
                });

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