using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EbookTest.Data;
using EbookTest.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace EbookTest.Controllers
{
    public class EbooksController : Controller
    {
        private readonly ApplicationDBcontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EbooksController(ApplicationDBcontext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: Ebooks

        [Route("Home/Index")]

        public IActionResult Index()
        {
            List<Ebook> items = _context.ebooks.ToList();
            return View(items);
        }

        // GET: Ebooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ebooks == null)
            {
                return NotFound();
            }

            var ebook = await _context.ebooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ebook == null)
            {
                return NotFound();
            }

            return View(ebook);
        }

        // GET: Ebooks/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Create(Ebook eb)
        {
            string strinfFileName = UploadFile(eb);
            var book = new Ebook
            {
                Id = eb.Id,
                auteur = eb.auteur,
                CreatedDateTime = eb.CreatedDateTime,
                Description = eb.Description,
                DisplayOrder = eb.DisplayOrder,
                prix = eb.prix,
                stock = eb.stock,
                Title = eb.Title,
                ImageUrl = strinfFileName
            };
            _context.ebooks.Add(book);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string UploadFile(Ebook eb)
        {
            string? fileName = null;
            if (eb.BookImage != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "_" + eb.BookImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStreeam = new FileStream(filePath, FileMode.Create))
                {
                    eb.BookImage.CopyTo(fileStreeam);
                }
            }
            return fileName;
        }

        // POST: Ebooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Ebooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ebooks == null)
            {
                return NotFound();
            }

            var ebook = await _context.ebooks.FindAsync(id);
            if (ebook == null)
            {
                return NotFound();
            }
            return View(ebook);
        }

        // POST: Ebooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,auteur,Description,prix,DisplayOrder,CreatedDateTime,stock")] Ebook ebook)
        {
            if (id != ebook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ebook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EbookExists(ebook.Id))
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
            return View(ebook);
        }

        // GET: Ebooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ebooks == null)
            {
                return NotFound();
            }

            var ebook = await _context.ebooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ebook == null)
            {
                return NotFound();
            }

            return View(ebook);
        }

        // POST: Ebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ebooks == null)
            {
                return Problem("Entity set 'ApplicationDBcontext.ebooks'  is null.");
            }
            var ebook = await _context.ebooks.FindAsync(id);
            if (ebook != null)
            {
                _context.ebooks.Remove(ebook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EbookExists(int id)
        {
            return _context.ebooks.Any(e => e.Id == id);
        }
        [Authorize(Policy = "RequireAdministratorRole")]
        public IActionResult AdminOnly()
        {
            return View();
        }


    }
}
