using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;
namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IVillaRepository
    {
        //Generic implementation to retrieve all villas
        IEnumerable<Villa> GetAllVillas(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null);
        IEnumerable<Villa> GetVilla(Expression<Func<Villa, bool>> filter, string? includeProperties = null);
        void AddVilla(Villa entity);
        void UpdateVilla(Villa entity);
        void DeleteVilla(Villa entity);
        void Save();
    }
}
