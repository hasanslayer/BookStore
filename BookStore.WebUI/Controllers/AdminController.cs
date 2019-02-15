using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;

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
        public ActionResult Index()
        {
            return View(_repository.Books);
        }
    }
}