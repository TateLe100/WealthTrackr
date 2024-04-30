using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WealthTrackr.Areas.Data;
using WealthTrackr.Models;

namespace WealthTrackr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            if (currentUser != null)
            {
                ViewBag.CurrentUser = currentUser.FirstName;
            }
            else
            {
                ViewBag.CurrentUser = "";
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            
            // check if user has account -> send through viewBag 
            //var recentUserTransactions = await _context.Transactions
            //                                       .Where(t => t.FkAccountId == currentUser.Id)
            //                                       .OrderByDescending(t => t.TransactionDate)
            //                                       .Take(5)
            //                                       .ToListAsync();
            //return View(recentUserTransactions);
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
