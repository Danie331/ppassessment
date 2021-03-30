using PolygonProp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolygonProp.DAL.Contract
{
    public interface IPolygonDatastore
    {
        Task<int> AddAsync(Polygon polygon);
        Task UpdateAsync(Polygon polygon);
        Task<IEnumerable<Polygon>> GetAllAsync();
        Task DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}
