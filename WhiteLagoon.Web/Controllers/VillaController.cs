using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // add Repository Wrapper
        //implementation of database or appdbcontext
        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; //Dependency Injection
        }
        //creates index page
        public IActionResult Index() //IActionResult is a global return type for action methods
        {
            var villas = _unitOfWork.Villa.GetAll(); //gets all villas from database
            return View(villas); //returns view with villas
        }
        //creates a create form
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //identifies post endpoint, happens when form is submitted
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description) //checks if name and description are the same
            {
                ModelState.AddModelError("Description", "Name and Description cannot be the same"); //adds error message to model state
            }
            if (ModelState.IsValid) //checks if value aligns with data annotations
            {
                _unitOfWork.Villa.Add(obj); //adds object to database
                _unitOfWork.Save(); //confirms insertion
                TempData["success"] = $"Villa {obj.Name} created successfully"; //Notification for successful creation
                return RedirectToAction(nameof(Index)); //redirects to index page after insertion, (ActionName, ControllerName)
            }
            TempData["error"] = $"Villa could not be created"; //Notification for failure to create

            return View(); //returns view if model state is not valid
        }
        //creates an update form
        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId); //gets object from database where asp-route-villaId is equal to db Id

            if (obj == null)
                return RedirectToAction("Error", "Home"); //redirects to error page if object is null

            return View(obj);
        }

        [HttpPost] //identifies post endpoint, happens when form is submitted
        public IActionResult Update(Villa obj)
        {
            if (obj.Name == obj.Description) //checks if name and description are the same
            {
                ModelState.AddModelError("Description", "Name and Description cannot be the same"); //adds error message to model state
            }
            if (ModelState.IsValid && obj.Id>0) //checks if value aligns with data annotations
            {
                _unitOfWork.Villa.UpdateVilla(obj); //adds object to database
                _unitOfWork.Save(); //confirms insertion
                TempData["success"] = $"Villa {obj.Name} updated successfully";
                return RedirectToAction(nameof(Index)); //redirects to index page after insertion, (ActionName, ControllerName)
            }

            TempData["error"] = $"Villa could not be updated";
            return View(); //returns view if model state is not valid
        }
        //creates a delete form
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId); //gets object from database where asp-route-villaId is equal to db Id

            if (obj is null)
                return RedirectToAction("Error", "Home");

            return View(obj);
        }

        [HttpPost] //identifies post endpoint, happens when form is submitted
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == obj.Id); //gets object from database where asp-route-villaId is equal to db Id
            if (objFromDb is not null) //checks if value aligns with data annotations
            {
                _unitOfWork.Villa.Delete(objFromDb); //adds object to database
                _unitOfWork.Save(); //confirms insertion
                TempData["success"] = $"Villa {objFromDb.Name} deleted successfully"; //Notification for successful deletion
                return RedirectToAction(nameof(Index)); //redirects to index page after insertion, (ActionName, ControllerName)
            }
            TempData["error"] = "Villa could not be deleted"; //Notification for failure to delete

            return View(); //returns view if model state is not valid
        }
    }
}
