using AutoMapper;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Data = PolygonProp.DAL.DataContext.Models;
using Domain = PolygonProp.Model;

namespace PolygonProp.DAL.AutoMapper
{
    public class DataToModelConvertor : ITypeConverter<Data.Polygon, Domain.Polygon>
    {
        public Domain.Polygon Convert(Data.Polygon source, Domain.Polygon destination, ResolutionContext context)
        {
            var wktReader = new WKTReader();
            var geom = wktReader.Read(source.Data.AsText());
            var sb = new StringBuilder();
            var serializer = GeoJsonSerializer.Create();
            var feature = new Feature(geom, new AttributesTable(new[]
            {
                    new KeyValuePair<string, object>("Id", source.Id),
                    new KeyValuePair<string, object>("Name", source.Name)
            }));
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
                serializer.Serialize(sw, feature);

            return new Domain.Polygon
            {
                Id = source.Id,
                Name = source.Name,
                Data = sb.ToString(),
                Wkt = source.Data.AsText(),
                IsDeleted = source.IsDeleted
            };
        }
    }
}
