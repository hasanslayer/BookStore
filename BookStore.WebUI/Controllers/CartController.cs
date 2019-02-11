using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository _repository;
        private IOrderProcessor _orderProcessor;

        public CartController(IBookRepository repo, IOrderProcessor orderProcessor)
        {
            _repository = repo;
            _orderProcessor = orderProcessor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToChart(Cart cart, int isbn, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.ISBN == isbn);

            if (book != null)
            {
                cart.AddItem(book);
            }

            return RedirectToAction("Index", new { returnUrl }); //Index
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int isbn, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.ISBN == isbn);

            if (book != null)
            {
                cart.RemoveItem(book);
            }

            return RedirectToAction("Index", new { returnUrl }); //Index
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (!cart.CartLines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }
            if (ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}