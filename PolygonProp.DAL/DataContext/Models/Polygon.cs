using NetTopologySuite.Geometries;

#nullable disable

namespace PolygonProp.DAL.DataContext.Models
{
    public partial class Polygon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Geometry Data { get; set; }
        public bool IsDeleted { get; set; }
    }
}
