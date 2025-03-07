using Microsoft.EntityFrameworkCore;
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
    public class VillaRepository : Repository<Villa>, IVillaRepository //Inherits from Repository to get the generic implementation, and interface of IVillaRepository
    {

        private readonly ApplicationDbContext _db;
        //Implementation of database context
        public VillaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        //Specific implementation of save changes after modification of database
        public void Save()
        {
            _db.SaveChanges();
        }
        //Specific implementation to update selected villa
        public void UpdateVilla(Villa entity)
        {
            _db.Villas.Update(entity);
        }
    }
}
