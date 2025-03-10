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
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = $"Amenity {obj.Amenity.Name} created successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Amenity {obj.Amenity.Name} could not be created";
            obj = PopulateVillaNameList();
            return View(obj);
        }
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = PopulateVillaNameList(amenityId);
            if (amenityVM.Amenity is null)
            {
                TempData["error"] = "Amenity does not exist";
                return RedirectToAction("Error", "Home"); //redirects to error page
            }
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM obj)
        {
            if (ModelState.IsValid && obj.Amenity.Id > 0)
            {
                _unitOfWork.Amenity.UpdateAmenity(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = $"Amenity {obj.Amenity.Name} updated successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Amenity could not be updated";
            obj = PopulateVillaNameList();
            return View(obj);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = PopulateVillaNameList(amenityId);
            if (amenityVM.Amenity is null)
            {
                TempData["error"] = "Amenity does not exist";
                return RedirectToAction("Error", "Home"); //redirects to error page
            }
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM obj)
        {
            Amenity? objFromDb = _unitOfWork.Amenity.Get(u => u.Id == obj.Amenity.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Delete(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = $"Villa Number {obj.Amenity.Name} has been removed";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Amenity could not be deleted";
            return View();
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
