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
        private readonly IWebHostEnvironment _webHostEnvironment; // add WebHostEnvironment for file uploads
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork; //Dependency Injection
            _webHostEnvironment = webHostEnvironment; //Dependency Injection
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
                AddImages(obj); //adds image to object

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
                AddImages(obj, true); //adds image to object
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
                RemoveOldImage(objFromDb); //removes old image
                _unitOfWork.Villa.Delete(objFromDb); //adds object to database
                _unitOfWork.Save(); //confirms insertion
                TempData["success"] = $"Villa {objFromDb.Name} deleted successfully"; //Notification for successful deletion
                return RedirectToAction(nameof(Index)); //redirects to index page after insertion, (ActionName, ControllerName)
            }
            TempData["error"] = "Villa could not be deleted"; //Notification for failure to delete

            return View(); //returns view if model state is not valid
        }

        private void AddImages(Villa obj, bool? isUpdate = false)
        {
            if (obj.Image is not null) //if there is an image upload
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName); //renames file name to random guid, you can add more logic to rename the file, such as accepting png but rejecting jpg, get extension is used to get file name extension
                //if image is valid
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage"); //creates path for image uploads

                if(isUpdate == true) //if it is an update
                    RemoveOldImage(obj); //maintains image

                //to add the image
                using var fileStream = new FileStream(Path.Combine(imagePath, filename), FileMode.Create); //creates file stream
                obj.Image.CopyTo(fileStream); //copies image to file stream

                obj.ImageUrl = @"\images\VillaImage\" + filename; //sets image url to file path
            }
            else if (isUpdate == false) //if there is no image upload and it is an update
            {
                obj.ImageUrl = "https://via.placehold.co/600x400"; //sets image url to placeholder image
            }
        }

        private void RemoveOldImage(Villa obj)
        {
            if (!string.IsNullOrEmpty(obj.ImageUrl)) //if there is an image upload
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\')); //gets old path of image and removes backslashes
                
                if (System.IO.File.Exists(oldImagePath)) //checks if file exists
                {
                    System.IO.File.Delete(oldImagePath); //deletes file
                }
            }
        }
    }
}
