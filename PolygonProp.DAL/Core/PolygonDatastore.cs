using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PolygonProp.DAL.Contract;
using PolygonProp.DAL.DataContext;
using PolygonProp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data = PolygonProp.DAL.DataContext.Models;

namespace PolygonProp.DAL.Core
{
    class PolygonDatastore : IPolygonDatastore
    {
        private readonly IMapper _mapper;
        private readonly PolygonContext _context;
        public PolygonDatastore(PolygonContext context,
                                IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(Polygon polygon)
        {
            try
            {
                var dto = _mapper.Map<Data.Polygon>(polygon);
                _context.Entry(dto).State = EntityState.Added;
                await _context.SaveChangesAsync();

                return dto.Id;
            }
            catch (Exception ex)
            {
                // Log ex
                throw;
            }
        }

        public async Task DeleteAllAsync()
        {
            try
            {
                foreach (var poly in _context.Polygons)
                {
                    poly.IsDeleted = true;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log ex
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var targetPoly = await _context.Polygons.FirstAsync(p => p.Id == id);
                targetPoly.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log ex
                throw;
            }
        }

        public async Task<IEnumerable<Polygon>> GetAllAsync()
        {
            try
            {
                var dataItems = await _context.Polygons.Where(p => !p.IsDeleted).AsNoTracking().ToListAsync();
                return _mapper.Map<IEnumerable<Polygon>>(dataItems);
            }
            catch (Exception ex)
            {
                // Log ex
                throw;
            }
        }

        public async Task UpdateAsync(Polygon polygon)
        {
            try
            {
                var polyRecord = await _context.Polygons.AsNoTracking().FirstAsync(p => p.Id == polygon.Id);
                var entity = _mapper.Map(polygon, polyRecord, typeof(Polygon), typeof(Data.Polygon));

                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log ex
                throw;
            }
        }
    }
}
