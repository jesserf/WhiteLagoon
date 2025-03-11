﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IUnitOfWork //acts as a repository wrapper
    {
        IVillaRepository Villa { get; }
        IVillaNumberRepository VillaNumber { get; }
        IBookingRepository Booking { get; }
        IAmenityRepository Amenity { get; }
        IApplicationUserRepository User { get; }
        void Save();
    }
}
