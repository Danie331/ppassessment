
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PolygonProp.DAL.DataContext;
using PolygonProp.DAL.Contract;
using PolygonProp.DAL.Core;

namespace PolygonProp.DAL
{
    public static class DependencyMapper
    {
        public static void RegisterDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPolygonDatastore, PolygonDatastore>();

            services.AddDbContext<PolygonContext>(options => options.UseSqlServer(configuration.GetConnectionString("PolygonDb"), x => x.UseNetTopologySuite()));
        }
    }
}
