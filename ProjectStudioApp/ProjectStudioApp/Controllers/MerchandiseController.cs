using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectStudioApp.Datafile;
using ProjectStudioApp.Models;

namespace ProjectStudioApp.Controllers
{
    public class MerchandiseController : Controller
    {
        private readonly ZooliranteDbContext _context;

        public MerchandiseController(ZooliranteDbContext context)
        {
            _context = context;
        }

        // GET: Merchandise
        public async Task<IActionResult> Index()
        {
            return View(await _context.Merchandises.ToListAsync());
        }

        // GET: Merchandise/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandises
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (merchandise == null)
            {
                return NotFound();
            }

            return View(merchandise);
        }

        // GET: Merchandise/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Merchandise/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,ItemDescription,ItemCost,ItemImage")] Merchandise merchandise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(merchandise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(merchandise);
        }

        // GET: Merchandise/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandises.FindAsync(id);
            if (merchandise == null)
            {
                return NotFound();
            }
            return View(merchandise);
        }

        // POST: Merchandise/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,ItemDescription,ItemCost,ItemImage")] Merchandise merchandise)
        {
            if (id != merchandise.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchandise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchandiseExists(merchandise.ItemId))
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
            return View(merchandise);
        }

        // GET: Merchandise/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandises
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (merchandise == null)
            {
                return NotFound();
            }

            return View(merchandise);
        }

        // POST: Merchandise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var merchandise = await _context.Merchandises.FindAsync(id);
            if (merchandise != null)
            {
                _context.Merchandises.Remove(merchandise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MerchandiseExists(int id)
        {
            return _context.Merchandises.Any(e => e.ItemId == id);
        }
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var merchandise = _context.Merchandises.Find(id);
            if (merchandise == null)
                return NotFound();

            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var cartItem = cart.FirstOrDefault(c => c.ItemId == id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ItemId = merchandise.ItemId,
                    ItemName = merchandise.ItemName,
                    ItemCost = merchandise.ItemCost,
                    ItemImage = merchandise.ItemImage,
                    Quantity = 1
                });
            }
            CartSessionHelper.SaveCart(HttpContext.Session, cart);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            var item = cart.FirstOrDefault(c => c.ItemId == id);
            if (item != null)
            {
                cart.Remove(item);
                CartSessionHelper.SaveCart(HttpContext.Session, cart);
            }
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            return View(cart);
        }

        public IActionResult Purchase()
        {
            var cart = CartSessionHelper.GetCart(HttpContext.Session);
            if (cart.Count == 0)
                return RedirectToAction("Cart");
            return View(cart);
        }

        [HttpPost]
        public IActionResult FinalisePurchase()
        {
            CartSessionHelper.ClearCart(HttpContext.Session);
            return View("PurchaseConfirmation");
        }
    }
}
