using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolygonProp.DAL.Contract;
using System.Net.Mime;
using System.Threading.Tasks;
using Dto = PolygonProp.PolygonApi.DtoModels;
using Domain = PolygonProp.Model;
using System.Collections.Generic;

namespace PolygonProp.PolygonApi.Controllers
{
    [AllowAnonymous, Route("polygonservice"), ApiController]
    public class PolygonController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPolygonDatastore _dataService;

        public PolygonController(IPolygonDatastore dataService,
                                 IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpGet, Produces(typeof(IEnumerable<Dto.Polygon>))]
        public async Task<IActionResult> GetAll()
        {
            var polys = await _dataService.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<Dto.Polygon>>(polys));
        }

        [HttpPost, Consumes(MediaTypeNames.Application.Json), ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> AddPolygon(Dto.Polygon polyDto)
        {
            var polyModel = _mapper.Map<Domain.Polygon>(polyDto);
            var id = await _dataService.AddAsync(polyModel);

            return Ok(id);
        }

        [HttpPost, Route("{id}"), Consumes(MediaTypeNames.Application.Json), ProducesResponseType(201)]
        public async Task<IActionResult> UpdatePolygon(Dto.Polygon polyDto)
        {
            var polyModel = _mapper.Map<Domain.Polygon>(polyDto);
            await _dataService.UpdateAsync(polyModel);

            return Ok();
        }

        [HttpDelete, Route("{id}"), ProducesResponseType(200)]
        public async Task<IActionResult> DeletePolygon(int id)
        {
            await _dataService.DeleteAsync(id);

            return Ok();
        }

        [HttpDelete, ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAllPolygons()
        {
            await _dataService.DeleteAllAsync();

            return Ok();
        }
    }
}
