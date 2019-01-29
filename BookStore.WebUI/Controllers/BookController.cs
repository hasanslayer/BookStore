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

        public BookController(IBookRepository bookRepository)
        {
            _repository = bookRepository;
        }

        public ViewResult List()
        {
            return View(_repository.Books);
        }
    }
}