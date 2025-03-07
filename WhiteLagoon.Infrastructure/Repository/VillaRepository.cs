using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        public void AddVilla(Villa entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteVilla(Villa entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Villa> GetAllVillas(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Villa> GetVilla(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateVilla(Villa entity)
        {
            throw new NotImplementedException();
        }
    }
}
