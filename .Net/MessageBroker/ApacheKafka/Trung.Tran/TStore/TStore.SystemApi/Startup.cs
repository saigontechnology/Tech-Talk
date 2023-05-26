using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TStore.Shared.Constants;
using TStore.Shared.Services;
using TStore.SystemApi.Services;

namespace TStore.SystemApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRealtimeNotiService, RealtimeNotiService>();

            services.AddSingleton<IKafkaProducerManager, KafkaProducerManager>();

            services.AddSingleton<IMessagePublisher, KafkaMessagePublisher>();

            services.AddSingleton<IApplicationLog>(p =>
            {
                IRealtimeNotiService notiService = p.GetRequiredService<IRealtimeNotiService>();
                return new ApplicationLog(SystemConstants.ServiceIds.SystemApi, notiService);
            });

            services.AddSingleton<IMessageBrokerService, KafkaMessageBrokerService>();

            services.AddSwaggerGen();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(c =>
            {
                c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
            });

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
