using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WealthTrackr.Areas.Data;
using WealthTrackr.Data;
using WealthTrackr.Models;
using WealthTrackr.ViewModels;

namespace WealthTrackr.Controllers
{
    public class FinancialAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FinancialAccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FinancialAccounts
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            var currentUser = await _userManager.GetUserAsync(User);

            //var financialAccount = _context.FinancialAccounts.Find(currentUser.Id);
            var financialAccount = await _context.FinancialAccounts.FirstOrDefaultAsync(m => m.FkUserId == currentUser.Id);
            if (financialAccount == null)
            {
                return RedirectToAction(nameof(Create));
            }
            else
            {
                var accountID = financialAccount.FinancialAccountId;
                return RedirectToAction(nameof(Details), new { accountId = accountID });
            }
            


        }

        //GET: FinancialAccounts/Details/5
        public async Task<IActionResult> Details(int? accountId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var financialAccount = await _context.FinancialAccounts.FirstOrDefaultAsync(m => m.FinancialAccountId == accountId);
            if (financialAccount == null)
            {
                return RedirectToAction("Dashboard", "Home");
            }


            // list of categories 
            var categories = await _context.Categories.Where(x => x.FkAccountId == financialAccount.FkUserId).ToListAsync();
            // list of transactions 
            var transactions = await _context.Transactions.Where(x => x.FkAccountId == financialAccount.FkUserId).ToListAsync();


            // iterate through transaction 

            double totalIncome = 0;
            double totalExpense = 0;

            foreach (var transaction in transactions)
            {
                foreach (var category in categories)
                {
                    if (transaction.TransactionType.Equals(category.CategoryName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (category.Type == "Expense")
                        {
                            totalExpense += transaction.Amount;
                        }
                        else if(category.Type == "Income")
                        {
                            totalIncome += transaction.Amount;
                        }
                        
                    }
                }
            }

            var recentUserTransactions = await _context.Transactions
                                                   .Where(t => t.FkAccountId == currentUser.Id)
                                                   .OrderByDescending(t => t.TransactionDate)
                                                   .Take(5)
                                                   .ToListAsync();


            

            //collect list of users
            var users = _userManager.Users.ToList();
            //iterate through users
            foreach (var user in users)
            {
                // if financialAccountId = userId 
                if (user.Id == financialAccount.FkUserId)
                {
                    ViewBag.RecentTransactions = recentUserTransactions;
                    ViewBag.TotalIncome = totalIncome;
                    ViewBag.TotalExpense = totalExpense;
                    ViewBag.AccountFirstName = user.FirstName;
                    ViewBag.AccountLastName = user.LastName;
                    ViewBag.AccountEmail = user.Email;
                }
            }




            // TODO: Put recent transactions to front end, graph possibly, 2buttons FRONT END STUFF 

            return View(financialAccount);
        }

        // GET: FinancialAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FinancialAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FiancialAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                FinancialAccount newAccount = new FinancialAccount
                {
                    FkUserId = currentUser.Id,
                    AccountName = model.AccountName,
                    Balance = model.Balance,
                };


                _context.Add(newAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { accountId = model.FinancialAccountId });
            }
            else
            {
                ViewBag.Error = "Error, please try again later.";
                return View();
            }
        }

        // GET: FinancialAccounts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var financialAccount = await _context.FinancialAccounts.FindAsync(id);
        //    if (financialAccount == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(financialAccount);
        //}

        //// POST: FinancialAccounts/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("FinancialAccountId,FkUserId,AccountName,Balance")] FinancialAccount financialAccount)
        //{
        //    if (id != financialAccount.FinancialAccountId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(financialAccount);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FinancialAccountExists(financialAccount.FinancialAccountId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(financialAccount);
        //}

        //// GET: FinancialAccounts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var financialAccount = await _context.FinancialAccounts
        //        .FirstOrDefaultAsync(m => m.FinancialAccountId == id);
        //    if (financialAccount == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(financialAccount);
        //}

        //// POST: FinancialAccounts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var financialAccount = await _context.FinancialAccounts.FindAsync(id);
        //    if (financialAccount != null)
        //    {
        //        _context.FinancialAccounts.Remove(financialAccount);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool FinancialAccountExists(int id)
        {
            return _context.FinancialAccounts.Any(e => e.FinancialAccountId == id);
        }

        //public async IActionResult MyAccount()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    if (currentUser == null)
        //    {
        //        return RedirectToAction(nameof(Create));
        //    }
        //    else
        //    {
        //        //var financialAccount = _context.FinancialAccounts.Find(currentUser.Id);
        //        var financialAccount = await _context.FinancialAccounts.FirstOrDefaultAsync(m => m.FkUserId == currentUser.Id);
        //        return Details(financialAccount);

        //    }
        //}

    }
}
