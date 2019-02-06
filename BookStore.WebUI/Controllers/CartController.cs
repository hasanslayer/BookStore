using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository _repository;

        public CartController(IBookRepository repo)
        {
            _repository = repo;
        }

        public RedirectToRouteResult AddToChart(int isbn, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.ISBN == isbn);

            if (book != null)
            {
                GetCart().AddItem(book);
            }

            return RedirectToAction("", new { returnUrl }); //Index
        }

        public RedirectToRouteResult RemoveFromCart(int isbn, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.ISBN == isbn);

            if (book != null)
            {
                GetCart().RemoveItem(book);
            }

            return RedirectToAction("", new { returnUrl }); //Index
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];

            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}