using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var categoriesList = _context.Categories.Where(c => c.FkAccountId == currentUser.Id).ToList();

            var financialAccount = _context.FinancialAccounts.Where(c => c.FkUserId == currentUser.Id).ToList();

            if (financialAccount.Count() == 0)
            {
                return RedirectToAction("Create", "FinancialAccounts");
            }

            if (categoriesList.Count() == 0)
            {
                return RedirectToAction("Create", "Categories");
            } 

            List<string> categoryName = [];
            foreach (var name in categoriesList)
            {
                categoryName.Add(name.CategoryName);
            }
            ViewBag.Categories = categoryName;
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionModel transaction)
        { 

            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return NotFound();
                }


                Transaction newTransaction = new Transaction
                {
                    TransactionId = transaction.TransactionId,
                    TransactionDate = transaction.TransactionDate,
                    FkAccountId = currentUser.Id,
                    Amount = transaction.Amount,
                    TransactionType = transaction.TransactionType,
                    Description = transaction.Description,
                };


                string type = transaction.TransactionType;
                double transactionAmount = transaction.Amount;
                string userID = currentUser.Id;

                await ProcessTransactionAsync(transactionAmount, userID, type);

                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();



                return RedirectToAction(nameof(MyTransactions));
            }
            else
            {
                ViewBag.Error = "Error, please try again later.";
                return View(transaction);
            }

        }
        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }

        // RECENT TRANSACTION 
        [Authorize]
        public async Task<IActionResult> MyTransactions()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var userTransactions = await _context.Transactions.Where(m => m.FkAccountId == currentUser.Id).ToListAsync();

            return View(userTransactions);
        }

        public async Task ProcessTransactionAsync(double amount, string Id, string type)
        {

            var financialAccount = await _context.FinancialAccounts.FirstOrDefaultAsync(m => m.FkUserId == Id);
            var transactionType = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == type && x.FkAccountId == Id);
            string plusORminus = transactionType.Type;
            double total = financialAccount.Balance;
            double newTotal = 0;
            if (plusORminus == "Expense")
            {
                newTotal = total - amount;
            }
            else
            {
                newTotal = total + amount;
            }

            financialAccount.Balance = newTotal;
            await _context.SaveChangesAsync();

        }

    }
}
