﻿using System;
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
    [Authorize]
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


            double totalIncome = 0;
            double totalExpense = 0;

            foreach (var transaction in transactions)
            {
                // iterate through transactions 
                foreach (var category in categories)
                {
                    // every transaction check if the categoryName is equal
                    // if the type and name are equal check that category to see if expense or income
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


        private bool FinancialAccountExists(int id)
        {
            return _context.FinancialAccounts.Any(e => e.FinancialAccountId == id);
        }


    }
}
