using BookCRUDOperationCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCRUDOperationCore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext db;
        public BooksController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        public JsonResult BookList()
        {
            return Json(new { data = db.Books.Where(u=>u.Deleted==false).ToList() });
        }

        public IActionResult BookOperation(int Id)
        {
            Book q = new Book();
            if (Id != 0)
            {
                 q = db.Books.FirstOrDefault(u => u.Id == Id);
                return View(q);
            }

            return View(q);
        }

        [HttpPost]
        public IActionResult BookOperation(Book data)
        {
            if (data.Id == 0)//adding
            {
                Book b = new Book();
                b.Author = data.Author;
                b.ISBN = data.ISBN;
                b.Name = data.Name;
                db.Books.Add(b);
                db.SaveChanges();
                return View("Index");
            }
            else//update
            {
                Book b = db.Books.FirstOrDefault(u => u.Id == data.Id);
                if (b == null)
                {
                    return NotFound();
                }
                b.Author = data.Author;
                b.ISBN = data.ISBN;
                b.Name = data.Name;
                db.SaveChanges();
                return View("Index");
            }
        }

        public JsonResult Delete(int Id)
        {
            var data = db.Books.FirstOrDefault(u => u.Id == Id);
            if (data == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            data.Deleted = true;
            db.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });

        }
    }
}
