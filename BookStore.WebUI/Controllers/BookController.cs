﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;

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

        public ViewResult List(string specialization, int pageNo = 1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = _repository.Books
                    .Where(b => specialization == null || b.Specialization == specialization)
                    .OrderBy(b => b.ISBN)
                    .Skip((pageNo - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNo,
                    ItemsPerPage = PageSize,
                    TotalItems = specialization == null ? _repository.Books.Count() : _repository.Books.Count(b => b.Specialization == specialization)
                },

                CurrentSpecialization = specialization
            };


            return View(model);
        }

        public ViewResult ListAll()
        {
            return View(_repository.Books);
        }

    }
}