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

        public IActionResult Update(Villa obj)
        {
            _db.Remove(obj.Id); //deletes object from database

            return View(); //returns view if model state is not valid
        }
        public IActionResult Delete(Villa obj)
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
    }
}
