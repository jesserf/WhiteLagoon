﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = PopulateVillaNameList();
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            //ModelState.Remove("Villa");
            bool isVillaNumberExist = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            //bool isVillaNumberExist = _db.VillaNumbers.Count(u => u.Villa_Number == obj.VillaNumber.Villa_Number)==0;
            if (ModelState.IsValid && !isVillaNumberExist)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = $"Villa Number {obj.VillaNumber.Villa_Number} created successfully";
                return RedirectToAction("Index", "VillaNumber");
            }

            if (isVillaNumberExist)
            {
                ModelState.AddModelError("VillaNumber.Villa_Number", $"Villa Number {obj.VillaNumber.Villa_Number} already exists");
            }
            TempData["error"] = $"Villa Number {obj.VillaNumber.Villa_Number} could not be created";
            obj = PopulateVillaNameList();
            return View(obj);
        }

        private VillaNumberVM PopulateVillaNameList()
        {
            return new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            }; ;
        }
    }
}
