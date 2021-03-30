using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolygonProp.DAL;
using PolygonProp.PolygonApi.Middleware;

namespace PolygonProp.PolygonApi
{
    public class Startup
    {
        private const string _corsDefault = "CorsTesting";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterDAL(Configuration);

            var allowedOrigins = Configuration.GetSection("AllowedOrigins").Value;
            services.AddCors(options =>
            {
                options.AddPolicy(_corsDefault,
                builder =>
                {
                    builder.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddControllers()
                    .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                    .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddAutoMapper(GetType().Assembly, typeof(DAL.AutoMapper.AutoMapperProfile).Assembly);

            services.AddSwaggerDocument(settings => settings.Title = "Assessment");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseCors(_corsDefault);

            app.UseRouting();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
