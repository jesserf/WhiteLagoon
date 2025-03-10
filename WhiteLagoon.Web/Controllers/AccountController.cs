using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager) //using rolemanager we can create roles
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            returnUrl??= Url.Content("~/"); //if url is null, set it to the root of the site

            LoginVM loginVM = new ()
            {
                RedirectUrl = returnUrl
            };
            return View();
        }
        public IActionResult Register()
        {
            if(!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult()) //if the role does not exist, then create the two roles, GetResult returns a boolean whilte GetAwaiter returns a task
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).Wait(); //Will wait for the async method to finish
                _roleManager.CreateAsync(new IdentityRole("Customer")).Wait();
            }

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                })
            };

            return View(registerVM);
        }
    }
}
