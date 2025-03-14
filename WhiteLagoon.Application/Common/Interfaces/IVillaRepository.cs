﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;
namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IVillaRepository : IRepository<Villa> //Inherits from IRepository to get the generic implementation
    {
        //Generic implementation to retrieve all villas
        void UpdateVilla(Villa entity);
    }
}
