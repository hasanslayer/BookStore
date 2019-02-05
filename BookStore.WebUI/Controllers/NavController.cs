using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;

namespace BookStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBookRepository _repository;

        public NavController(IBookRepository repo)
        {
            _repository = repo;
        }
        public PartialViewResult Menu(string specialization = null)
        {
            ViewBag.SelectedSpec = specialization;
            IEnumerable<string> spec = _repository.Books
                .Select(b => b.Specialization)
                .Distinct();

            return PartialView(spec);
        }
    }
}