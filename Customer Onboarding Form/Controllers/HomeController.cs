using Customer_Onboarding_Form.Context;
using Customer_Onboarding_Form.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;


namespace Customer_Onboarding_Form.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly CustomerContext _context;

		public HomeController(ILogger<HomeController> logger, CustomerContext context)
		{
			_logger = logger;
			_context = context;
		}
        public class AccountController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;

            public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            
            [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAllUser"); // Redirect to another page after successful login
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }

                // If we got this far, something failed, redisplay the form
                return View(model);
            }
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index");
            }




        }


        public IActionResult Index()
		{
			return View();
		}
		// Controller to Register Users  
		public IActionResult Register_Customer()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register_Customer(UserModel user)
		{
			if (user == null)
			{
				return BadRequest("Please input your data.");
			}
			var userExist = await _context.Customers.FirstOrDefaultAsync(x => x.Email == user.Email);
			if (userExist != null)
			{
				return BadRequest("User already exist");
			}
			await _context.Customers.AddAsync(user);
			var save = await _context.SaveChangesAsync();
			if (save > 0)
			{
				//return RedirectToAction("Privacy");
				return RedirectToAction("Succes");
			}
			return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult Succes()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		//Controller to fetch the data of Customer and list them
		public async Task<IActionResult> GetAllUser()
		{
			var getUser = await _context.Customers.ToListAsync();
			ViewBag.UserList = getUser;
			return View();
		}
		//Controller to edit the data of Customer 
		[HttpGet]
		public async Task<IActionResult> UpdateDetails(int id)
		{
			var user = await _context.Customers.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			ViewBag.UserDetails = user;
			return View(user);
		}
		/*[HttpPost]
		public async Task<IActionResult> UpdateDetails(UserModel user)
		{
			if (ModelState.IsValid)
            {
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("GetAllUser");
            }
            return View(user);
        
		}*/
		[HttpPost]
		public async Task<IActionResult> UpdateDetails(UserModel user)
		{
			if (ModelState.IsValid)
			{
				if (user.Id > 0)
				{
					var userDetails = await _context.Customers.FindAsync(user.Id);

					if (userDetails == null)
					{
						return NotFound();
					}

					userDetails.FullName = user.FullName;
					userDetails.Email = user.Email;
					userDetails.Address = user.Address;
					userDetails.PhoneNumber = user.PhoneNumber;
					userDetails.DateOfBirth = user.DateOfBirth;
					userDetails.Bvn = user.Bvn;
					userDetails.Gender = user.Gender;
					userDetails.Country = user.Country;
					userDetails.State = user.State;

					_context.Customers.Update(userDetails);
					var isSaved = await _context.SaveChangesAsync() > 0;

					if (isSaved)
					{
						return RedirectToAction("GetAllUser");
					}
				}
			}

			// If ModelState is not valid or update fails, return the view with the user model
			return View(user);
		}

		/*public async Task<IActionResult> UpdateDetails(UserModel user)
		{
            if(user.Id != null)
            {
                var userDetails = await _context.Customers.FindAsync(user.Id);
                userDetails.FullName = user.FullName;
                userDetails.Email = user.Email;
                userDetails.Address = user.Address;
                userDetails.PhoneNumber = user.PhoneNumber;
                userDetails.DateOfBirth = user.DateOfBirth;
                userDetails.Bvn = user.Bvn;
                userDetails.Gender = user.Gender;
                userDetails.Country = user.Country;
                userDetails.State = user.State;
                _context.Customers.Update(userDetails);
                 var isSaved =await _context.SaveChangesAsync() > 0;
                if (isSaved)
                {
                    return RedirectToAction("GetAllUser");
                }
                else
                {
                    return View();
                }

            }
            else
            { 
                return View(); 
            }
           
        }
        */

		public async Task<IActionResult> GetUserDeatils(int id)
		{
			var user = _context.Customers.Find(id);
			if (user == null)
			{
				return NotFound();
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var user = await _context.Customers.FindAsync(id);
			if (user != null)
			{
				_context.Customers.Remove(user);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("GetAllUser");
		}



	}
}