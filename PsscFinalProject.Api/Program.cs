using PsscFinalProject.Data;
using PsscFinalProject.Data.Repositories;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Events;
using PsscFinalProject.Events.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


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

            // Register repositories with the DI container
            builder.Services.AddScoped<PublishOrderWorkflow>();
            builder.Services.AddScoped<PublishBillingWorkflow>();
            builder.Services.AddScoped<PublishDeliveryWorkflow>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IDeliveryRepository,DeliveryRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IBillRepository, BillRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<CalculateOrderOperation>();

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
                    options.JsonSerializerOptions.WriteIndented = true; // Pretty-printed JSON
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
