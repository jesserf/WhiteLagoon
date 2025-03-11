using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
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
        #region Login
        public IActionResult Login(string? returnUrl = null)
        {
            returnUrl??= Url.Content("~/"); //if url is null, set it to the root of the site

            LoginVM loginVM = new ()
            {
                RedirectUrl = returnUrl
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM obj)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, lockoutOnFailure:false); //Sign in the user

                if (result.Succeeded)
                {
                    TempData["success"] = $"Login successfully";
                    if (string.IsNullOrEmpty(obj.RedirectUrl)) //If the redirect url is null, redirect to the home page
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return LocalRedirect(obj.RedirectUrl); //Redirect to previous page
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }

            TempData["error"] = $"Login Failed";
            return View(obj);

        }
        #endregion
        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }
        #region Register
        public IActionResult Register(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/"); //if url is null, set it to the root of the site

            if (!_roleManager.RoleExistsAsync(SD.RoleAdmin).GetAwaiter().GetResult()) //if the role does not exist, then create the two roles, GetResult returns a boolean whilte GetAwaiter returns a task
            {
                _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin)).Wait(); //Will wait for the async method to finish
                _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer)).Wait();
            }

            RegisterVM registerVM = PopulateRoleList(new RegisterVM()); //Populate the role list

            registerVM.RedirectUrl = returnUrl; //puts returnurl to redirecturl
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM obj)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = obj.Name,
                    Email = obj.Email,
                    PhoneNumber = obj.PhoneNumber,
                    NormalizedEmail = obj.Email.ToUpper(), //Normalized Email = Email in uppercase
                    EmailConfirmed = true,
                    UserName = obj.Email,
                    CreatedDate = DateTime.Now //When the account was created
                };

                var result = _userManager.CreateAsync(user, obj.Password).GetAwaiter().GetResult(); //Create the user with the password

                if (result.Succeeded)
                {
                    TempData["success"] = $"Account created successfully";
                    if (!string.IsNullOrEmpty(obj.Role))
                    {
                        await _userManager.AddToRoleAsync(user, obj.Role); //Add the user to the role
                        //AddToRolesAsync can take a list of roles e.g. IEnumerables
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.RoleCustomer); //If no role is selected, add the user to the customer role
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false); //Sign in the user

                    if (string.IsNullOrEmpty(obj.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return LocalRedirect(obj.RedirectUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            TempData["error"] = $"Account could not be created";
            obj.RoleList = PopulateRoleList(obj).RoleList; //Populate the role list
            return View(obj);

        }
        #endregion
        private RegisterVM PopulateRoleList(RegisterVM obj)
        {
            obj.RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });

            return obj;
        }
    }
}
