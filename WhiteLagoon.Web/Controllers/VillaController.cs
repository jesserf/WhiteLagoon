using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo; // Add ApplicationDbContext implementation
        //implementation of database or appdbcontext
        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
        }
        //creates index page
        public IActionResult Index()
        {
            var villas = _villaRepo.GetAll();
            return View(villas);
        }
        //creates a create form
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //identifies post endpoint
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description) //checks if name and description are the same
            {
                ModelState.AddModelError("Description", "Name and Description cannot be the same"); //adds error message to model state
            }
            if (ModelState.IsValid) //checks if value aligns with data annotations
            {
                _villaRepo.Add(obj); //adds object to database
                _villaRepo.Save(); //confirms insertion
                TempData["success"] = $"Villa {obj.Name} created successfully";
                return RedirectToAction(nameof(Index)); //redirects to index page after insertion, (ActionName, ControllerName)
            }
            TempData["error"] = $"Villa could not be created";

            return View(); //returns view if model state is not valid
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _villaRepo.Get(u => u.Id == villaId);

            if (obj == null)
                return RedirectToAction("Error", "Home");

            return View(obj);
        }

        [HttpPost] //identifies post endpoint
        public IActionResult Update(Villa obj)
        {
            if (obj.Name == obj.Description) //checks if name and description are the same
            {
                ModelState.AddModelError("Description", "Name and Description cannot be the same"); //adds error message to model state
            }
            if (ModelState.IsValid && obj.Id>0) //checks if value aligns with data annotations
            {
                _villaRepo.UpdateVilla(obj); //adds object to database
                _villaRepo.Save(); //confirms insertion
                TempData["success"] = $"Villa {obj.Name} updated successfully";
                return RedirectToAction(nameof(Index)); //redirects to index page after insertion, (ActionName, ControllerName)
            }

            TempData["error"] = $"Villa could not be updated";
            return View(); //returns view if model state is not valid
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaRepo.Get(u => u.Id == villaId);

            if (obj is null)
                return RedirectToAction("Error", "Home");

            return View(obj);
        }

        [HttpPost] //identifies post endpoint
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _villaRepo.Get(u => u.Id == obj.Id);
            if (objFromDb is not null) //checks if value aligns with data annotations
            {
                _villaRepo.Delete(objFromDb); //adds object to database
                _villaRepo.Save(); //confirms insertion
                TempData["success"] = $"Villa {objFromDb.Name} deleted successfully";
                return RedirectToAction(nameof(Index)); //redirects to index page after insertion, (ActionName, ControllerName)
            }
            TempData["error"] = $"Villa could not be deleted";

            return View(); //returns view if model state is not valid
        }
    }
}
