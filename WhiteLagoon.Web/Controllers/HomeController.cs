using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        HomeVM homeVm = new HomeVM
        {
            VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities"),
            CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            Nights = 1
        };
        return View(homeVm);
    }
    [HttpPost]
    public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
    {
        var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities").ToList(); //get all villas

        foreach (var villa in villaList)
        {
            if (villa.Id % 2 == 0)
            {
                villa.isAvailable = false;
            }
        }

        HomeVM homeVM = new()
        {
            VillaList = villaList,
            CheckInDate = checkInDate,
            Nights = nights
        };

        return PartialView("_VillaList", homeVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
