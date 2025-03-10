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

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
