using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechNotebook.Models.ViewModels;

namespace TechNotebook.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        //register
        //login
        //logout
        public AuthController(UserManager<IdentityUser> manager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = manager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //check validation
            if(ModelState.IsValid)
            {
                //Created valid user object

                var user = new IdentityUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email     
                };
                //create user
                var res = await _userManager.CreateAsync(user,registerViewModel.Password);
                //if create user successfully
                if (res.Succeeded)
                {
                    //if user role exist in database
                   if(!await _roleManager.RoleExistsAsync("User"))
                    {
                       await _roleManager.CreateAsync(new IdentityRole("User"));
                    }
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: true);

                    return RedirectToAction("Index", "Home");

                }
            }
            return View(registerViewModel);
        }

		[HttpGet]
		public IActionResult Login()
		{
			
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					ModelState.AddModelError("", "Email or Password incorrect");
					return View(model);

				}

				var signinRes = await _signInManager.PasswordSignInAsync(
					user, model.Password, false, false);

				if (!signinRes.Succeeded)
				{
					ModelState.AddModelError("", "Email or Password incorrect");
					return View(model);
				}
				return RedirectToAction("Index", "Post");

			}
			return View(model);
		}

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Post");
        }
	}
}
