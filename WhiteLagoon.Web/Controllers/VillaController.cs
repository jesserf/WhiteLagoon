using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db; // Add ApplicationDbContext implementation
        //implementation of database or appdbcontext
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        //creates index page
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
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
                _db.Villas.Add(obj); //adds object to database
                _db.SaveChanges(); //confirms insertion
                return RedirectToAction("Index", "Villa"); //redirects to index page after insertion, (ActionName, ControllerName)
            }

            return View(); //returns view if model state is not valid
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);

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
                _db.Villas.Update(obj); //adds object to database
                _db.SaveChanges(); //confirms insertion
                return RedirectToAction("Index", "Villa"); //redirects to index page after insertion, (ActionName, ControllerName)
            }

            return View(); //returns view if model state is not valid
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);

            if (obj is null)
                return RedirectToAction("Error", "Home");

            return View(obj);
        }

        [HttpPost] //identifies post endpoint
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb is not null) //checks if value aligns with data annotations
            {
                _db.Villas.Remove(objFromDb); //adds object to database
                _db.SaveChanges(); //confirms insertion
                return RedirectToAction("Index", "Villa"); //redirects to index page after insertion, (ActionName, ControllerName)
            }

            return View(); //returns view if model state is not valid
        }
    }
}
