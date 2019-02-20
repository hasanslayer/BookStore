using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IBookRepository _repository;

        public AdminController(IBookRepository repository)
        {
            _repository = repository;
        }
        // GET: Admin
        public ViewResult Index()
        {
            return View(_repository.Books);
        }

        public ViewResult Edit(int isbn)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.ISBN == isbn);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveBook(book);
                TempData["message"] = book.Title + " has been saved"; // TempData finish when http reqeust finishing
                return RedirectToAction("Index");
            }
            else
            {
                // if not valid ,do something
                return View(book);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Book());
        }

        [HttpGet]
        public ActionResult Delete(int isbn)
        {
            Book deletedBook = _repository.DeleteBook(isbn);
            if (deletedBook != null)
            {
                TempData["message"] = deletedBook.Title + " was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}