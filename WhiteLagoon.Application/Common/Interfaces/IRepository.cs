using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        IEnumerable<T> GetAllVillas(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T GetVilla(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void AddVilla(T entity);
        void DeleteVilla(T entity);
    }
}
