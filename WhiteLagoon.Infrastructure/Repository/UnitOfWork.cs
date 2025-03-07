﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        //preparing values for implementation
        private readonly ApplicationDbContext _db;
        public IVillaRepository Villa { get; private set; } 
        public UnitOfWork(ApplicationDbContext db)
        {
            //dependency injection of both database and villa repository
            _db = db;
            Villa = new VillaRepository(_db);
        } 
    }
}
