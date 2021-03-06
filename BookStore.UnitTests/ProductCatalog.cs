﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Html_Helper;
using BookStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookStore.UnitTests
{
    [TestClass]
    public class ProductCatalog
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
            {
                new Book() {ISBN = 1, Title = "Book1"},
                new Book() {ISBN = 2, Title = "Book2"},
                new Book() {ISBN = 3, Title = "Book3"},
                new Book() {ISBN = 4, Title = "Book4"},
                new Book() {ISBN = 5, Title = "Book5"}
            });

            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            BookListViewModel resultBooks = (BookListViewModel)controller.List(null, 1).Model;

            //Assert
            Book[] bookArray = resultBooks.Books.ToArray();
            Assert.IsTrue(bookArray.Length == 3);
            Assert.AreEqual(bookArray[0].Title, "Book1");
            Assert.AreEqual(bookArray[1].Title, "Book2");
            Assert.AreEqual(bookArray[2].Title, "Book3");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 14,
                ItemsPerPage = 5
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            String expectedResult = "<a class=\"btn btn-default\" href=\"Page1\">1</a>"
                                   + "<a class=\"btn btn-default btn-primary selected\" href=\"Page2\">2</a>"
                                   + "<a class=\"btn btn-default\" href=\"Page3\">3</a>";

            //Act
            String result = myHelper.PageLinks(pagingInfo, pageUrlDelegate).ToString();

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]
            {
                new Book() { ISBN = 1,Title = "Operation System"},
                new Book() { ISBN = 2,Title = "Web Application using ASP.NET"},
                new Book() { ISBN = 3,Title = "Android Mobile Applications"},
                new Book() { ISBN = 4,Title = "Database Systems"},
                new Book() { ISBN = 5,Title = "MIS"}
            });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            BookListViewModel result = (BookListViewModel)controller.List(null, 2).Model;

            //Assert
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);

        }

        [TestMethod]
        public void Can_Filter_Books()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book() { ISBN = 1,Title = "Operation System",Specialization = "CS"},
                    new Book() { ISBN = 2,Title = "Web Application using ASP.NET",Specialization = "IS"},
                    new Book() { ISBN = 3,Title = "Android Mobile Applications",Specialization = "IS"},
                    new Book() { ISBN = 4,Title = "Database Systems",Specialization = "IS"},
                    new Book() { ISBN = 5,Title = "MIS",Specialization = "IS"}
                });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            Book[] result = ((BookListViewModel)controller.List("IS", 2).Model).Books.ToArray();
            //Assert
            Assert.AreEqual(result.Length, 1);
            Assert.IsTrue(result[0].Title == "MIS" && result[0].Specialization == "IS");
        }

        [TestMethod]
        public void Can_Create_Specialization()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book() { ISBN = 1,Title = "Operation System",Specialization = "CS"},
                    new Book() { ISBN = 2,Title = "Web Application using ASP.NET",Specialization = "IS"},
                    new Book() { ISBN = 3,Title = "Android Mobile Applications",Specialization = "IS"},
                    new Book() { ISBN = 4,Title = "Database Systems",Specialization = "IS"},
                    new Book() { ISBN = 5,Title = "MIS",Specialization = "IS"}
                });
            NavController controller = new NavController(mock.Object);

            //Act
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();
            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0] == "CS" && result[1] == "IS");
        }

        [TestMethod]
        public void Indicate_Selected_Spec()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book() { ISBN = 1,Title = "Operation System",Specialization = "CS"},
                    new Book() { ISBN = 2,Title = "Web Application using ASP.NET",Specialization = "IS"},
                    new Book() { ISBN = 3,Title = "Android Mobile Applications",Specialization = "IS"},
                    new Book() { ISBN = 4,Title = "Database Systems",Specialization = "IS"},
                    new Book() { ISBN = 5,Title = "MIS",Specialization = "IS"}
                });
            NavController controller = new NavController(mock.Object);

            //Act
            string result = controller.Menu("IS").ViewBag.SelectedSpec;
            //Assert
            Assert.AreEqual("IS", result);
        }

        [TestMethod]
        public void Generate_Specialization_Specific_Book_Count()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book() { ISBN = 1,Title = "Operation System",Specialization = "CS"},
                    new Book() { ISBN = 2,Title = "Web Application using ASP.NET",Specialization = "IS"},
                    new Book() { ISBN = 3,Title = "Android Mobile Applications",Specialization = "IS"},
                    new Book() { ISBN = 4,Title = "Database Systems",Specialization = "CS"},
                    new Book() { ISBN = 5,Title = "MIS",Specialization = "IS"}
                });
            BookController controller = new BookController(mock.Object);

            //Act
            int result1 = ((BookListViewModel)controller.List("IS").Model).PagingInfo.TotalItems;
            int result2 = ((BookListViewModel)controller.List("CS").Model).PagingInfo.TotalItems;
            //Assert
            Assert.AreEqual(result1, 3);
            Assert.AreEqual(result2, 2);
        }
    }
}
