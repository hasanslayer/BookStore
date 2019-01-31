using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository _repository;
        public int PageSize = 4;

        public BookController(IBookRepository bookRepository)
        {
            _repository = bookRepository;
        }

        public ViewResult List(int pageNo = 1)
        {
            return View(_repository.Books
                .OrderBy(b => b.ISBN)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
            );
        }

        public ViewResult ListAll()
        {
            return View(_repository.Books);
        }

    }
}