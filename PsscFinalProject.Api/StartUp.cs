//using PsscFinalProject.Data.Repositories;
//using PsscFinalProject.Domain.Repositories;
//using PsscFinalProject.Domain.Workflows;

//namespace PsscFinalProject.Api
//{
//    public class Startup
//    {
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllers();
//            services.AddScoped<PublishOrderWorkflow>();
//            services.AddScoped<IClientRepository, ClientRepository>();
//            services.AddScoped<IOrderRepository, OrderRepository>();
//            services.AddScoped<IProductRepository, ProductRepository>();

//            services.AddSwaggerGen();
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseRouting();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });
//        }
//    }
//}