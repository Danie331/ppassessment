
using AutoMapper;
using Data = PolygonProp.DAL.DataContext.Models;
using Domain = PolygonProp.Model;

namespace PolygonProp.DAL.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.Polygon, Domain.Polygon>().ConvertUsing<DataToModelConvertor>();

            CreateMap<Domain.Polygon, Data.Polygon>().ConvertUsing<ModelToDataConvertor>();
        }
    }
}
