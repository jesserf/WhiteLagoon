using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }
        public IActionResult Create()
        {
            return View(PopulateVillaNameList());
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            bool isAmenityExist = _unitOfWork.Amenity.Any(u => u.Id == obj.Amenity.Id);
            if (ModelState.IsValid && !isAmenityExist)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = $"Amenity {obj.Amenity.Name} created successfully";
                return RedirectToAction(nameof(Index));
            }

            if (isAmenityExist)
            {
                ModelState.AddModelError("Amenity.Id", $"Amenity {obj.Amenity.Id} already exists");
            }
            TempData["error"] = $"Amenity {obj.Amenity.Name} could not be created";
            obj = PopulateVillaNameList();
            return View(obj);
        }

        private AmenityVM PopulateVillaNameList()
        {
            return new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
        }

        private AmenityVM PopulateVillaNameList(int number)
        {
            return new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == number)
            };
        }
    }
}
