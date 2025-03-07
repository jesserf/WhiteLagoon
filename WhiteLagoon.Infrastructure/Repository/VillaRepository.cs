using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {

        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddVilla(Villa entity)
        {
            _db.Add(entity);
        }

        public void DeleteVilla(Villa entity)
        {
            _db.Remove(entity);
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
            _db.SaveChanges();
        }

        public void UpdateVilla(Villa entity)
        {
            _db.Villas.Update(entity);
        }
    }
}
