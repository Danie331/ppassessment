using AutoMapper;
using GeoAPI.Geometries;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Data = PolygonProp.DAL.DataContext.Models;
using Domain = PolygonProp.Model;

namespace PolygonProp.DAL.AutoMapper
{
    public class ModelToDataConvertor : ITypeConverter<Domain.Polygon, Data.Polygon>
    {
        public Data.Polygon Convert(Domain.Polygon source, Data.Polygon destination, ResolutionContext context)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var geoJsonReader = new GeoJsonReader(geometryFactory, new Newtonsoft.Json.JsonSerializerSettings());
            var feature = geoJsonReader.Read<Feature>(source.Data);
            Polygon poly = (Polygon)feature.Geometry;
            if (!poly.Shell.IsCCW)
            {
                feature.Geometry = new Polygon((LinearRing)poly.Shell.Reverse(), geometryFactory);
            }
            else
            {
                feature.Geometry = new Polygon(poly.Shell, geometryFactory);
            }

            return new Data.Polygon
            {
                Id = source.Id,
                Name = source.Name,
                Data = feature.Geometry,
                IsDeleted = source.IsDeleted
            };
        }
    }
}
