using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniInventory_Management.DB;
using MiniInventory_Management.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniInventory_Management.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TaskDbContext _context;

        public ProductsController(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string category)
        {
            var categories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync();
            ViewBag.Categories = new SelectList(categories);
            var products = from p in _context.Products select p;
            if (!string.IsNullOrEmpty(searchString)) products = products.Where(p => p.Name.Contains(searchString));
            if (!string.IsNullOrEmpty(category)) products = products.Where(p => p.Category == category);
            return View(await products.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Category,Price,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();
            return PartialView("/Views/Shared/_ProductDetailsPartial.cshtml", product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return PartialView("/Views/Shared/_EditProductPartial.cshtml", product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Price,Quantity")] Product product)
        {
            if (id != product.Id)
            {
                return Json(new { success = false, errors = new[] { "Product ID mismatch." } });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errors = new[] { "An unexpected error occurred: " + ex.Message } });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
            return Json(new { success = false, errors = errors });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();
            return PartialView("/Views/Shared/_DeleteProductPartial.cshtml", product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errors = new[] { "An error occurred while deleting: " + ex.Message } });
            }
        }

        private bool ProductExists(int id) => _context.Products.Any(e => e.Id == id);
    }
}