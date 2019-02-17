﻿using System;
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
    }
}