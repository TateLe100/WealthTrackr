using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WealthTrackr.Data;
using WealthTrackr.Models;

namespace WealthTrackr.Controllers
{
    public class FinancialAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinancialAccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FinancialAccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.FinancialAccounts.ToListAsync());
        }

        // GET: FinancialAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialAccount = await _context.FinancialAccounts
                .FirstOrDefaultAsync(m => m.FinancialAccountId == id);
            if (financialAccount == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Create([Bind("FinancialAccountId,FkUserId,AccountName,Balance")] FinancialAccount financialAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(financialAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(financialAccount);
        }

        // GET: FinancialAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialAccount = await _context.FinancialAccounts.FindAsync(id);
            if (financialAccount == null)
            {
                return NotFound();
            }
            return View(financialAccount);
        }

        // POST: FinancialAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FinancialAccountId,FkUserId,AccountName,Balance")] FinancialAccount financialAccount)
        {
            if (id != financialAccount.FinancialAccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(financialAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinancialAccountExists(financialAccount.FinancialAccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(financialAccount);
        }

        // GET: FinancialAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialAccount = await _context.FinancialAccounts
                .FirstOrDefaultAsync(m => m.FinancialAccountId == id);
            if (financialAccount == null)
            {
                return NotFound();
            }

            return View(financialAccount);
        }

        // POST: FinancialAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var financialAccount = await _context.FinancialAccounts.FindAsync(id);
            if (financialAccount != null)
            {
                _context.FinancialAccounts.Remove(financialAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinancialAccountExists(int id)
        {
            return _context.FinancialAccounts.Any(e => e.FinancialAccountId == id);
        }
    }
}
