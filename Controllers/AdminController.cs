using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthTrackr.Areas.Data;
using WealthTrackr.Data;

namespace WealthTrackr.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Get 
        public ActionResult ViewUsers()
        {
            //List<ApplicationUser> accountUsers = _context.Users.ToList();
            //return View(accountUsers);
            var users = _userManager.Users.ToList();
            //List<ApplicationUser> CustomerList = [];
            //List<ApplicationUser> AgentList = [];
            //List<ApplicationUser> AdminList = [];
            //foreach (var user in users)
            //{
            //    bool isInCustomerRole = await _userManager.IsInRoleAsync(user, "Customer");
            //    bool isInAgentRole = await _userManager.IsInRoleAsync(user, "Agent");
            //    if (isInCustomerRole)
            //    {
            //        CustomerList.Add(user);
            //    }
            //    else if (isInAgentRole)
            //    {
            //        AgentList.Add(user);
            //    }
            //    else
            //    {
            //        AdminList.Add(user);
            //    }
            //}
            //ViewBag.CustomerList = CustomerList;
            ViewBag.userList = users;
            return View();

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

        //// GET: AdminController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AdminController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
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

        //// GET: AdminController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

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
