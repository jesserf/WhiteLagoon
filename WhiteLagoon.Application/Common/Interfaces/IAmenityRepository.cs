﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        //Generic implementation to retrieve all amenities
        void UpdateAmenity(Amenity entity);
    }
}
