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
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<PsscDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddTransient<IGradesRepository, GradesRepository>();
            //builder.Services.AddTransient<IStudentsRepository, StudentsRepository>();
            //builder.Services.AddTransient<PublishExamWorkflow>();
            //must put teh right repositories pls
            builder.Services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();

            builder.Services.AddAzureClients(client =>
            {
                client.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
            });

            builder.Services.AddHttpClient();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PsscFinalProject.Api", Version = "v1" });
            });


            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
