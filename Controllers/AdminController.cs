using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthTrackr.Areas.Data;
using WealthTrackr.Data;
using WealthTrackr.ViewModels;

namespace WealthTrackr.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Get 
        public async Task<ActionResult> ViewUsers()
        {
            var users = _userManager.Users.ToList();
            List<ApplicationUser> CustomerList = [];
            List<ApplicationUser> AdminList = [];
            foreach (var user in users)
            {
                bool isInCustomerRole = await _userManager.IsInRoleAsync(user, "Customer");
                bool isInAdminRole = await _userManager.IsInRoleAsync(user, "Admin");
                if (isInCustomerRole)
                {
                    CustomerList.Add(user);
                }
                else if (isInAdminRole)
                {
                    AdminList.Add(user);
                }
            }

            ViewBag.CustomerList = CustomerList;
            ViewBag.AdminList = AdminList;
            return View();

        }

        public IActionResult CreateAdmin()
        {
            return View();
        }
        //POST: Create Agent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(ApplicationUserModel userView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = new ApplicationUser();
                    user.EmailConfirmed = true;
                    user.Email = userView.Email;
                    user.UserName = userView.Email;
                    user.FirstName = userView.FirstName;
                    user.LastName = userView.LastName;
                    user.PhoneNumber = userView.PhoneNumber;
                    user.RealPassword = userView.Password;
                    IdentityResult checkUser = await _userManager.CreateAsync(user, userView.Password);
                    bool x;
                    x = await _roleManager.RoleExistsAsync("Admin");
                    if (!x)
                    {
                        var role = new IdentityRole();
                        role.Name = "Admin";
                        await _roleManager.CreateAsync(role);
                        var identityResult = await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        var identityResult = await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    return RedirectToAction("ViewUsers", "Admin");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View(userView);
            }

        }



        //// GET: AdminController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: AdminController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AdminController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AdminController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: AdminController/Edit/5
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(ViewUsers));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View();
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return View();
            }
            return RedirectToAction("ViewUsers");
        }

        //// POST: AdminController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
