using AutoMapper;
using Domain = PolygonProp.Model;
using Dto = PolygonProp.PolygonApi.DtoModels;

namespace PolygonProp.PolygonApi.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dto.Polygon, Domain.Polygon>().ForMember(i => i.Id, r => r.MapFrom(s => int.Parse(s.Id)));
            CreateMap<Domain.Polygon, Dto.Polygon>().ForMember(i => i.Id, r => r.MapFrom(s => s.Id.ToString()));
        }
    }
}
